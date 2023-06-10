using Cryoland.Infrastructure.Documents.Abstractions;
using Cryoland.Infrastructure.Documents.Implementations.DaysOffReport;
using Cryoland.Infrastructure.Documents.Implementations.DaysOffReportPdf;
using Cryoland.Infrastructure.Documents.Implementations.InfoMessagesLogbook;
using Cryoland.Infrastructure.Documents.Services.ThumbnailService;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cryoland.Infrastructure.Documents.Web.Api
{
    public static partial class ApiExtensions
    {
        public static void AddDocumentsApi(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/daysoff/document/create",
                [Produces(typeof(FileStreamResult))]
                [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
                ([FromServices] DaysOffReportDocument document, [FromBody] DaysOffReportInput dto)
                    => Results.File(document.Create(dto), document.Mime, $"Письмо коллегам {DateTime.Now:dd.MM.yyyy__HH_mm}.docx"))
            .WithTags("DaysOff")
            .WithOpenApi();

            endpoints.MapPost("/daysoff/document/create/pdf",
                [Produces(typeof(FileStreamResult))]
                [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
                ([FromServices] DaysOffReportPdfDocument document, [FromBody] DaysOffReportInput dto)
                    => Results.File(document.Create(dto), document.Mime, $"Письмо коллегам {DateTime.Now:dd.MM.yyyy__HH_mm}.pdf"))
            .WithTags("DaysOff")
            .WithOpenApi();


            endpoints.MapPost("/daysoff/document/thumbnail/width:{width}/height:{height}",
                [Produces(typeof(FileStreamResult))]
                [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
                async ([FromServices] IThumbnailService thumbnailService,
                    [FromServices] DaysOffReportDocument document, 
                    [FromBody] DaysOffReportInput dto,
                    CancellationToken cancellationToken,
                    [FromRoute] int width,  // [FromRoute] int width = 1024 <- In preview or .NET 8+ versions
                    [FromRoute] int height) // [FromRoute] int height = 1024 <- In preview or .NET 8+ versions
                =>
                {
                    var documentStream = document.Create(dto);
                    var thumbnailStream = await thumbnailService.CreateThumbnailAsync(documentStream,
                                                                                      SupportedTypes.DOCX,
                                                                                      width,
                                                                                      height,
                                                                                      cancellationToken: cancellationToken);

                    return Results.File(thumbnailStream, Mime.JPEG, $"Письмо коллегам {DateTime.Now:dd.MM.yyyy__HH_mm}.jpeg");
                })
            .WithTags("DaysOff")
            .WithOpenApi();

            endpoints.MapGet("/logbook/document/create",
                [Produces(typeof(FileStreamResult))]
                [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
                    ([FromServices] LogbookDocument document) 
                =>
                {
                    LogbookInput dto = new()
                    {
                        Items = new List<LogbookDto>() {
                            new LogbookDto
                            {
                                 ConfirmDate = DateTime.Now.AddDays(-20),
                                 Number = "71201",
                                 OrgName = "Org Name 1"
                            },
                            new LogbookDto
                            {
                                 ConfirmDate = DateTime.Now.AddDays(-15),
                                 Number = "14351",
                                 OrgName = "Org Name 2"
                            },
                            new LogbookDto
                            {
                                 ConfirmDate = DateTime.Now.AddDays(-10),
                                 Number = "25625",
                                 OrgName = "Org Name 3"
                            },
                            new LogbookDto
                            {
                                 ConfirmDate = DateTime.Now.AddDays(-5),
                                 Number = "14534",
                                 OrgName = "Org Name 4"
                            },
                        }
                    };

                    return Results.File(document.Create(dto), document.Mime, $"Журнал {DateTime.Now:dd.MM.yyyy__HH_mm}.xlsx");
                })
            .WithTags("Logbook")
            .WithOpenApi();
        }
    }
}
