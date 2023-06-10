namespace Cryoland.Infrastructure.Documents.Implementations.DaysOffReport
{
    public class DaysOffReportDocument : Document<DaysOffReportInput, DaysOffReportProcessor>
    {
        public DaysOffReportDocument(IPdfConverter pdfConverter) : base(pdfConverter)
        {
        }
    }
}
