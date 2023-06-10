using NPOI.XWPF.UserModel;
using System.Globalization;

namespace Cryoland.Infrastructure.Documents.Implementations.DaysOffReport
{
    public class DaysOffReportProcessor : ITemplateProcessor<DaysOffReportInput>
    {
        public virtual SupportedTypes Type => SupportedTypes.DOCX;

        public void Process(DaysOffReportInput input, Stream targetStream)
        {
            // set default datetime format in current thread
            CultureInfo culture = (CultureInfo)CultureInfo.GetCultureInfo("ru-RU").Clone();
            culture.DateTimeFormat.LongDatePattern = string.Empty;
            culture.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            culture.DateTimeFormat.LongTimePattern = string.Empty;
            Thread.CurrentThread.CurrentCulture = culture;

            using MemoryStream inputStream = new(Templates.DaysOffReportTemplate, false);

            XWPFDocument document = new(inputStream);

            // taking into account all text chunks in current document
            IEnumerable<XWPFRun> runs = GetAllRuns(document).Where(i => i.GetText(0) is { Length: > 0 } _i && _i.Trim().Any());

            // reflection: properties and its' values
            static IEnumerable<(string, string)> ExtractPropertyValuePairs(object input)
            {
                return input.GetType().GetProperties().Select(i => ($"{i.Name}", $"{i.GetValue(input)}".Trim()));
            }

            foreach (var (placeholder, value) in ExtractPropertyValuePairs(input))
            {
                foreach (XWPFRun run in runs.Where(i => i.GetText(0).Contains(placeholder)))
                {
                    if (run == null) continue;

                    run.SetText(value);
                }
            }

            document.Write(targetStream);
        }

        private static IEnumerable<XWPFRun> GetAllRuns(XWPFDocument document)
            => Enumerable.Concat(
                Enumerable.Concat(document.HeaderList.SelectMany(i => i.Tables),
                                  document.FooterList.SelectMany(i => i.Tables))
                          .Concat(document.Tables)
                          .SelectMany(i => i.Rows)
                          .SelectMany(i => i.GetTableCells())
                          .SelectMany(i => i.Paragraphs)
                          .SelectMany(i => i.Runs),

                document.Paragraphs.SelectMany(i => i.Runs)
            );
    }
}
