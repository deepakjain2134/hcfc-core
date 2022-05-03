//using Amazon;
//using Amazon.KeyManagementService;
//using Amazon.KeyManagementService.Model;
//using Amazon.Runtime;
//using System;
//using System.IO;
//using System.Text;
//using System.Threading.Tasks;

//namespace HCF.Utility
//{
//    public class KmsEncryptClient : IKmsEncryptClient
//    {
//        public KmsEncryptClient()
//        {

//        }
//        static string key = "arn:aws:kms:us-east-1:901723081794:key/900ebc49-ee7f-4abd-bc90-a34a7a62b27f";

//        private  readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1; 
       
//        public  string Decrypt(string encryptedValue)
//        {
//             var credentials = new BasicAWSCredentials(AppSettings.AWSAccessKey, AppSettings.AWSSecretKey);

//             AmazonKeyManagementServiceClient client = new AmazonKeyManagementServiceClient(credentials, bucketRegion);

//            var ciphertestStream = new MemoryStream(Convert.FromBase64String(encryptedValue)) { Position = 0 };

//            var decryptRequest = new DecryptRequest { CiphertextBlob = ciphertestStream };

//            var response = client.Decrypt(decryptRequest);

//            var buffer = new byte[response.Plaintext.Length];

//            var bytesRead = response.Plaintext.Read(buffer, 0, (int)response.Plaintext.Length);

//            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
//        }

//        public  string Encrypt(string value)
//        {
//            var credentials = new BasicAWSCredentials(AppSettings.AWSAccessKey, AppSettings.AWSSecretKey);
//            AmazonKeyManagementServiceClient client = new AmazonKeyManagementServiceClient(credentials, bucketRegion);

//            var plaintextData = new MemoryStream(Encoding.UTF8.GetBytes(value))
//            {
//                Position = 0
//            };

//            var encryptRequest = new EncryptRequest
//            {
//                KeyId = key,
//                Plaintext = plaintextData
//            };

//            var response = client.Encrypt(encryptRequest);
//            var buffer = new byte[response.CiphertextBlob.Length];
//            response.CiphertextBlob.Read(buffer, 0, (int)response.CiphertextBlob.Length);
//            return Convert.ToBase64String(buffer);
//        }

//        public  async Task<string> DecryptAsync(string encryptedValue)
//        {
//            var credentials = new BasicAWSCredentials(AppSettings.AWSAccessKey, AppSettings.AWSSecretKey);
//            AmazonKeyManagementServiceClient client = new AmazonKeyManagementServiceClient(credentials, bucketRegion);

//            var ciphertestStream = new MemoryStream(Convert.FromBase64String(encryptedValue)) { Position = 0 };

//            var decryptRequest = new DecryptRequest { CiphertextBlob = ciphertestStream };

//            var response = await client.DecryptAsync(decryptRequest);

//            var buffer = new byte[response.Plaintext.Length];

//            var bytesRead = response.Plaintext.Read(buffer, 0, (int)response.Plaintext.Length);

//            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
//        }

//        public  async Task<string> EncryptAsync(string value)
//        {
//            var credentials = new BasicAWSCredentials(AppSettings.AWSAccessKey, AppSettings.AWSSecretKey);
//            AmazonKeyManagementServiceClient client = new AmazonKeyManagementServiceClient(credentials, bucketRegion);

//            var plaintextData = new MemoryStream(Encoding.UTF8.GetBytes(value))
//            {
//                Position = 0
//            };

//            var encryptRequest = new EncryptRequest
//            {
//                KeyId = key,
//                Plaintext = plaintextData
//            };

//            var response = await client.EncryptAsync(encryptRequest);

//            var buffer = new byte[response.CiphertextBlob.Length];

//            response.CiphertextBlob.Read(buffer, 0, (int)response.CiphertextBlob.Length);

//            return Convert.ToBase64String(buffer);
//        }
//    }
//}
