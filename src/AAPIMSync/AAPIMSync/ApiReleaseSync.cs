using PReardon.AAPIMSync.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PReardon.AAPIMSync
{
    public static class ApiReleaseSync
    {
        private const string _folderPart = "api-management\\apiReleases";
        public static ApiRelease Sync(ApiRelease release, ApiRelease authorative)
        {
            //ToDo: Check this
            release.Id = authorative.Id;
            release.ApiId = authorative.ApiId;
            release.CreatedDateTime = authorative.CreatedDateTime;
            release.UpdatedDateTime = authorative.UpdatedDateTime;
            release.Notes = authorative.Notes;

            return release;
        }

        public static bool IsInSync(ApiRelease release, ApiRelease authorative)
        {
            if (release.Id != authorative.Id) return false;
            if (release.ApiId != authorative.ApiId) return false;
            if (release.CreatedDateTime != authorative.CreatedDateTime) return false;
            if (release.UpdatedDateTime != authorative.UpdatedDateTime) return false;
            if (release.Notes != authorative.Notes) return false;

            return true;
        }

        public static async Task<Dictionary<string, ApiRelease>> GetApiReleases(string folder)
        {
            var releases = new Dictionary<string, ApiRelease>();
            foreach (var dir in Directory.GetDirectories($"{folder}\\{_folderPart}"))
            {
                var json = await File.ReadAllTextAsync($"{dir}\\{ApimUtils.ConfigurationFileName}");
                var tagName = new DirectoryInfo(dir).Name;
                var release = JsonSerializer.Deserialize<ApiRelease>(json);

                releases.Add(tagName, release);
            }
            return releases;
        }

        public static async Task CompareAndSync(string folder, Dictionary<string, ApiRelease> entities, Dictionary<string, ApiRelease> authoritive)
        {
            var newEntities = authoritive.Keys.Except(entities.Keys);
            foreach (var release in newEntities)
            {
                var t = authoritive[release];

                //Add New Tag to Existing
                var folderPath = $"{folder}\\{_folderPart}\\{release}";
                Directory.CreateDirectory(folderPath);

                var json = JsonSerializer.Serialize(t, new JsonSerializerOptions { WriteIndented = true });
                var filePath = $"{folderPath}\\{ApimUtils.ConfigurationFileName}";
                System.Console.WriteLine($"Writing {filePath}");
                await File.WriteAllTextAsync(filePath, json);
            }
            var oldEntities = entities.Keys.Except(authoritive.Keys);
            foreach (var release in oldEntities)
            {
                //Remove Products
                var folderPath = Path.Combine($"{folder}\\{_folderPart}\\", release);
                File.Delete(Path.Combine(folderPath, ApimUtils.ConfigurationFileName));

                Directory.Delete(folderPath);

                entities.Remove(release);
            }
            foreach (var entity in entities)
            {
                if (!IsInSync(entity.Value, authoritive[entity.Key]))
                {
                    System.Console.WriteLine($"OUT OF SYNC: {entity.Value.Id}");
                    //Ensure it needs to be Sync'd
                    var updatedTag = Sync(entity.Value, authoritive[entity.Key]);

                    //Now Save
                    var json = JsonSerializer.Serialize(updatedTag, new JsonSerializerOptions { WriteIndented = true });
                    var filePath = $"{folder}\\{_folderPart}\\{entity.Key}\\{ApimUtils.ConfigurationFileName}"; //Path.Combine(folder1, @"\api-management\products", product.Key, ApimUtils.ConfigurationFileName);
                    await File.WriteAllTextAsync(filePath, json);
                }
                else
                {
                    System.Console.WriteLine($"{entity.Value.Id} Is in sync");
                }
            }
        }
    }
}
