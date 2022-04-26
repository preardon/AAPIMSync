using PReardon.AAPIMSync.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PReardon.AAPIMSync
{
    public static class ApiVersionSetSync
    {
        private const string _folderPart = "api-management\\apiVersionSets";
        public static ApiVersionSet Sync(ApiVersionSet entity, ApiVersionSet authorative)
        {
            //ToDo: Check this
            entity.Id = authorative.Id;
            entity.Name = authorative.Name;
            entity.Description = authorative.Description;
            entity.VersioningScheme = authorative.VersioningScheme;
            entity.VersionQueryName = authorative.VersionQueryName;
            entity.VersionHeaderName = authorative.VersionHeaderName;

            return entity;
        }

        public static bool IsInSync(ApiVersionSet entity, ApiVersionSet authorative)
        {
            if (entity.Id != authorative.Id) return false;
            if (entity.Name != authorative.Name) return false;
            if (entity.Description != authorative.Description) return false;
            if (entity.VersioningScheme != authorative.VersioningScheme) return false;
            if (entity.VersionQueryName != authorative.VersionQueryName) return false;
            if (entity.VersionHeaderName != authorative.VersionHeaderName) return false;

            return true;
        }

        public static async Task<Dictionary<string, ApiVersionSet>> Get(string folder)
        {
            var releases = new Dictionary<string, ApiVersionSet>();
            foreach (var dir in Directory.GetDirectories($"{folder}\\{_folderPart}"))
            {
                var json = await File.ReadAllTextAsync($"{dir}\\{ApimUtils.ConfigurationFileName}");
                var tagName = new DirectoryInfo(dir).Name;
                var release = JsonSerializer.Deserialize<ApiVersionSet>(json, JsonSerialisation.Options);

                releases.Add(tagName, release);
            }
            return releases;
        }

        public static async Task CompareAndSync(string folder, Dictionary<string, ApiVersionSet> entities, Dictionary<string, ApiVersionSet> authoritive)
        {
            var newEntities = authoritive.Keys.Except(entities.Keys);
            foreach (var entity in newEntities)
            {
                var t = authoritive[entity];

                //Add New Tag to Existing
                var folderPath = $"{folder}\\{_folderPart}\\{entity}";
                Directory.CreateDirectory(folderPath);

                var json = JsonSerialisation.Serialise(t);
                var filePath = $"{folderPath}\\{ApimUtils.ConfigurationFileName}";
                System.Console.WriteLine($"Writing {filePath}");
                await File.WriteAllTextAsync(filePath, json);
            }
            var oldEntities = entities.Keys.Except(authoritive.Keys);
            foreach (var entity in oldEntities)
            {
                //Remove Products
                var folderPath = Path.Combine($"{folder}\\{_folderPart}\\", entity);
                File.Delete(Path.Combine(folderPath, ApimUtils.ConfigurationFileName));

                Directory.Delete(folderPath, true);

                entities.Remove(entity);
            }
            foreach (var entity in entities)
            {
                if (!IsInSync(entity.Value, authoritive[entity.Key]))
                {
                    System.Console.WriteLine($"OUT OF SYNC: {entity.Value.Id}");
                    //Ensure it needs to be Sync'd
                    var updatedTag = Sync(entity.Value, authoritive[entity.Key]);

                    //Now Save
                    var json = JsonSerialisation.Serialise(updatedTag);
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
