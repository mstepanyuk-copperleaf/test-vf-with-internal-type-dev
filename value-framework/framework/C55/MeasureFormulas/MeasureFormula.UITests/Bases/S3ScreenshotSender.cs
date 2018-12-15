using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;


public class S3ScreenshotSender
{
    protected const string BucketName = "valueframework-test-screenshots";
    protected static readonly RegionEndpoint BucketRegion = RegionEndpoint.USWest2;
    protected static readonly string buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER") == null ?
        "dev" : Environment.GetEnvironmentVariable("BUILD_NUMBER");
    protected static IAmazonS3 Client;
    

    public S3ScreenshotSender()
    {
        Client = new AmazonS3Client(BucketRegion);
    }

    public async Task SaveScreenshot(string screenshotFileLocation, string filename, string testName)
    {
        var bucketPath = BucketName + @"/" + buildNumber;
        var screenshotRequest = new PutObjectRequest
        {
            BucketName = bucketPath,
            Key = filename,
            FilePath = screenshotFileLocation,
            ContentType = "image/png",
            CannedACL = S3CannedACL.PublicRead
        };

        await Client.PutObjectAsync(screenshotRequest);
        Console.WriteLine($"Screenshot for {testName}: http://s3-us-west-2.amazonaws.com/{bucketPath}/{filename}");
    }
    
}
