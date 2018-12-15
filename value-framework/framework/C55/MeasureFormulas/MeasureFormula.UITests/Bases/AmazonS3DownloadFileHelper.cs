using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace MeasureFormula.UITests.Bases
{
    public class AmazonS3DownloadFileHelper
    {
        protected static IAmazonS3 Client;
        protected static readonly RegionEndpoint BucketRegion = RegionEndpoint.USWest2;
        private readonly string OutputFolderPath = Path.GetTempPath();


        public AmazonS3DownloadFileHelper()
        {
            Client = new AmazonS3Client(BucketRegion);
        }

        public void DownloadValueFrameworkFile(string fileKey)
        {
            var filePath = OutputFolderPath + "vflibrary-base-framework.xlsx";

            /* for experiment only
            var request = new TransferUtilityDownloadRequest
            {
                BucketName = "converted-value-frameworks",
                Key = fileKey,
                FilePath = filePath
            };
            var transferUtility = new TransferUtility(Client);
            transferUtility.Download(request);
            */
            var request = new GetObjectRequest
            {
                BucketName = "converted-value-frameworks",
                Key = fileKey,
            };
            using (GetObjectResponse response = Client.GetObject(request))
            {
                response.WriteResponseStreamToFile(filePath, false);
            }
        }
    }
}
