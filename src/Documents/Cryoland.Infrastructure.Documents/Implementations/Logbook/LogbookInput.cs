namespace Cryoland.Infrastructure.Documents.Implementations.InfoMessagesLogbook
{
    public class LogbookInput
    {
        public IEnumerable<LogbookDto> Items { get; set; }
    }

    public class LogbookDto
    {
        public string OrgName { get; set; }

        public string Number { get; set; }

        public DateTime ConfirmDate { get; set; }
    }
}
