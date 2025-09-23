namespace AybCache;

public static class CacheKeyGenerator
{
    public static string GenerateCacheKey(string cacheKey, object[] arguments)
    {
        try
        {
            if (IsArgumentsEmpty(arguments))
            {
                return cacheKey;
            }

            var firstArgument = arguments[0];
            
            return firstArgument switch
            {
                string str => FormatCacheKeyWithString(cacheKey, str),
                List<string> stringList => FormatCacheKeyWithStringList(cacheKey, stringList),
                ICacheKeyHolder cacheKeyHolder => FormatCacheKeyWithCacheKeyHolder(cacheKey, cacheKeyHolder),
                _ => null
            };
        }
        catch
        {
            // ignored
            return null;
        }
    }

    private static bool IsArgumentsEmpty(object[] arguments)
    {
        return !arguments.Any();
    }

    private static string FormatCacheKeyWithString(string cacheKey, string str)
    {
        return string.Format(cacheKey, str.ToLower());
    }

    private static string FormatCacheKeyWithStringList(string cacheKey, List<string> stringList)
    {
        var sortedLowerCaseValues = string.Join(",", 
            stringList.Select(x => x.ToLower()).OrderBy(q => q));
        
        return string.Format(cacheKey, sortedLowerCaseValues);
    }

    private static string FormatCacheKeyWithCacheKeyHolder(string cacheKey, ICacheKeyHolder cacheKeyHolder)
    {
        return string.Format(cacheKey, cacheKeyHolder.CacheKey);
    }
}
