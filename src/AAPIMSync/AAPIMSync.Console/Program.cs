using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PReardon.AAPIMSync.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please Enter at least 2 Folders");
            }
            var folderMaster = args[0];
            var updateFolders = new List<string>();
            for(var i = 1; i< args.Length; i++)
            {
                updateFolders.Add(args[i]);
            }

            //folderMaster = @"";
            //updateFolders.Add(@"");
            //updateFolders.Add(@"");

            Console.WriteLine($"Using {folderMaster} to Update {string.Join(',', updateFolders)}");
            Console.WriteLine("Press Y to Continue");
            var accept = Console.ReadKey();

            if (accept.Key == ConsoleKey.Y)
            {
                var mainApi = await ApiSync.Get(folderMaster);
                var mainProduct = await ProductSync.GetProducts(folderMaster);
                var mainRelease = await ApiReleaseSync.GetApiReleases(folderMaster);
                var mainVersion = await ApiVersionSetSync.Get(folderMaster);
                var mainTag = await TagSync.GetTags(folderMaster);

                foreach (var f in updateFolders)
                {
                    Console.WriteLine($"Upating {f}");
                    Console.WriteLine("");

                    Console.WriteLine("Syncing Api");
                    var apiF1 = await ApiSync.Get(f);
                    await ApiSync.CompareAndSync(f, apiF1, folderMaster, mainApi);

                    Console.WriteLine("Syncing Products");
                    var productF1 = await ProductSync.GetProducts(f);
                    //await ApimUtils.CheckRefsAsync(mainProduct.Values.First(), folderMaster);
                    await ProductSync.CompareAndSyncProducts(f, productF1, mainProduct);

                    Console.WriteLine("Syncing Api Releases");
                    var releaseF1 = await ApiReleaseSync.GetApiReleases(f);
                    await ApiReleaseSync.CompareAndSync(f, releaseF1, mainRelease);

                    Console.WriteLine("Syncing Api Versions");
                    var versionF1 = await ApiVersionSetSync.Get(f);
                    await ApiVersionSetSync.CompareAndSync(f, versionF1, mainVersion);

                    Console.WriteLine("Syncing Tags");
                    var TaqF1 = await TagSync.GetTags(f);
                    await TagSync.CompareAndSync(f, TaqF1, mainTag);
                }
            }
            else
            {
                Console.WriteLine("Sync Aborted");
            }
        }

    }
}
