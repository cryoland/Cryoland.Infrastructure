using Cryoland.Infrastructure.Documents.Implementations.DaysOffReport;

namespace Cryoland.Infrastructure.Documents.Implementations.DaysOffReportPdf
{
    public class DaysOffReportPdfDocument : Document<DaysOffReportInput, DaysOffReportPdfProcessor>
    {
        public DaysOffReportPdfDocument(IPdfConverter pdfConverter) : base(pdfConverter)
        {
        }
    }
}
