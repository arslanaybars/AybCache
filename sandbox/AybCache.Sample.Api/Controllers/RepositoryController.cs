namespace AybCache.Sample.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RepositoryController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    
    public RepositoryController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _productRepository.GetProducts());
    }
    
    [HttpGet("filter")]
    public async Task<IActionResult> GetProductByBrand([FromQuery]string brand)
    {
        return Ok(await _productRepository.GetProductsByBrand(brand));
    }
}