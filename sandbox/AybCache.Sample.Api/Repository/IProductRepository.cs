namespace AybCache.Sample.Api.Repository;

public interface IProductRepository
{
    public Task<List<Product>> GetProducts();
    public Task<List<Product>> GetProductsByBrand(string brand);
}