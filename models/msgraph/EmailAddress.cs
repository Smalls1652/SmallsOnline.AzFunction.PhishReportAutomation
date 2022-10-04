namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

/// <summary>
/// Email address information of a recipient.
/// </summary>
public class EmailAddress : IEmailAddress
{
    public EmailAddress()
    {}

    /// <summary>
    /// The email address.
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; } = null!;

    /// <summary>
    /// The display name of the email address.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}