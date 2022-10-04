namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

public interface IThreatSubmission
{
    string? Id { get; set; }
    string? Category { get; set; }
    string? ContentType { get; set; }
    string? Source { get; set; }
    string? Status { get; set; }
}