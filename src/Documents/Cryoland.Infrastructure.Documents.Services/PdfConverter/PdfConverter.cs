using Microsoft.Extensions.Options;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Cryoland.Infrastructure.Documents")]
namespace Cryoland.Infrastructure.Documents.Services.PdfConverter
{
    internal class PdfConverter : IPdfConverter
    {
        private readonly IOptions<PdfConverterService> _options;
        private readonly IHttpClientFactory _httpClientFactory;

        public PdfConverter(IOptions<PdfConverterService> options, IHttpClientFactory httpClientFactory)
        {
            _options = options;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Stream> ProcessFileAsync(Stream stream, SupportedTypes type, CancellationToken cancellationToken = default)
        {
            stream.Seek(0, SeekOrigin.Begin);

            HttpClient client = _httpClientFactory.CreateClient();
            using var memoryStream = new MemoryStream();

            await stream.CopyToAsync(memoryStream, cancellationToken);

            using var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(memoryStream.ToArray()), "files", $"{Guid.NewGuid()}.{type.ToFileExtension()}" }
            };

            var response = await client.PostAsync(new Uri(_options.Value.Url), content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var output = await response.Content.ReadAsStreamAsync(cancellationToken);

            return output;
        }
    }
}
