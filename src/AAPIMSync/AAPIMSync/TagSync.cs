using PReardon.AAPIMSync.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PReardon.AAPIMSync
{
    public static class TagSync
    {
        private const string _tagsFolder = "api-management\\tags";
        public static Tag Sync(Tag tag, Tag authorative)
        {
            //ToDo: Check this
            tag.Id = authorative.Id;
            tag.Name = authorative.Name;
            tag.ApiIds = authorative.ApiIds;
            tag.ProductIds = authorative.ProductIds;
            tag.Descriptions = authorative.Descriptions;
            tag.OperationIds = authorative.OperationIds;

            return tag;
        }

        public static bool IsInSync(Tag tag, Tag authorative)
        {
            if (tag.Id != authorative.Id) return false;
            if (tag.Name != authorative.Name) return false;
            if (!ApimUtils.ListsAreEqual(tag.ApiIds, authorative.ApiIds)) return false;
            if (!ApimUtils.ListsAreEqual(tag.ProductIds, authorative.ProductIds)) return false;
            if (!ApimUtils.ListsAreEqual(tag.Descriptions, authorative.Descriptions)) return false;
            if (!ApimUtils.ListsAreEqual(tag.OperationIds, authorative.OperationIds)) return false;

            return true;
        }

        public static async Task<Dictionary<string, Tag>> GetTags(string folder)
        {
            var tags = new Dictionary<string, Tag>();
            foreach (var dir in Directory.GetDirectories($"{folder}\\{_tagsFolder}"))
            {
                var json = await File.ReadAllTextAsync($"{dir}\\{ApimUtils.ConfigurationFileName}");
                var tagName = new DirectoryInfo(dir).Name;
                var tag = JsonSerializer.Deserialize<Tag>(json);

                tags.Add(tagName, tag);
            }
            return tags;
        }

        public static async Task CompareAndSync(string folder, Dictionary<string, Tag> tags, Dictionary<string, Tag> authoritiveTags)
        {
            var newTags = authoritiveTags.Keys.Except(tags.Keys);
            foreach (var tag in newTags)
            {
                var t = authoritiveTags[tag];
                
                //Add New Tag to Existing
                var folderPath = $"{folder}\\{_tagsFolder}\\{tag}";
                Directory.CreateDirectory(folderPath);

                var json = JsonSerializer.Serialize(t);
                var filePath = $"{folderPath}\\{ApimUtils.ConfigurationFileName}";
                System.Console.WriteLine($"Writing {filePath}");
                await File.WriteAllTextAsync(filePath, json);
            }
            var oldTags = tags.Keys.Except(authoritiveTags.Keys);
            foreach (var tag in oldTags)
            {
                //Remove Products
                var t = tags[tag];
                var folderPath = Path.Combine($"{folder}\\{_tagsFolder}\\", tag);
                File.Delete(Path.Combine(folderPath, ApimUtils.ConfigurationFileName));

                Directory.Delete(folderPath);

                tags.Remove(tag);
            }
            foreach (var tag in tags)
            {
                if (!IsInSync(tag.Value, authoritiveTags[tag.Key]))
                {
                    System.Console.WriteLine($"OUT OF SYNC: {tag.Value.Id}");
                    //Ensure it needs to be Sync'd
                    var updatedTag = Sync(tag.Value, authoritiveTags[tag.Key]);

                    //Now Save
                    var json = JsonSerializer.Serialize(updatedTag, new JsonSerializerOptions { WriteIndented = true });
                    var filePath = $"{folder}\\{_tagsFolder}\\{tag.Key}\\{ApimUtils.ConfigurationFileName}"; //Path.Combine(folder1, @"\api-management\products", product.Key, ApimUtils.ConfigurationFileName);
                    await File.WriteAllTextAsync(filePath, json);
                }
                else
                {
                    System.Console.WriteLine($"Tag {tag.Value.Id} Is in sync");
                }
            }
        }
    }
}
