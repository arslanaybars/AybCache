namespace AybCache.Sample.Api;

public static class CacheKeys
{
    public static class Repository
    {
        public const string Products = "repository:products";
        public const string ProductsByBrand = "repository:products:{0}";
    }

    public static class Client
    {
        public const string AgifyWithName = "client:agify:{0}";
    }

    public static class Mediator
    {
        public const string Name = "mediator:name:{0}";
    }
}