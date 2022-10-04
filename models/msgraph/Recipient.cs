namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

/// <summary>
/// Represents a recipient of a message.
/// </summary>
public class Recipient : IRecipient
{
    public Recipient()
    {}

    /// <summary>
    /// Email address information of the recipient.
    /// </summary>
    [JsonPropertyName("emailAddress")]
    public EmailAddress EmailAddress { get; set; } = null!;
}