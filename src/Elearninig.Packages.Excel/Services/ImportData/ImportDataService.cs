using Microsoft.AspNetCore.Http;
using Npoi.Mapper;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Elearninig.Packages.Excel.Services.ImportData;

public class ImportDataService : IImportDataService
{
    public List<T> ImportFromExcel<T>(IFormFile formFile) where T : class
    {
        IWorkbook workbook = GetAppropriateWorkbook(formFile);

        ISheet sheet = workbook.GetSheetAt(0);
        var mapper = new Mapper(formFile.OpenReadStream());
        var r = sheet.PhysicalNumberOfRows;
        var rows = mapper.Take<T>(sheet.SheetName).ToList();
        // Select data from excel sheet
        var excelData = rows.Select(x => x.Value).Where(x => !IsEmptyRow(x)).ToList();

        // Map excel data to our DTO
        return excelData;
    }

    private IWorkbook GetAppropriateWorkbook(IFormFile formFile)
    {
        if (formFile.FileName.Trim().ToLower().IndexOf(".xlsx") > 0)
        {
            return new XSSFWorkbook(formFile.OpenReadStream());
        }

        return new HSSFWorkbook(formFile.OpenReadStream());
    }

    private bool IsEmptyRow(object myObject) // check if myObject contain null valuse or not
    {
        return myObject.GetType().GetProperties()
            .Where(pi => pi.PropertyType == typeof(string))
            .Select(pi => (string)pi.GetValue(myObject))
            .All(value => string.IsNullOrEmpty(value));
    }
}