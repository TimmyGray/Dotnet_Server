using BuyingLibrary.Contexts;

namespace Aspnet_server.Tests.Fakes;

public sealed class InMemoryService<T> : IService<T>
{
    public List<T> Items { get; } = [];

    public Func<string, T?>? GetById { get; init; }
    public Func<T, T?>? PutBehavior { get; init; }
    public Func<string, T?>? DeleteBehavior { get; init; }

    public Task<List<T>> GetAsync(CancellationToken cancellationToken = default) => Task.FromResult(Items);

    public Task<T?> GetAsync(string id, CancellationToken cancellationToken = default) => Task.FromResult(GetById is null ? default : GetById(id));

    public Task<T> PostAsync(T obj, CancellationToken cancellationToken = default)
    {
        Items.Add(obj);
        return Task.FromResult(obj);
    }

    public Task<T?> PutAsync(T obj, CancellationToken cancellationToken = default) => Task.FromResult(PutBehavior is null ? obj : PutBehavior(obj));

    public Task<T?> DeleteAsync(string id, CancellationToken cancellationToken = default) => Task.FromResult(DeleteBehavior is null ? default : DeleteBehavior(id));
}
