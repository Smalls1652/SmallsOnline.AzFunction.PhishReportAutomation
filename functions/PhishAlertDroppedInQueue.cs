using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SmallsOnline.AzFunctions.PhishReportAutomation.Services;

namespace SmallsOnline.AzFunctions.PhishReportAutomation.Functions;

/// <summary>
/// Houses the function that is triggered when a reported phish email is dropped in the queue.
/// </summary>
public class PhishAlertDroppedInQueue
{
    private readonly ILogger<PhishAlertDroppedInQueue> _logger;
    private readonly IGraphClientService _graphClientService;

    public PhishAlertDroppedInQueue(ILoggerFactory loggerFactory, IGraphClientService graphClientService)
    {
        _logger = loggerFactory.CreateLogger<PhishAlertDroppedInQueue>();
        _graphClientService = graphClientService;
    }

    /// <summary>
    /// Gets the reported phishing email that was reported by a user and submits it to Microsoft for analysis.
    /// </summary>
    /// <remarks>
    /// This is triggered when a message is dropped in the "phish-reports" Azure Storage Queue.
    /// <param name="submissionItem">The reported phishing email.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [Function("PhishAlertDroppedInQueue")]
    public async Task Run(
        [QueueTrigger("phish-reports", Connection = "StorageAccountConnection")] PhishSubmission submissionItem
    )
    {
        _logger.LogInformation("Received phish report submission for '{InternetMessageId}' from '{Recipient}'.", submissionItem.InternetMessageId, submissionItem.RecipientEmail);

        // Check to see if the GraphClient is connected.
        if (_graphClientService.GraphClient.IsConnected == false)
        {
            // If not, then connect.
            _logger.LogInformation("GraphClient is not connected. Attempting to connect.");
            await _graphClientService.GraphClient.ConnectClientAsync();
        }

        // Get the user who reported the phish from the Graph API.
        string? userJson = await _graphClientService.GraphClient.SendApiCallAsync(
            endpoint: $"users/{submissionItem.RecipientEmail}",
            apiPostBody: null,
            httpMethod: HttpMethod.Get
        );

        // Throw an an error if the returned JSON is null.
        if (userJson is null)
        {
            throw new Exception($"Unable to retrieve user information for '{submissionItem.RecipientEmail}'.");
        }

        // Deserialize the JSON into a User object.
        User user = JsonSerializer.Deserialize<User>(userJson)!;

        _logger.LogInformation("Retrieved user ID, '{UserId}', for '{UserPrincipalName}'.", user.Id, user.UserPrincipalName);

        // Find the message(s) that were reported as phish in the user's mailbox.
        string? messagesJson = await _graphClientService.GraphClient.SendApiCallAsync(
            endpoint: $"users/{user.Id}/messages?$filter=internetMessageId eq '{submissionItem.InternetMessageId}'",
            apiPostBody: null,
            httpMethod: HttpMethod.Get
        );

        // Throw an an error if the returned JSON is null.
        if (messagesJson is null)
        {
            throw new Exception($"Unable to retrieve messages for '{submissionItem.RecipientEmail}'.");
        }

        // Deserialize the JSON into a MessageCollection object.
        UserMessageCollection messages = JsonSerializer.Deserialize<UserMessageCollection>(messagesJson)!;

        // If no messages were found, then throw an error.
        if (messages.Value is null || messages.Value.Count == 0)
        {
            throw new Exception($"No messages were found.");
        }

        // Get the current submissions to Microsoft that were reported by administrators.
        string? currentSubmissionsJson = await _graphClientService.GraphClient.SendApiCallAsync(
            endpoint: "security/threatSubmission/emailThreats?$filter=source eq 'administrator'",
            apiPostBody: null,
            httpMethod: HttpMethod.Get
        );

        // Throw an an error if the returned JSON is null.
        if (currentSubmissionsJson is null)
        {
            throw new Exception($"Unable to retrieve current email submissions.");
        }

        // Deserialize the JSON into a ThreatSubmissionCollection object.
        EmailThreatSubmissionCollection currentSubmissions = JsonSerializer.Deserialize<EmailThreatSubmissionCollection>(currentSubmissionsJson)!;

        // If no submissions were found, then throw an error.
        if (currentSubmissions.Value is null || currentSubmissions.Value.Count == 0)
        {
            throw new Exception($"No current email submissions were found.");
        }

        // Loop through the messages that were found in the user's mailbox.
        foreach (UserMessage messageItem in messages.Value)
        {
            _logger.LogInformation("Processing message ID, '{MessageId}' with subject '{Subject}', for '{UserPrincipalName}'.", messageItem.Id, messageItem.Subject, user.UserPrincipalName);

            // Check to see if the message has already been submitted to Microsoft by matching
            // the message's subject and sender address.
            EmailThreatSubmission? existingSubmission = currentSubmissions.Value.Find(
                (submission) => submission.Subject == messageItem.Subject && submission.Sender == messageItem.Sender.EmailAddress.Address
            );

            // Evaluate if an existing submission was found.
            if (existingSubmission is not null)
            {
                // If an existing submission was found, then log the information and continue/skip to the next message.
                _logger.LogInformation("Existing submission found for '{InternetMessageId}'. Skipping...", submissionItem.InternetMessageId);
                continue;
            }
            else
            {
                // If an existing submission was not found, then submit the message to Microsoft for analysis.
                _logger.LogInformation("Submitting phish report to Microsoft for '{InternetMessageId}'.", submissionItem.InternetMessageId);

                // Create a new EmailThreatSubmission object to use for the HTTP POST request.
                EmailThreatSubmission submissionPostItem = new()
                {
                    OdataType = "#microsoft.graph.security.emailUrlThreatSubmission",
                    Category = "phishing",
                    RecipientEmailAddress = user.UserPrincipalName,
                    MessageUrl = $"https://graph.microsoft.com/beta/users/{user.Id}/messages/{messageItem.Id}"
                };

                // Serialize the EmailThreatSubmission object into a JSON string.
                // Ignore null properties during serialization.
                string submissionPostJson = JsonSerializer.Serialize(
                    value: submissionPostItem,
                    options: new()
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    }
                );

                try
                {
                    // Submit the message to Microsoft for analysis through the Graph API.
                    await _graphClientService.GraphClient.SendApiCallAsync(
                        endpoint: "security/threatSubmission/emailThreats",
                        apiPostBody: submissionPostJson,
                        httpMethod: HttpMethod.Post
                    );
                }
                catch
                {
                    _logger.LogError("Unable to submit phish report to Microsoft for '{InternetMessageId}'.", submissionItem.InternetMessageId);
                }
            }
        }
    }
}
