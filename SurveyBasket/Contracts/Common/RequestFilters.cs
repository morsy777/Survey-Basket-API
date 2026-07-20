namespace SurveyBasket.Contracts.Common;

public record RequestFilters
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
