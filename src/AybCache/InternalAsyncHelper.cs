namespace AybCache;

internal static class InternalAsyncHelper
{
    public static object CallAwaitTaskWithPostActionAndFinallyAndGetResult<T>(Type taskReturnType, object actualReturnValue, Func<T ,Task> action, Action<Exception> finalAction)
    {
        return typeof (InternalAsyncHelper)
            .GetMethod(nameof(AwaitTaskWithPostActionAndFinallyAndGetResult), BindingFlags.Public | BindingFlags.Static)
            ?.MakeGenericMethod(taskReturnType)
            .Invoke(null, new object[] { actualReturnValue, action, finalAction });
    }
    
    public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetResult<T>(Task<T> actualReturnValue, Func<T, Task> postAction, Action<Exception> finalAction)
    {
        Exception exception = null;

        try
        {
            var result = await actualReturnValue;
            await postAction(result);
            return result;
        }
        catch (Exception ex)
        {
            exception = ex;
            throw;
        }
        finally
        {
            finalAction(exception);
        }
    }
}