[CmdletBinding()]
param($QueueItem, $TriggerMetadata)

# Try to convert the item from the queue to the 'PhishSubmission' class.
try {
    $convertedItem = [PhishSubmission]($QueueItem)
}
catch {
    throw [System.Exception]::new($PSItem.Exception.Message)
}

# Write out the queue message and insertion time to the information log.
Write-Host "A new phish report item was dropped in the queue: $($convertedItem | ConvertTo-Json)"
Write-Host "Queue item insertion time: $($TriggerMetadata.InsertionTime)"

# Get the recipient user's info.
$user = Get-MgUser -UserId $convertedItem.RecipientEmail

# Search the recipient user's messages for the internet message ID of the email.
$foundMessages = Get-MgUserMessage -UserId $user.Id -Filter "internetMessageId eq '$($convertedItem.InternetMessageId)'"

# Get the items that have been currently submitted to Microsoft as phishing.
$currentlySubmittedItems = Get-MgSecurityThreatSubmissionEmailThreat -Filter "source eq 'administrator'"

$submittedItems = [System.Collections.Generic.List[pscustomobject]]::new()

# Loop through each message that was found in the user's mailbox.
foreach ($messageItem in $foundMessages) {
    # Run a check to make sure the message wasn't already submitted. 
    if ($null -eq ($currentlySubmittedItems | Where-Object { ($PSItem.Sender -eq $messageItem.Sender.EmailAddress.Address) -and ($PSItem.RecipientEmailAddress -in $messageItem.ToRecipients.EmailAddress.Address) })) {
        # If no submission was found, then start the submission process for the message.

        # Create the POST body for submitting the message.
        $postBodyObj = @{
            "@odata.type"           = "#microsoft.graph.security.emailUrlThreatSubmission";
            "category"              = "phishing";
            "recipientEmailAddress" = $user.UserPrincipalName;
            "messageUrl"            = "https://graph.microsoft.com/beta/users/$($user.Id)/messages/$($messageItem.Id)";
        }

        Write-Information -InformationAction "Continue" -MessageData "Submitting message, '$($messageItem.Id)', received by '$($user.UserPrincipalName)' as phishing."

        try {
            # Submit the message to Microsoft.
            $submissionResult = Invoke-MgGraphRequest -Method "POST" -Uri "https://graph.microsoft.com/beta/security/threatSubmission/emailThreats" -Body $postBodyObj -ErrorAction "Stop"

            $submittedItems.Add(
                [pscustomobject]@{
                    "SubmissionId"      = $submissionResult.Id;
                    "InternetMessageId" = $convertedItem.InternetMessageId;
                    "MessageUrl"        = "https://graph.microsoft.com/beta/users/$($user.Id)/messages/$($messageItem.Id)";
                }
            )
        }
        catch {
            $errorDetails = $PSItem
            Write-Warning "Failed to submit '$($messageItem.Id)' with message:`n$($errorDetails.Exception.Message)"
        }
    }
    else {
        # If the message was already submitted, then skip it.
        Write-Warning "Skipping '$($messageItem.Id)' as it has already been submitted."
    }
}