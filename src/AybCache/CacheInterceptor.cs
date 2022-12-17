using Microsoft.Extensions.Caching.Distributed;

namespace AybCache;

public class CacheInterceptor : IInterceptor
{
    private readonly IDistributedCache _distributedCache;

    public CacheInterceptor(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache)); ;
    }

    public void Intercept(IInvocation invocation)
    {
        var cacheAttribute = invocation.MethodInvocationTarget
            .GetCustomAttributes(typeof(AybCacheAttribute), false)
            .FirstOrDefault() as AybCacheAttribute;

        if (cacheAttribute == null)
        {
            invocation.Proceed();
            return;
        }

        var cacheKey = CacheKeyGenerator.GenerateCacheKey(cacheAttribute.CacheKey, invocation.Arguments);

        if (string.IsNullOrEmpty(cacheKey))
        {
            invocation.Proceed();
            return;
        }

        var data = _distributedCache.Get(cacheKey);

        if (data != null)
        {
            if (!invocation.Method.IsAsyncMethod())
            {
                invocation.ReturnValue = data.ByteArrayToObject(invocation.Method.ReturnType);
                return;
            }

            var type = invocation.Method.ReturnType.GenericTypeArguments[0];
            var method = typeof(Task).GetMethod(nameof(Task.FromResult), BindingFlags.Public | BindingFlags.Static)
                ?.MakeGenericMethod(type);
            if (method != null)
            {
                invocation.ReturnValue = method.Invoke(null, new object[] { data.ByteArrayToObject(type) });
            }

            return;
        }

        invocation.Proceed();

        if (invocation.Method.IsAsyncMethod())
        {
            invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                invocation.Method.ReturnType.GenericTypeArguments[0],
                invocation.ReturnValue,
                async (object abv) =>
                {
                    await _distributedCache.SetAsync(cacheKey, abv.ObjectToByteArray(), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheAttribute.Seconds)
                    });
                },
                ex => { });
        }
        else
        {
            _distributedCache.Set(cacheKey, invocation.ReturnValue.ObjectToByteArray(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheAttribute.Seconds)
            });
        }
    }
}