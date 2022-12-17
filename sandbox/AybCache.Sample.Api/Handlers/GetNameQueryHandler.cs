namespace AybCache.Sample.Api.Handlers;

public class GetNameQueryHandler : IRequestHandler<GetNameQuery, string>
{
    [AybCache(Seconds = 6000, CacheKey = CacheKeys.Mediator.Name)]
    public async Task<string> Handle(GetNameQuery request, CancellationToken cancellationToken)
    {
        Thread.Sleep(4000);
        return await Task.Run(() => Task.FromResult(request.Name), cancellationToken);
    }
}