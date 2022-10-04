namespace SmallsOnline.AzFunctions.PhishReportAutomation.Models.MsGraph;

/// <summary>
/// Data about a user in Azure AD.
/// </summary>
public class User : IUser
{
    public User()
    {}

    /// <summary>
    /// The user's object ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// The user's user principal name (UPN).
    /// </summary>
    [JsonPropertyName("userPrincipalName")]
    public string UserPrincipalName { get; set; } = null!;
}