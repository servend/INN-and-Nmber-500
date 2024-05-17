using System;
using System.IO;
using OfficeOpenXml;

class Program
{
    static void Main()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        string filePath = "C:/Users/User/Desktop/Лиды/ноябрь/База.xlsx";
        int batchSize = 499;

        using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension.Rows;

            for (int i = 1; i <= rowCount; i += batchSize)
            {
                int endIndex = Math.Min(i + batchSize - 1, rowCount);
                CreateNewTable(worksheet, i, endIndex, i / batchSize + 1);
            }
        }
    }

    static void CreateNewTable(ExcelWorksheet sourceWorksheet, int startIndex, int endIndex, int tableIndex)
    {
        using (ExcelPackage package = new ExcelPackage())
        {
            ExcelWorksheet newWorksheet = package.Workbook.Worksheets.Add("Новая таблица");
            newWorksheet.Cells["A1"].Value = "ИНН";
            newWorksheet.Cells["B1"].Value = "Телефон";

            for (int i = startIndex; i <= endIndex; i++)
            {
                string phoneValue = sourceWorksheet.Cells["B" + i].Value?.ToString();
                if (phoneValue != null)
                {
                    newWorksheet.Cells["B" + (i - startIndex + 2)].Value = new string(phoneValue.Where(c => char.IsDigit(c)).ToArray());
                }

                newWorksheet.Cells["A" + (i - startIndex + 2)].Value = sourceWorksheet.Cells["A" + i].Value;
            }

            string newFilePath = $"C:/Output/Новый_файл_{tableIndex}.xlsx";
            package.SaveAs(new FileInfo(newFilePath));
        }
    }
}