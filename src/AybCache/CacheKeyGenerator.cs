namespace AybCache;

public static class CacheKeyGenerator
{
    public static string GenerateCacheKey(string cacheKey, object[] arguments)
    {
        try
        {
            if (!arguments.Any())
            {
                return cacheKey;
            }
            
            if (arguments[0] is string str)
            {
                return string.Format(cacheKey, str.ToLower());
            }

            if (arguments[0] is List<string> stringArguments)
            {
                return string.Format(cacheKey,
                    string.Join(",", stringArguments.Select(x => x.ToLower()).OrderBy(q => q)));
            }

            if (arguments[0] is ICacheKeyHolder cacheKeyHolder)
            {
                return string.Format(cacheKey, cacheKeyHolder.CacheKey);
            }

        }
        catch
        {
            // ignored
        }

        return null;
    }
}