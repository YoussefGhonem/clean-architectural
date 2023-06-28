using Elearninig.Packages.Excel.Services.ExportData;
using Elearninig.Packages.Excel.Services.ImportData;
using Microsoft.Extensions.DependencyInjection;

namespace Elearninig.Packages.Excel;

public static class ConfigureService
{
    public static IServiceCollection AddExcelOperations(this IServiceCollection services)
    {
        services.AddSingleton<IExcelService, ExcelService>();
        services.AddSingleton<IImportDataService, ImportDataService>();
        return services;
    }
}