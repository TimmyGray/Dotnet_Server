using BuyingLibrary.models.classes;
using Microsoft.Extensions.Logging;

namespace BuyingLibrary.Contexts;

public sealed class ImageService
{
    private readonly GridFSBucket _imageStore;
    private readonly ILogger<ImageService> _logger;

    public ImageService(MongoContext context, ILogger<ImageService> logger)
    {
        _imageStore = context.ImageStore;
        _logger = logger;
    }

    public async Task<List<BuyImage>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var files = await _imageStore.Find(FilterDefinition<GridFSFileInfo>.Empty)
                                     .ToListAsync(cancellationToken);
        _logger.LogDebug("Found {Count} image file(s) in GridFS.", files.Count);

        var images = new List<BuyImage>(files.Count);
        foreach (var file in files)
        {
            var image = new BuyImage
            {
                Id = file.Id.ToString(),
                Name = file.Filename,
                Data = await _imageStore.DownloadAsBytesAsync(file.Id, cancellationToken: cancellationToken)
            };
            _logger.LogDebug("Downloaded image {Id}, size {Size} bytes.", image.Id, image.Data?.Length ?? 0);
            images.Add(image);
        }

        return images;
    }

    public async Task GetOneAsync(string id, Stream destination, CancellationToken cancellationToken = default)
    {
        var objectId = new ObjectId(id);
        _logger.LogDebug("Streaming image {Id} to destination stream.", id);
        await _imageStore.DownloadToStreamAsync(objectId, destination, cancellationToken: cancellationToken);
    }
}
