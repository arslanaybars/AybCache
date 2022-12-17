# AybCache

This library helps you to cache your services responses with aspect-oriented approach 

# How to use AybCache

1. Add ``IDistributedCache`` to your IoC

Here is simple example of injecting ``IDistributedCache`` as *Redis*
```csharp
var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString");  
builder.Services.AddDistributedRedisCache(options =>  
{  
    options.Configuration = redisConnectionString;  
});  
  
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConnectionString));
```

2. Add **services.AddAybCache()** in program.cs
```csharp
    builder.Services.AddAybCache();
```
3. Add proxied services with using ``AddProxiedScoped()``
```csharp
// Repository example
builder.Services.AddProxiedScoped<IProductRepository, ProductRepository>();  
  
// HttpClient example  
builder.Services.AddProxiedScoped<IAgifyHttpClient, AgifyHttpClient>();  

// Mediator example  
builder.Services.AddProxiedScoped<IRequestHandler<GetNameQuery, string>, GetNameQueryHandler>();

```
4. Add the ``[AybCache]`` attribute which method you want to cache.
```csharp
[AybCache(Seconds = 600, CacheKey = CacheKeys.Repository.Products)]  
public async Task<List<Product>> GetProducts()  
{  
    Thread.Sleep(5000);  
    return await Task.Run(() => Task.FromResult(_products.ToList()));  
}  
  
[AybCache(Seconds = 600, CacheKey = CacheKeys.Repository.ProductsByBrand)]  
public async Task<List<Product>> GetProductsByBrand(string brand)  
{  
    Thread.Sleep(5000);  
    return await Task.Run(() => Task.FromResult(_products.Where(x => x.Brand == brand).ToList()));  
}
```
### Sample
To further understand the library, please review the Sample API project.

### How we generate cache keys?
We have cache key generate login in ``CacheKeyGenerator.GenerateCacheKey()`` method. 

Example
Consider the following example of a defined cache keys:
```csharp
public static class CacheKeys  
{  
    public static class Repository  
    {  
        public const string Products = "repository:products";  
        public const string ProductsByBrand = "repository:products:{0}";  
    }  
     
    public static class Mediator  
    {  
        public const string Name = "mediator:name:{0}";  
    }  
}
```
These are the techniques we wish to cache.
```csharp
// repository:products:{0}
// repository:products:{{brand}} used as cache key
[AybCache(Seconds = 600, CacheKey = CacheKeys.Repository.ProductsByBrand)]  
public async Task<List<Product>> GetProductsByBrand(string **brand**)  
{  
    Thread.Sleep(5000);  
    return await Task.Run(() => Task.FromResult(_products.Where(x => x.Brand == brand).ToList()));  
}
```
Or for strongly typed params
```csharp
// mediator:name:{0}
// mediator:name:{{GetNameQuery.CacheKey}} used as cache key
// ICacheKeyHolder give us the cache key prop that we used this field for generating the cache key
[AybCache(Seconds = 6000, CacheKey = CacheKeys.Mediator.Name)]  
public async Task<string> Handle(GetNameQuery request, CancellationToken cancellationToken)  
{  
    return await Task.Run(() => Task.FromResult(request.Name), cancellationToken);  
}
```
```csharp
public class GetNameQuery : IRequest<string>, ICacheKeyHolder  
{  
    public string Name { get; set; }  
  
    public string CacheKey => Name;  
}
```
