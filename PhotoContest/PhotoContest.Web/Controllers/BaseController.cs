namespace PhotoContest.Web.Controllers
{
    using System.Web.Mvc;
    using PhotoContest.Data;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;

    public abstract class BaseController : Controller
    {
        protected static CloudBlobContainer imagesContainer;

        protected BaseController(IPhotoContestData data)
        {
            this.Data = data;
            InitStorage();
        }

        protected IPhotoContestData Data { get; private set; }

        private static void InitStorage()
        {
            var credentials = new StorageCredentials(AppKeys.Storage_Account_Name, AppKeys.PrimaryAccessKey);
            var storageAccount = new CloudStorageAccount(credentials, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            imagesContainer = blobClient.GetContainerReference("images");
        }
    }
}