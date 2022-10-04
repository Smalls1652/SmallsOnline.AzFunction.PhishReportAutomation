namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

public class EmailThreatSubmissionCollection : IGraphCollection<EmailThreatSubmission>
{
    public EmailThreatSubmissionCollection()
    {}

    [JsonPropertyName("value")]
    public List<EmailThreatSubmission>? Value { get; set; }
}