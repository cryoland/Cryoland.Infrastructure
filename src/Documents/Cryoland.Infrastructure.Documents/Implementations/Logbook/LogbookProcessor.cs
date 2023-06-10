using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Cryoland.Infrastructure.Documents.Implementations.InfoMessagesLogbook
{
    public class LogbookProcessor : ITemplateProcessor<LogbookInput>
    {
        public SupportedTypes Type => SupportedTypes.XLSX;

        public void Process(LogbookInput input, Stream targetStream)
        {
            using var templateStream = new MemoryStream(Templates.LogbookTemplate, false);
            XSSFWorkbook document = new(templateStream);
            var sheet = document.GetSheetAt(0);

            SetReportHeader(sheet, input);
            SetReportGridData(sheet, input);

            document.Write(targetStream, leaveOpen: true);
        }

        private static string MonthFromNumber(int num) => num switch
        {
            1 => "января",
            2 => "февраля",
            3 => "марта",
            4 => "апреля",
            5 => "мая",
            6 => "июня",
            7 => "июля",
            8 => "августа",
            9 => "сентября",
            10 => "октября",
            11 => "ноября",
            12 => "декабря",
            _ => "undefined"
        };

        private void SetReportHeader(ISheet sheet, LogbookInput input)
        {
            var cell = GetNamedCell(sheet, "Dates");
            var cellText = cell.StringCellValue;

            var date = input.Items.MinBy(x => x.ConfirmDate)?.ConfirmDate;

            if (date != null)
            {
                var formatDate = $"{date.Value.Day} {MonthFromNumber(date.Value.Month)}";

                cellText = cellText.Replace("%beginDate%", formatDate);
                cellText = cellText.Replace("%beginYear%", date.Value.Year.ToString());
            }

            cell.SetCellValue(cellText);

            IFont font = sheet.Workbook.CreateFont();
            font.FontName = "Times New Roman";
            font.FontHeightInPoints = 7;

            cell.CellStyle.SetFont(font);
            cell.CellStyle.Alignment = HorizontalAlignment.Right;
        }

        private static void SetReportGridData(ISheet sheet, LogbookInput input)
        {
            var headerCell = GetNamedCell(sheet, "ReportFooter");
            var currentRowIndex = headerCell.RowIndex;

            foreach (var item in input.Items)
            {
                currentRowIndex++;
                sheet.CopyRow(currentRowIndex, currentRowIndex + 1);
                var row = sheet.GetRow(currentRowIndex);
                var currentColumnIndex = headerCell.ColumnIndex;

                row.GetCell(currentColumnIndex++).SetCellValue(item.Number);
                row.GetCell(currentColumnIndex++).SetCellValue("Письмо");
                row.GetCell(currentColumnIndex++).SetCellValue(item.OrgName);
                row.GetCell(currentColumnIndex++).SetCellValue($"{item.ConfirmDate:dd.MM.yyyy}");
                row.GetCell(currentColumnIndex++).SetCellValue("О работе по договору");
                row.GetCell(currentColumnIndex++).SetCellValue(1);
                row.GetCell(currentColumnIndex).SetCellValue(1);
            }

            var templateRow = sheet.GetRow(currentRowIndex + 1);
            sheet.RemoveRow(templateRow);
            int rowIndex = templateRow.RowNum;
            int lastRowNum = sheet.LastRowNum;

            if (rowIndex >= 0 && rowIndex < lastRowNum)
            {
                sheet.ShiftRows(rowIndex + 1, lastRowNum, -1);
            }
        }

        private static ICell GetNamedCell(ISheet sheet, string cellName)
        {
            var range = sheet.Workbook.GetName(cellName);
            if (range == null)
                return null;

            var rangeAdresses = new RangeAddress(range.RefersToFormula);
            var cellRef = new CellReference(rangeAdresses.FromCell);
            var cell = sheet.GetRow(cellRef.Row).GetCell(cellRef.Col);
            return cell;
        }
    }
}
