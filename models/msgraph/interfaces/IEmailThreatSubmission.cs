namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

public interface IEmailThreatSubmission
{
    string? OdataType { get; set; }
    string? InternetMessageId { get; set; }
    DateTimeOffset? ReceivedDateTime { get; set; }
    string? OriginalCategory { get; set; }
    string? Subject { get; set; }
    string? RecipientEmailAddress { get; set; }
    string? Sender { get; set; }
    string? MessageUrl { get; set; }
}