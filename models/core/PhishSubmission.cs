namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.Core;

/// <summary>
/// Represents a submission of a suspected phishing email.
/// </summary>
public class PhishSubmission : IPhishSubmission
{
    public PhishSubmission()
    {}

    /// <summary>
    /// The email address of the recipient.
    /// </summary>
    [JsonPropertyName("recipientEmail")]
    public string RecipientEmail { get; set; } = null!;

    /// <summary>
    /// The internet message ID of the email.
    /// </summary>
    [JsonPropertyName("internetMessageId")]
    public string InternetMessageId { get; set; } = null!;
}