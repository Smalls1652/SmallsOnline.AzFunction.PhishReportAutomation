namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

/// <summary>
/// A collection of <See cref="UserMessage">UserMessage</See> objects.
/// </summary>
public class UserMessageCollection : IGraphCollection<UserMessage>
{
    public UserMessageCollection()
    {}

    /// <summary>
    /// <See cref="UserMessage">UserMessage</See> objects returned by the Graph API.
    /// </summary>
    [JsonPropertyName("value")]
    public List<UserMessage>? Value { get; set; }
}