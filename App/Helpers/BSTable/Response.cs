namespace Helpers.BSTable;

public class Response<T>
{
    public int Total { get; set; }
    public int TotalNotFiltered { get; set; }
    public List<T>? Rows { get; set; }
}