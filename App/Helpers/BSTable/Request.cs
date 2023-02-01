namespace Helpers.BSTable;

public class Request
{
    public int Limit { get; set; } = 10;
    public int Offset { get; set; } = 0;
    public string Sort { get; set; } = "";
    public string Order { get; set; } = "asc";
    public string? Search { get; set; } = "";

    public string SearchUpper() => string.IsNullOrWhiteSpace(Search) ? "" : Search.ToUpper();
}