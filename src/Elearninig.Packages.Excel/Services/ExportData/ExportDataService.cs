using Humanizer;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using File = Elearninig.Packages.Excel.Models.File;

namespace Elearninig.Packages.Excel.Services.ExportData;

public class ExcelService : IExcelService
{


    public File Export<T>(IEnumerable<T> data, string worksheetName, bool applyFormatting = false, List<string>? displayColumns = null)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        displayColumns = displayColumns ?? new List<string>();

        return new File
        {
            Content = applyFormatting ? ToFormattedExcel(data, worksheetName, displayColumns) : ToExcel(data),
            Name = "ExportedData",
            Extension = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };
    }


    // Excel formatting is pretty good but take a long time but still acceptable
    private static byte[] ToFormattedExcel<T>(IEnumerable<T> objects, string worksheetName, List<string>? displayColumns = null)
    {
        var currentRow = 1;

        var type = typeof(T);
        var propertyInfo = type.GetProperties().ToList();
        var columnsCount = propertyInfo.Count;

        if (displayColumns is not null && displayColumns.Any())
        {
            propertyInfo = propertyInfo.Where(x => displayColumns.ConvertAll(d => d.Trim().ToLower()).Contains(x.Name.Trim().ToLower())).ToList();
            columnsCount = propertyInfo.Count;
        }

        ExcelPackage pck = new();
        var ws = pck.Workbook.Worksheets.Add(worksheetName);

        for (var i = 0; i < columnsCount; i++)
        {
            ws.Cells[currentRow, i + 1].Value = propertyInfo[i].Name.Humanize(LetterCasing.Title);
        }

        currentRow++;
        foreach (object item in objects)
        {
            for (var i = 0; i < propertyInfo.Count; i++)
            {
                var propertyType = item?.GetType().GetProperty(propertyInfo[i].Name)?.PropertyType;
                var column = propertyInfo[i].Name.Trim().ToLower();

                if (propertyType == typeof(DateTimeOffset))
                {
                    ws.Cells[currentRow, i + 1].Style.Numberformat.Format = "m/d/yyyy";
                    var dt = (DateTimeOffset)item.GetType().GetProperty(propertyInfo[i].Name).GetValue(item, null);
                    ws.Cells[currentRow, i + 1].Value = dt.ToUniversalTime().ToString("dd MMMM yyyy");
                }
                else if (propertyType == typeof(DateTimeOffset?))
                {
                    var dt = (DateTimeOffset?)item.GetType().GetProperty(propertyInfo[i].Name).GetValue(item, null);
                    if (!dt.HasValue) continue;
                    ws.Cells[currentRow, i + 1].Style.Numberformat.Format = "m/d/yyyy";
                    ws.Cells[currentRow, i + 1].Value = dt.Value.ToUniversalTime().ToString("dd MMMM yyyy");
                }
                else
                {
                    ws.Cells[currentRow, i + 1].Value = item.GetType().GetProperty(propertyInfo[i].Name).GetValue(item, null);
                }

            }

            currentRow++;
        }

        ExcelAddressBase eab = new(1, 1, currentRow - 1, columnsCount);
        ws.Tables.Add(eab, "MyTable");
        ws.Tables["MyTable"].TableStyle = TableStyles.Medium15;

        try
        {
            // The 'libgdiplus' library cannot be loaded on an Apple M1 ARM-based machine with Mac OS
            ws.Cells.AutoFitColumns();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        MemoryStream ms = new();
        pck.SaveAs(ms);
        return ms.ToArray();
    }

    // Take no time but excel formatting not good
    private static byte[] ToExcel<T>(IEnumerable<T> objects)
    {
        using var pck = new ExcelPackage();
        pck.Workbook.Worksheets.Add("Sheet1").Cells[1, 1].LoadFromCollection(objects, true);
        return pck.GetAsByteArray();
    }
}