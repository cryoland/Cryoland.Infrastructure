using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cryoland.Infrastructure.Documents.Abstractions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDocumentsImplementations(this IServiceCollection services)
        {
            foreach (Type documentType in Assembly.GetCallingAssembly()
                                                  .GetExportedTypes()
                                                  .Where(type => type.Name.EndsWith("Document")
                                                              && type.BaseType.GetInterfaces().Single().Name == typeof(IDocument<>).Name))
            {
                services.AddScoped(documentType);
            }

            return services;
        }
    }
}
