namespace Cryoland.Infrastructure.Documents.Services.ThumbnailService
{
    public interface IThumbnailService
    {
        Task<Stream> CreateThumbnailAsync(
            Stream stream,
            SupportedTypes type,
            int width = 1024,
            int height = 1024,
            CancellationToken cancellationToken = default);
    }
}
