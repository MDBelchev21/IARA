namespace IARA.DomainModel;

public class BaseFilter<T> where T : class
{
    public string? FreeTextSearch { get; set; }
    public T? Filters { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

