using PReardon.AAPIMSync.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PReardon.AAPIMSync
{
    public static class ApiSync
    {
        private const string _folderPart = "api-management\\apis";
        public static Api Sync(Api entity, Api authorative)
        {
            //ToDo: Check this
            entity.Id = authorative.Id;
            entity.Name = authorative.Name;
            entity.ApiRevision = authorative.ApiRevision;
            entity.SubscriptionRequired = authorative.SubscriptionRequired; 
            entity.ServiceUrl = authorative.ServiceUrl;
            entity.Path = authorative.Path;
            entity.IsCurrent = authorative.IsCurrent;
            entity.ApiRevisionDescription = authorative.ApiRevisionDescription;
            entity.ApiVersionSetId = authorative.ApiVersionSetId;

            //Refs
            entity.RefDescription = authorative.RefDescription;
            entity.RefPolicy = authorative.RefPolicy;

            //Children
            entity.AuthenticationSettings = authorative.AuthenticationSettings;
            entity.SubscriptionKeyParameterNames = authorative.SubscriptionKeyParameterNames;

            //Lists
            entity.Protocols = authorative.Protocols;
            entity.Operations = authorative.Operations;
            entity.ApiSchemas = authorative.ApiSchemas;

            return entity;
        }

        public static async Task<Dictionary<string, Api>> Get(string folder)
        {
            var releases = new Dictionary<string, Api>();
            foreach (var dir in Directory.GetDirectories($"{folder}\\{_folderPart}"))
            {
                var json = await File.ReadAllTextAsync($"{dir}\\{ApimUtils.ConfigurationFileName}");
                var tagName = new DirectoryInfo(dir).Name;
                var release = JsonSerializer.Deserialize<Api>(json);

                releases.Add(tagName, release);
            }
            return releases;
        }

        public static async Task CompareAndSync(string folder, Dictionary<string, Api> entities, string authoritiveFolder, Dictionary<string, Api> authoritive)
        {
            var newEntities = authoritive.Keys.Except(entities.Keys);
            foreach (var entity in newEntities)
            {
                var t = authoritive[entity];

                //Add New Tag to Existing
                var folderPath = $"{folder}\\{_folderPart}\\{entity}";
                Directory.CreateDirectory(folderPath);

                var json = JsonSerializer.Serialize(t, new JsonSerializerOptions { WriteIndented = true });
                var filePath = $"{folderPath}\\{ApimUtils.ConfigurationFileName}";
                System.Console.WriteLine($"Writing {filePath}");
                await File.WriteAllTextAsync(filePath, json);

                //Check Refs
                if (!string.IsNullOrWhiteSpace(t.RefDescription))
                {
                    //Check Description
                    await CreateRef(t.RefDescription, folder, authoritiveFolder);
                }

                //Operations
                foreach (var o in t.Operations)
                {
                    if (!string.IsNullOrWhiteSpace(o.RefDescription))
                    {
                        // Check Operation Description
                        await CreateRef(o.RefDescription, folder, authoritiveFolder);
                    }

                    if (!string.IsNullOrWhiteSpace(o.RefPolicy))
                    {
                        // Check Operation Policy
                        await CreateRef(o.RefPolicy, folder, authoritiveFolder);
                    }

                    if (!string.IsNullOrWhiteSpace(o.Request.RefDescription))
                    {
                        // Check Operation Request Desicription
                        await CreateRef(o.Request.RefDescription, folder, authoritiveFolder);
                    }

                    foreach (var r in o.Request.Representations)
                    {

                        if (!string.IsNullOrEmpty(r.RefSample))
                        {
                            // Check Operation Request Representation Samples
                            await CreateRef(r.RefSample, folder, authoritiveFolder);
                        }
                    }

                    foreach (var response in o.Responses)
                    {
                        if (!string.IsNullOrEmpty(response.RefDescription))
                        {
                            //Check Operation Response Description
                            await CreateRef(response.RefDescription, folder, authoritiveFolder);

                        }

                        foreach (var r in response.Representations)
                        {
                            if (!string.IsNullOrEmpty(r.RefSample))
                            {
                                // Check Operation Response Representation Samples
                                await CreateRef(r.RefSample, folder, authoritiveFolder);
                            }
                        }
                    }

                }

                //Check Api Scheamas
                foreach (var s in t.ApiSchemas)
                {
                    if (!string.IsNullOrEmpty(s.RefDocumentValue))
                    {
                        //Check Api Schema Document Value
                        await CreateRef(s.RefDocumentValue, folder, authoritiveFolder);
                    }
                }

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
                var a = authoritive[entity.Key];
                var e = entity.Value;
                if (!entity.Value.Equals(a))
                {
                    System.Console.WriteLine($"OUT OF SYNC: {entity.Value.Id}");
                    //Ensure it needs to be Sync'd

                    //Todo Check for Removed Entities
                    var updatedApi = Sync(e, a);

                    //Now Save
                    var json = JsonSerializer.Serialize(updatedApi, new JsonSerializerOptions { WriteIndented = true });
                    var filePath = $"{folder}\\{_folderPart}\\{entity.Key}\\{ApimUtils.ConfigurationFileName}"; //Path.Combine(folder1, @"\api-management\products", product.Key, ApimUtils.ConfigurationFileName);
                    await File.WriteAllTextAsync(filePath, json);
                    e = updatedApi;
                }
                else
                {
                    System.Console.WriteLine($"{entity.Value.Id} Is in sync");
                }

                //Check Refs
                if(!string.IsNullOrWhiteSpace(e.RefDescription))
                {
                    //Check Description
                    await SyncRef(e.RefDescription, folder, authoritiveFolder);
                }

                //Operations
                foreach(var o in e.Operations)
                {
                    if(!string.IsNullOrWhiteSpace(o.RefDescription))
                    {
                        // Check Operation Description
                        await SyncRef(o.RefDescription, folder, authoritiveFolder);
                    }

                    if (!string.IsNullOrWhiteSpace(o.RefPolicy))
                    {
                        // Check Operation Policy
                        await SyncRef(o.RefPolicy, folder, authoritiveFolder);
                    }

                    if (!string.IsNullOrWhiteSpace(o.Request.RefDescription))
                    {
                        // Check Operation Request Desicription
                        await SyncRef(o.Request.RefDescription, folder, authoritiveFolder);
                    }


                    foreach (var r in o.Request.Representations)
                    {
                        
                        if(!string.IsNullOrEmpty(r.RefSample))
                        {
                            // Check Operation Request Representation Samples
                            await SyncRef(r.RefSample, folder, authoritiveFolder);
                        }
                    }

                    foreach (var response in o.Responses)
                    {
                        if(!string.IsNullOrEmpty(response.RefDescription))
                        {
                            //Check Operation Response Description
                            await SyncRef(response.RefDescription, folder, authoritiveFolder);

                        }

                        foreach (var r in response.Representations)
                        {
                            if (!string.IsNullOrEmpty(r.RefSample))
                            {
                                // Check Operation Response Representation Samples
                                await SyncRef(r.RefSample, folder, authoritiveFolder);
                            }
                        }
                    }

                }

                //Check Api Scheamas
                foreach(var s in e.ApiSchemas)
                {
                    if(!string.IsNullOrEmpty(s.RefDocumentValue))
                    {
                        //Check Api Schema Document Value
                        await SyncRef(s.RefDocumentValue, folder, authoritiveFolder);
                    }
                }
            }
        }

        public async static Task SyncRef(string reference, string baseFolderToUpdate, string baseAuthoritiveFolder)
        {
            var authPath = Path.Combine(baseAuthoritiveFolder, reference);
            var updPath = Path.Combine(baseFolderToUpdate, reference);

            //Should I really case about Updating??
            var auth = await File.ReadAllTextAsync(authPath);
            var upd = await File.ReadAllTextAsync(updPath);

            if(!auth.Equals(upd))
            {
                Console.WriteLine($"  - Updating {reference}");
                await File.WriteAllTextAsync(updPath, auth);
            }
        }

        public async static Task CreateRef(string reference, string baseFolderToUpdate, string baseAuthoritiveFolder)
        {
            var authPath = Path.Combine(baseAuthoritiveFolder, reference);
            var newPath = Path.Combine(baseFolderToUpdate, reference);

            var auth = await File.ReadAllTextAsync(authPath);

            var dir = Path.GetDirectoryName(newPath);
            if(!Directory.Exists(dir))
            {
                Console.WriteLine($"-- Creating Directory {dir}");
                Directory.CreateDirectory(dir);
            }

            Console.WriteLine($"  - Creating {reference}");
            await File.WriteAllTextAsync(newPath, auth);
        }
    }
}
