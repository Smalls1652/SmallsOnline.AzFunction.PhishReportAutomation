# Azure Functions profile.ps1
#
# This profile.ps1 will get executed every "cold start" of your Function App.
# "cold start" occurs when:
#
# * A Function App starts up for the very first time
# * A Function App starts up after being de-allocated due to inactivity
#
# You can define helper functions, run commands, or specify environment variables
# NOTE: any variables defined that are not environment variables will get reset after the first execution

# Create a MSGraph client with the provided app details and switch the profile to the 'beta' endpoints.
Connect-MgGraph -ClientId $env:AppId -TenantId $env:TenantId -CertificateThumbprint $env:CertThumbprint
Select-MgProfile -Name "beta"

# Define the 'PhishSubmission' class for use with the functions.
class PhishSubmission {
    [string]$RecipientEmail
    [string]$InternetMessageId
}