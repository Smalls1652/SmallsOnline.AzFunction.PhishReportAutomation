namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

public class UserMessageCollection : IGraphCollection<UserMessage>
{
    public UserMessageCollection()
    {}

    [JsonPropertyName("value")]
    public List<UserMessage>? Value { get; set; }
}