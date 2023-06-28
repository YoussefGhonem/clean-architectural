using Microsoft.EntityFrameworkCore;

namespace Elearninig.Base.Application.common.Pagination;

public static class PaginationExtension
{
    // tuple provides a lightweight way to retrieve multiple values from a method cal
    public static (List<T> list, int total) Paginate<T>(this IQueryable<T> query, int? pageSize, int? pageNumber)
    {
        const int maxPageSize = 20;
        var paginatedList = new List<T>();

        if (!pageSize.HasValue && !pageNumber.HasValue)
        {
            paginatedList = query.Take(maxPageSize).ToList();
            return (paginatedList.ToList(), query.Count());
        }

        var pageIndex = pageNumber!.Value - 1;
        paginatedList = query.Skip(pageIndex * pageSize!.Value).Take(pageSize.Value).ToList();
        return (paginatedList, query.Count());
    }

    public static async Task<(List<T> list, int total)> PaginateAsync<T>(
        this IQueryable<T> query,
        int? pageSize,
        int? pageNumber,
        CancellationToken cancellationToken = default)
    {
        const int maxPageSize = 20;
        List<T> paginatedList;
        int count;

        if (!pageSize.HasValue && !pageNumber.HasValue)
        {
            paginatedList = await query.Take(maxPageSize).ToListAsync(cancellationToken);
            count = await query.CountAsync(cancellationToken);
            return (paginatedList, count);
        }

        var pageIndex = pageNumber!.Value - 1;
        count = await query.CountAsync(cancellationToken);
        paginatedList = await query.Skip(pageIndex * pageSize!.Value).Take(pageSize.Value).ToListAsync(cancellationToken);
        return (paginatedList, count);
    }
}