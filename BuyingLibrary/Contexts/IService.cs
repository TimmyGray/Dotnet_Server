namespace BuyingLibrary.Contexts;

public interface IService<T>
{
    Task<List<T>> GetAsync(CancellationToken cancellationToken = default);

    Task<T?> GetAsync(string id, CancellationToken cancellationToken = default);

    Task<T> PostAsync(T obj, CancellationToken cancellationToken = default);

    Task<T?> PutAsync(T obj, CancellationToken cancellationToken = default);

    Task<T?> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
