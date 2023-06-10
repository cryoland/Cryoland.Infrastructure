using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cryoland.Infrastructure.Documents
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDocuments(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();

            services.Configure<PdfConverterService>(configuration.GetSection("PdfConverterService"));
            services.AddScoped<IPdfConverter, PdfConverter>();

            services.Configure<ThumbnailServiceConfig>(configuration.GetSection("ThumbnailService"));
            services.AddScoped<IThumbnailService, ThumbnailService>();

            services.AddDocumentsImplementations();

            return services;
        }
    }
}
