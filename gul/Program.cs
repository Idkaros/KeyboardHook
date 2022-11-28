using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gul
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        private static Google.Apis.Drive.v3.DriveService GetService()
        {
            //Google.Apis.Json
            //Google.Apis.Drive.v3.Data
            
            var tokenResponse = new Google.Apis.Auth.OAuth2.Responses.TokenResponse
            {
                AccessToken = "...",
                RefreshToken = "...",
            }


            var applicationName = "..."; // Use the name of the project in Google Cloud
            var username = "..."; // Use your email


            var apiCodeFlow = new Google.Apis.Auth.OAuth2.Flows.GoogleAuthorizationCodeFlow(new Google.Apis.Auth.OAuth2.Flows.GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = "...",
                    ClientSecret = "..."
                },
                Scopes = new[] { Scope.Drive },
                DataStore = new FileDataStore(applicationName)
            })


            var credential = new UserCredential(apiCodeFlow, username, tokenResponse)


            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            })
            return service;
        }
    }
}
