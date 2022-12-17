namespace AybCache;

[AttributeUsage(AttributeTargets.Method)]
public class AybCacheAttribute : Attribute
{
    public int Seconds { get; set; } = 300;
    public string CacheKey { get; set; }
}