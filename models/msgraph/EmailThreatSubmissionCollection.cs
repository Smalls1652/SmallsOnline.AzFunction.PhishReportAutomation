namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

/// <summary>
/// A collection of <See cref="EmailThreatSubmission">EmailThreatSubmission</See> objects.
/// </summary>
public class EmailThreatSubmissionCollection : IGraphCollection<EmailThreatSubmission>
{
    public EmailThreatSubmissionCollection()
    {}

    /// <summary>
    /// <See cref="EmailThreatSubmission">EmailThreatSubmission</See> objects returned by the Graph API.
    /// </summary>
    [JsonPropertyName("value")]
    public List<EmailThreatSubmission>? Value { get; set; }
}