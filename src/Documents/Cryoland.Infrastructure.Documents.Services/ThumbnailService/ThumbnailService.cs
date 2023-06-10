using Microsoft.Extensions.Options;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Cryoland.Infrastructure.Documents")]
namespace Cryoland.Infrastructure.Documents.Services.ThumbnailService
{
    internal class ThumbnailService : IThumbnailService
    {
        private readonly IOptions<ThumbnailServiceConfig> _options;
        private readonly IHttpClientFactory _httpClientFactory;

        public ThumbnailService(IOptions<ThumbnailServiceConfig> options, IHttpClientFactory httpClientFactory)
        {
            _options = options;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Stream> CreateThumbnailAsync(
            Stream stream,
            SupportedTypes type,
            int width = 1024,
            int height = 1024,
            CancellationToken cancellationToken = default)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using HttpClient client = _httpClientFactory.CreateClient();

            using var content = new MultipartFormDataContent
            {
                { new StreamContent(stream), "file", $"{Guid.NewGuid()}.{type.ToFileExtension()}" }
            };

            // don't mark with using statement to prevent returning disposed stream
            var response = await client.PostAsync(new Uri($"{_options.Value.Url}/{width}x{height}"), content, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync(cancellationToken);
        }
    }
}
