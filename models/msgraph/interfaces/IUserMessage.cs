namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

public interface IUserMessage
{
    string Id { get; set; }
    string InternetMessageId { get; set; }
    string Subject { get; set; }
    Recipient Sender { get; set; }
    List<Recipient> ToRecipients { get; set; }
}