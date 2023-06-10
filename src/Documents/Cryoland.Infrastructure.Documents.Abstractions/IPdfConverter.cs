namespace Cryoland.Infrastructure.Documents.Abstractions
{
    public interface IPdfConverter
    {
        Task<Stream> ProcessFileAsync(Stream stream, SupportedTypes type, CancellationToken cancellationToken = default);
    }
}
