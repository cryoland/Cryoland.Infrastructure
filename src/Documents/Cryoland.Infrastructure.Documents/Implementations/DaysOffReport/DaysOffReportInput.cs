namespace Cryoland.Infrastructure.Documents.Implementations.DaysOffReport
{
    public class DaysOffReportInput
    {
        public string Name { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public string Teammate { get; set; }
    }
}
