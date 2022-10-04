namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

public interface IEmailAddress
{
    string Address { get; set; }
    string Name { get; set; }
}