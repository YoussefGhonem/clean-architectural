using File = Elearninig.Packages.Excel.Models.File;

namespace Elearninig.Packages.Excel.Services.ExportData;

public interface IExcelService
{
    File Export<T>(IEnumerable<T> data, string worksheetName, bool applyFormatting = false, List<string> displayColumns = null);
}