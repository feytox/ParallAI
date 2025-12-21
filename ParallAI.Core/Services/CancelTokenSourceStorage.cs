namespace ParallAI.Core.Services;

public class CancelTokenSourceStorage
{
    private readonly Dictionary<Guid, CancellationTokenSource> cancelTokenSources = new();

    public CancellationTokenSource GetSource(Guid token)
    {
        return cancelTokenSources.TryGetValue(token, out var source)
            ? source
            : throw new KeyNotFoundException("The token was not found in the source storage.");
    }

    public void DeleteSource(Guid token)
    {
        if (!cancelTokenSources.Remove(token))
            throw new KeyNotFoundException("The token was not found in the source storage.");
    }

    public void AddSource(Guid token, CancellationTokenSource source)
    {
        cancelTokenSources.Add(token, source);
    }
}