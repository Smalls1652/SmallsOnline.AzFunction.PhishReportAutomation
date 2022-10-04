namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.Core;

public interface IPhishSubmission
{
    string RecipientEmail { get; set; }
    string InternetMessageId { get; set; }
}