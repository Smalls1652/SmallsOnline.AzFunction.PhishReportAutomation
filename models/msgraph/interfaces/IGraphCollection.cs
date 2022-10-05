namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

public interface IGraphCollection<T>
{
    List<T>? Value { get; set; }
}