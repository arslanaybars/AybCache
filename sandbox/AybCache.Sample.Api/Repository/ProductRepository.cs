namespace AybCache.Sample.Api.Repository;

public class ProductRepository : IProductRepository
{
    private readonly List<Product> _products;

    public ProductRepository()
    {
        _products = new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "M1 Macbook Pro",
                Brand = "Apple"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Thinkpad E15",
                Brand = "Lenova"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Abra v5.1",
                Brand = "Monster"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "M2 Macbook Air",
                Brand = "Apple"
            }
        };
    }
    
    
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
}