using Cryoland.Infrastructure.Documents;
using Cryoland.Infrastructure.Documents.Web.Api;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// DI
builder.Services.AddDocuments(builder.Configuration);

// Controllers
builder.Services.AddRouting(options =>
{
    options.AppendTrailingSlash = true;
    options.LowercaseUrls = true;
})
    .AddControllers()
    .AddNewtonsoftJson();

// Response compression
builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
                .AddResponseCompression(options =>
                {
                    options.Providers.Add<GzipCompressionProvider>();
                    options.EnableForHttps = true;
                });

// OpenAPI
builder.Configuration.AddJsonFile($"nswag.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"nswag.{builder.Environment.EnvironmentName}.json", optional: true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = builder.Configuration["documentGenerator:aspNetCoreToOpenApi:infoTitle"];
    settings.DocumentName = builder.Configuration["documentGenerator:aspNetCoreToOpenApi:documentName"];
    settings.Version = builder.Configuration["documentGenerator:aspNetCoreToOpenApi:infoVersion"];
    settings.GenerateExamples = true;
    settings.UseControllerSummaryAsTagDescription = true;
});

var app = builder.Build();

// API
app.AddDocumentsApi();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseRouting()
   .UseResponseCompression()
   .UseEndpoints(endpoints =>
   {
       endpoints.MapControllers();
   });

app.Run();
