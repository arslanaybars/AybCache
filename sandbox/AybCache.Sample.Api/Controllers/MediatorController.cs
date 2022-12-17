namespace AybCache.Sample.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MediatorController : ControllerBase
{
    private readonly IMediator _mediator;

    public MediatorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> Get([FromRoute] GetNameQuery query)
    {
        return Ok(await _mediator.Send(query));
    }
}