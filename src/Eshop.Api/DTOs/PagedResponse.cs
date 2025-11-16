namespace Eshop.Api.DTOs;

/// <summary>
/// A generic response wrapper for paginated results.
/// </summary>
/// <typeparam name="T">The type of the items in the list.</typeparam>
public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public PagedResponse(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        PageNumber = Math.Max(1, pageNumber);
        PageSize = Math.Max(1, pageSize);
        TotalCount = Math.Max(0, totalCount);
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
