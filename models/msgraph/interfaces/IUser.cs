namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

public interface IUser
{
    string Id { get; set; }
    string UserPrincipalName { get; set; }
}