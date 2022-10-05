namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

/// <summary>
/// A message in a user's mailbox.
/// </summary>
public class UserMessage : IUserMessage
{
    public UserMessage()
    {}

    /// <summary>
    /// The message's object ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// The message's Internet Message ID.
    /// </summary>
    [JsonPropertyName("internetMessageId")]
    public string InternetMessageId { get; set; } = null!;

    /// <summary>
    /// The subject of the message.
    /// </summary>
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = null!;

    /// <summary>
    /// The sender of the message.
    /// </summary>
    [JsonPropertyName("sender")]
    public Recipient Sender { get; set; } = null!;

    /// <summary>
    /// Recipients of the message.
    /// </summary>
    [JsonPropertyName("toRecipients")]
    public List<Recipient> ToRecipients { get; set; } = null!;

    /// <summary>
    /// Check to see if an email address is in the ToRecipients property.
    /// </summary>
    /// <param name="emailAddress">The email address of a recipient.</param>
    /// <returns>A boolean response depending on if the supplied email address is in ToRecipients.</returns>
    public bool IsRecipient(string emailAddress)
    {
        return ToRecipients.Exists((recipient) => recipient.EmailAddress.Address == emailAddress);
    }
}