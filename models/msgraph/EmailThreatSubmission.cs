namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

/// <summary>
/// Represents a submission of a suspected phishing email to Microsoft Defender for Office 365.
/// </summary>
public class EmailThreatSubmission : IThreatSubmission, IEmailThreatSubmission
{
    public EmailThreatSubmission()
    {}

    /// <summary>
    /// The OData type of the item in MS Graph.
    /// </summary>
    [JsonPropertyName("@odata.type")]
    public string? OdataType { get; set; }

    /// <summary>
    /// The ID of the submitted email message.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The category of the threat submission.
    /// </summary>
    [JsonPropertyName("category")]
    public string? Category { get; set; }

    /// <summary>
    /// The content type of the threat submission.
    /// </summary>
    [JsonPropertyName("contentType")]
    public string? ContentType { get; set; }

    /// <summary>
    /// The source of the threat submission.
    /// </summary>
    [JsonPropertyName("source")]
    public string? Source { get; set; }

    /// <summary>
    /// The status of the threat submission.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// The internal ID of the email threat submission.
    /// </summary>
    [JsonPropertyName("internetMessageId")]
    public string? InternetMessageId { get; set; }

    /// <summary>
    /// The date and time when the email was received by the user.
    /// </summary>
    [JsonPropertyName("receivedDateTime")]
    public DateTimeOffset? ReceivedDateTime { get; set; }

    /// <summary>
    /// The original category of the threat submission.
    /// </summary>
    [JsonPropertyName("originalCategory")]
    public string? OriginalCategory { get; set; }

    /// <summary>
    /// The subject of the email.
    /// </summary>
    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    /// <summary>
    /// The recipient of the email.
    /// </summary>
    [JsonPropertyName("recipientEmailAddress")]
    public string? RecipientEmailAddress { get; set; }

    /// <summary>
    /// The sender of the email.
    /// </summary>
    [JsonPropertyName("sender")]
    public string? Sender { get; set; }

    /// <summary>
    /// The url of the message to be submitted.
    /// </summary>
    [JsonPropertyName("messageUrl")]
    public string? MessageUrl { get; set; }
}