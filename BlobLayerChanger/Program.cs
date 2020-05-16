using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlobLayerChanger
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var account = CloudStorageAccount.Parse(args[0]);
            var client = account.CreateCloudBlobClient();

            var container = client.GetContainerReference(args[1]);
            var blobs = container.ListBlobs(args[2], true).Cast<CloudBlockBlob>();
            foreach (var blob in blobs)
            {
                Console.Write($"{blob.Name}");
                var tier = blob.Properties.StandardBlobTier;
                if (tier == StandardBlobTier.Hot)
                {
                    Console.Write($"...");
                    await blob.SetStandardBlobTierAsync(StandardBlobTier.Archive);
                    Console.Write($"OK");
                }

                Console.WriteLine();
            }
        }
    }
}
