using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PReardon.AAPIMSync
{
    public static class PolicySync
    {
        private const string _folderPart = "api-management\\policies";

        public async static Task CompareAndSyncAsync(string folder, string authoritiveFolder)
        {
            var authoritivePath = Path.Combine(authoritiveFolder, _folderPart);
            var updatePath = Path.Combine(folder, _folderPart);

            var authoritivePolicies = new List<string>();
            foreach(var s in Directory.GetFiles(authoritivePath, "", SearchOption.AllDirectories))
            {
                authoritivePolicies.Add(s.Replace(authoritivePath, "").TrimStart('\\'));
            }
            var updatePolicies = new List<string>();
            foreach(var s in Directory.GetFiles(updatePath, "", SearchOption.AllDirectories))
            {
                updatePolicies.Add(s.Replace(updatePath, "").TrimStart('\\'));
            }

            var newEntities = authoritivePolicies.Except(updatePolicies);
            foreach (var entity in newEntities)
            {
                //Add New Policy to Existing
                var path = Path.Combine(updatePath, entity);
                var authPath = Path.Combine(authoritivePath, entity);
                var dir = Path.GetDirectoryName(path);
                if(!Directory.Exists(dir))
                {
                    Console.WriteLine($"-- Creating Directory {dir}");
                    Directory.CreateDirectory(dir);
                }

                var auth = await File.ReadAllTextAsync(authPath);

                Console.WriteLine($"  - Creating {entity}");
                await File.WriteAllTextAsync(path, auth);
            }
            var oldEntities = updatePolicies.Except(authoritivePolicies);
            foreach (var entity in oldEntities)
            {
                //Remove Products
                var path = Path.Combine(updatePath, entity);
                Console.WriteLine($"  - Deleting {entity}");
                updatePolicies.Remove(path);
            }
            foreach (var entity in updatePolicies)
            {
                var authPath = Path.Combine(authoritivePath, entity);
                var newPath = Path.Combine(updatePath, entity);
                var auth = await File.ReadAllTextAsync(authPath);
                var upd = await File.ReadAllTextAsync(newPath);

                if (!auth.Equals(upd))
                {
                    Console.WriteLine($"OUT OF SYNC: {entity}");
                    await File.WriteAllTextAsync(entity, auth);
                }
                else
                {
                    Console.WriteLine($"{entity} Is in sync");
                }
            }
        }
    }
}
