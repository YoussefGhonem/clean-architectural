using Microsoft.AspNetCore.Http;

namespace Elearninig.Packages.Excel.Services.ImportData;

public interface IImportDataService
{
    List<T> ImportFromExcel<T>(IFormFile formFile) where T : class;
}