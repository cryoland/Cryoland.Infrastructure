namespace Cryoland.Infrastructure.Documents.Implementations.InfoMessagesLogbook
{
    public class LogbookDocument : Document<LogbookInput, LogbookProcessor>
    {
        public LogbookDocument(IPdfConverter pdfConverter) : base(pdfConverter)
        {
        }
    }
}
