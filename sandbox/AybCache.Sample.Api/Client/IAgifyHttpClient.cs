namespace AybCache.Sample.Api.Client;

public interface IAgifyHttpClient
{
    Task<Agify> GetAgify(string name, CancellationToken cancellationToken = default);

}