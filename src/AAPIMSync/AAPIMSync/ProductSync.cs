using PReardon.AAPIMSync.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PReardon.AAPIMSync
{
    public static class ProductSync
    {
        public static Product Sync(Product product, Product authorative, bool syncGroups)
        {
            //ToDo: Check this
            product.Id = authorative.Id;
            product.Name = authorative.Name;
            product.RefDescription = authorative.RefDescription;
            product.Terms = authorative.Terms;
            product.SubscriptionRequired = authorative.SubscriptionRequired;
            product.ApprovalRequired = authorative.ApprovalRequired;
            product.SubscriptionsLimit = authorative.SubscriptionsLimit;
            product.State = authorative.State;
            if (syncGroups) product.RefsGroups = authorative.RefsGroups;
            product.RefsApis = authorative.RefsApis;

            return product;
        }

        public static bool IsInSync(Product product, Product authorative, bool syncGroups)
        {
            if (product.Id != authorative.Id) return false;
            if (product.Name != authorative.Name) return false;
            if (product.RefDescription != authorative.RefDescription) return false;
            if (product.Terms != authorative.Terms) return false;
            if (product.SubscriptionRequired != authorative.SubscriptionRequired) return false;
            if (product.ApprovalRequired != authorative.ApprovalRequired) return false;
            if (product.SubscriptionsLimit != authorative.SubscriptionsLimit) return false;
            if (product.State != authorative.State) return false;
            if (syncGroups && !ApimUtils.ListsAreEqual(product.RefsGroups, authorative.RefsGroups)) return false;
            if (!ApimUtils.ListsAreEqual(product.RefsApis, authorative.RefsApis)) return false;

            return true;
        }

        public static async Task CompareAndSyncProducts(string folder, Dictionary<string, Product> products, Dictionary<string, Product> authoritiveProducts)
        {
            var newProducts = authoritiveProducts.Keys.Except(products.Keys);
            foreach (var product in newProducts)
            {
                var p = authoritiveProducts[product];
                p.RefsGroups = new List<string>(); // Clear groups
                //Add New Product to Existing
                var folderPath = $"{folder}\\api-management\\products\\{product}";
                Directory.CreateDirectory(folderPath);

                var json = JsonSerializer.Serialize(p);
                var filePath = $"{folderPath}\\{ApimUtils.ConfigurationFileName}";
                System.Console.WriteLine($"Writing {filePath}");
                await File.WriteAllTextAsync(filePath, json);

                if (!string.IsNullOrEmpty(p.RefDescription))
                {
                    System.Console.WriteLine("Writing Ref File");
                    await File.WriteAllTextAsync(Path.Combine(folder, p.RefDescription), p.Description);
                }
            }
            var oldProducts = products.Keys.Except(authoritiveProducts.Keys);
            foreach (var product in oldProducts)
            {
                //Remove Products
                var p = products[product];
                if (!string.IsNullOrEmpty(p.RefDescription))
                {
                    var filePath = Path.Combine(folder, p.RefDescription);
                    System.Console.WriteLine($"Deleting {filePath}");
                    File.Delete(filePath);
                }
                var folderPath = Path.Combine($"{folder}\\api-management\\products\\", product);
                File.Delete(Path.Combine(folderPath, ApimUtils.ConfigurationFileName));

                Directory.Delete(folderPath);

                products.Remove(product);
            }
            foreach (var product in products)
            {
                if (!ProductSync.IsInSync(product.Value, authoritiveProducts[product.Key], false))
                {
                    System.Console.WriteLine($"OUT OF SYNC: {product.Value.Id}");
                    //Ensure it needs to be Sync'd
                    var updatedPropduct = ProductSync.Sync(product.Value, authoritiveProducts[product.Key], false);

                    //Now Save
                    var json = JsonSerializer.Serialize(updatedPropduct, new JsonSerializerOptions { WriteIndented = true });
                    var filePath = $"{folder}\\api-management\\products\\{product.Key}\\{ApimUtils.ConfigurationFileName}"; //Path.Combine(folder1, @"\api-management\products", product.Key, "configuration.json");
                    await File.WriteAllTextAsync(filePath, json);
                }
                else
                {
                    System.Console.WriteLine($"Product {product.Value.Id} Is in sync");
                }

                //Check Ref
                if (product.Value.Description != authoritiveProducts[product.Key].Description)
                {
                    System.Console.WriteLine($"Definition Out of date");
                    var filePath = Path.Combine(folder, product.Value.RefDescription);
                    await File.WriteAllTextAsync(filePath, authoritiveProducts[product.Key].Description);
                }

            }
        }

        public static async Task<Dictionary<string, Product>> GetProducts(string folder)
        {
            var configs = new Dictionary<string, Product>();
            foreach (var dir in Directory.GetDirectories($"{folder}\\api-management\\products"))
            {
                var json = await File.ReadAllTextAsync($"{dir}\\{ApimUtils.ConfigurationFileName}");
                var tagName = new DirectoryInfo(dir).Name;
                var config = JsonSerializer.Deserialize<Product>(json);

                //Pop Deps
                if (!string.IsNullOrEmpty(config.RefDescription))
                {
                    config.Description = await File.ReadAllTextAsync(Path.Combine(folder, config.RefDescription));
                }

                configs.Add(tagName, config);
            }
            return configs;
        }
    }
}
