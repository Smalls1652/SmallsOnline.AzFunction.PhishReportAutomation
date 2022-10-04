using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SmallsOnline.AzFunctions.PhishReportAutomation.Services;

namespace SmallsOnline.AzFunctions.PhishReportAutomation.Functions
{
    public class PhishAlertDroppedInQueue
    {
        private readonly ILogger<PhishAlertDroppedInQueue> _logger;
        private readonly IGraphClientService _graphClientService;

        public PhishAlertDroppedInQueue(ILoggerFactory loggerFactory, IGraphClientService graphClientService)
        {
            _logger = loggerFactory.CreateLogger<PhishAlertDroppedInQueue>();
            _graphClientService = graphClientService;
        }

        [Function("PhishAlertDroppedInQueue")]
        public async Task Run(
            [QueueTrigger("phish-reports", Connection = "StorageAccountConnection")] PhishSubmission submissionItem
        )
        {
            _logger.LogInformation("Received phish report submission for '{InternetMessageId}' from '{Recipient}'.", submissionItem.InternetMessageId, submissionItem.RecipientEmail);

            if (_graphClientService.GraphClient.IsConnected == false)
            {
                _logger.LogInformation("GraphClient is not connected. Attempting to connect.");
                await _graphClientService.GraphClient.ConnectClientAsync();
            }

            string? userJson = await _graphClientService.GraphClient.SendApiCallAsync(
                endpoint: $"users/{submissionItem.RecipientEmail}",
                apiPostBody: null,
                httpMethod: HttpMethod.Get
            );

            if (userJson is null)
            {
                throw new Exception($"Unable to retrieve user information for '{submissionItem.RecipientEmail}'.");
            }

            User user = JsonSerializer.Deserialize<User>(userJson)!;

            _logger.LogInformation("Retrieved user ID, '{UserId}', for '{UserPrincipalName}'.", user.Id, user.UserPrincipalName);

            string? messagesJson = await _graphClientService.GraphClient.SendApiCallAsync(
                endpoint: $"users/{user.Id}/messages?$filter=internetMessageId eq '{submissionItem.InternetMessageId}'",
                apiPostBody: null,
                httpMethod: HttpMethod.Get
            );

            if (messagesJson is null)
            {
                throw new Exception($"Unable to retrieve messages for '{submissionItem.RecipientEmail}'.");
            }

            UserMessageCollection messages = JsonSerializer.Deserialize<UserMessageCollection>(messagesJson)!;

            if (messages.Value is null || messages.Value.Count == 0)
            {
                throw new Exception($"No messages were found.");
            }

            string? currentSubmissionsJson = await _graphClientService.GraphClient.SendApiCallAsync(
                endpoint: "security/threatSubmission/emailThreats?$filter=source eq 'administrator'",
                apiPostBody: null,
                httpMethod: HttpMethod.Get
            );

            if (currentSubmissionsJson is null)
            {
                throw new Exception($"Unable to retrieve current email submissions.");
            }

            EmailThreatSubmissionCollection currentSubmissions = JsonSerializer.Deserialize<EmailThreatSubmissionCollection>(currentSubmissionsJson)!;

            if (currentSubmissions.Value is null || currentSubmissions.Value.Count == 0)
            {
                throw new Exception($"No current email submissions were found.");
            }

            foreach (UserMessage messageItem in messages.Value)
            {
                _logger.LogInformation("Processing message ID, '{MessageId}' with subject '{Subject}', for '{UserPrincipalName}'.", messageItem.Id, messageItem.Subject, user.UserPrincipalName);

                Recipient? recipientItem = messageItem.ToRecipients.Find(
                    (recipient) => recipient.EmailAddress.Address == user.UserPrincipalName
                );

                if (recipientItem is null)
                {
                    _logger.LogInformation("Recipient '{Recipient}' was not found in message recipients.", user.UserPrincipalName);
                    continue;
                }

                EmailThreatSubmission? existingSubmission = currentSubmissions.Value.Find(
                    (submission) => submission.Subject == messageItem.Subject && messageItem.IsRecipient(submissionItem.RecipientEmail)
                );

                if (existingSubmission is not null)
                {
                    _logger.LogInformation("Existing submission found for '{InternetMessageId}'. Skipping...", submissionItem.InternetMessageId);
                    continue;
                }
                else
                {
                    _logger.LogInformation("Submitting phish report to Microsoft for '{InternetMessageId}'.", submissionItem.InternetMessageId);

                    EmailThreatSubmission submissionPostItem = new()
                    {
                        Category = "phishing",
                        RecipientEmailAddress = user.UserPrincipalName,
                        MessageUrl = $"https://graph.microsoft.com/beta/users/{user.Id}/messages/{messageItem.Id}"
                    };

                    try
                    {
                        await _graphClientService.GraphClient.SendApiCallAsync(
                            endpoint: "security/threatSubmission/emailThreats",
                            apiPostBody: JsonSerializer.Serialize(submissionPostItem),
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
}
