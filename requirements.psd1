# This file enables modules to be automatically managed by the Functions service.
# See https://aka.ms/functionsmanageddependency for additional information.
#
@{
    # For latest supported version, go to 'https://www.powershellgallery.com/packages/Az'.
    # To use the Az module in your function app, please uncomment the line below.
    # 'Az' = '8.*'
    "Microsoft.Graph.Authentication" = "1.*";
    "Microsoft.Graph.Mail"           = "1.*";
    "Microsoft.Graph.Security"       = "1.*";
    "Microsoft.Graph.Users"          = "1.*";
}