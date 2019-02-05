using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage.Queue;

using System.Configuration;



namespace AzureStorage
{
    class Program
    {
        static void Main(string[] args)
        {

            //UploadBlob();
            // Uploadfile();
            PushQueue();
        }
       public static void  UploadBlob()
        {
            string StorageString = ConfigurationSettings.AppSettings.Get("AzureStorageAccount");

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StorageString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("testcontainer");
            cloudBlobContainer.CreateIfNotExists();

            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference("testblob");

            using (var filestream = System.IO.File.OpenRead("FilePath"))
            {
                blob.UploadFromStream(filestream);

            }
           
        }
        public static void Uploadfile()
        {
            string StorageString = ConfigurationSettings.AppSettings.Get("AzureStorageAccount");

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StorageString);
            CloudFileClient cloudfileClient = cloudStorageAccount.CreateCloudFileClient();
            CloudFileShare share = cloudfileClient.GetShareReference("test");
            share.CreateIfNotExists();
            CloudFile sourceFile = share.GetRootDirectoryReference().GetFileReference("test.txt");
            using (var filestream = System.IO.File.OpenRead(@"D:\test.txt"))
            {
                sourceFile.UploadFromStream(filestream);

            }

        }
        public static void PushQueue()
        {
            string StorageString = ConfigurationSettings.AppSettings.Get("AzureStorageAccount");

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StorageString);
            CloudQueueClient cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
            CloudQueue queue = cloudQueueClient.GetQueueReference("test");
            queue.CreateIfNotExists();
            
            using (var filestream = System.IO.File.OpenRead(@"D:\test.txt"))
            {
                System.IO.BinaryReader br = new System.IO.BinaryReader(filestream);
                byte[] buff = null;
                long numBytes = filestream.Length;
                buff = br.ReadBytes((int)numBytes);
                CloudQueueMessage message = new CloudQueueMessage(buff);
                queue.AddMessage(message);

            }

        }

    }
}
