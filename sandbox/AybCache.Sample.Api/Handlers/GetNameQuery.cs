namespace AybCache.Sample.Api.Handlers;

public class GetNameQuery : IRequest<string>, ICacheKeyHolder
{
    public string Name { get; set; }

    public string CacheKey => Name;
}