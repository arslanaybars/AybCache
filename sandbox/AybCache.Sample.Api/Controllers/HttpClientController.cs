namespace AybCache.Sample.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HttpClientController : ControllerBase
{
    private readonly IAgifyHttpClient _agifyHttpClient;

    public HttpClientController(IAgifyHttpClient agifyHttpClient)
    {
        _agifyHttpClient = agifyHttpClient;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        return Ok(await _agifyHttpClient.GetAgify(id));
    }
}