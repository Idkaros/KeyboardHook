using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gul
{
    class Program2
    {
        static void Main(string[] args)
        {

            // Load the Service account credentials and define the scope of its access.
            
            var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(PathToServiceAccountKeyFile)
                            .CreateScoped(Google.Apis.Drive.v3.DriveService.ScopeConstants.Drive);

            var service = new Google.Apis.Drive.v3.DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        }

        private const string PathToServiceAccountKeyFile = @"C:\Youtube\dev\ServiceAccountCred.json";
        private const string ServiceAccountEmail = "driveuploadtest@testapikey-305109.iam.gserviceaccount.com";
        private const string UploadFileName = "Test hello.txt";
        private const string DirectoryId = "10krlloIS2i_2u_ewkdv3_1NqcpmWSL1w";
    }
}
