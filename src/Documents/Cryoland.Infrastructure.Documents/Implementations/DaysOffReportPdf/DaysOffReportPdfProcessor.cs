using Cryoland.Infrastructure.Documents.Implementations.DaysOffReport;

namespace Cryoland.Infrastructure.Documents.Implementations.DaysOffReportPdf
{
    public class DaysOffReportPdfProcessor : DaysOffReportProcessor
    {
        public override SupportedTypes Type => SupportedTypes.PDF;
    }
}
