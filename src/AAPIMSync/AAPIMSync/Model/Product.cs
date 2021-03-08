using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class Product : ConfigurationBase
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("$ref-description")]
        public string RefDescription { get; set; }
        [JsonPropertyName("terms")]
        public string Terms { get; set; }
        [JsonPropertyName("subscriptionRequired")]
        public bool SubscriptionRequired { get; set; }
        [JsonPropertyName("approvalRequired")]
        public bool? ApprovalRequired { get; set; }
        [JsonPropertyName("subscriptionsLimit")]
        public int? SubscriptionsLimit { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("$refs-groups")]
        public List<string> RefsGroups { get; set; }

        [JsonPropertyName("$refs-apis")]
        public List<string> RefsApis { get; set; }


        [JsonIgnore]
        public string Description { get; set; }

        /// <summary>
        /// Verify All Reference folders Exist
        /// </summary>
        /// <param name="baseFolder">The Folder APIM Configuration is sync into</param>
        /// <returns>True if refs all exist</returns>
        public bool VerifyRefs(string baseFolder)
        {
            var refFiles = RefsApis;
            refFiles.AddRange(RefsGroups);
            if (!string.IsNullOrEmpty(RefDescription)) refFiles.Add(RefDescription);

            foreach(var refFile in refFiles)
            {
                if (!File.Exists(Path.Join(baseFolder, refFile))) return false;
            }
            return true;
        }

        public override bool Equals(Object obj)
        {
            return obj is Product && this == (Product)obj;
        }
        public static bool operator ==(Product x, Product y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                x.Name == y.Name &&
                x.RefDescription == y.RefDescription &&
                x.Terms == y.Terms &&
                x.SubscriptionRequired == y.SubscriptionRequired &&
                x.ApprovalRequired == y.ApprovalRequired &&
                x.SubscriptionsLimit == y.SubscriptionsLimit &&
                x.State == y.State &&
                ApimUtils.ListsAreEqual(x.RefsGroups, y.RefsGroups) &&
                ApimUtils.ListsAreEqual(x.RefsApis, y.RefsApis);
        }
        public static bool operator !=(Product x, Product y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return (Id, Name, RefDescription, Terms, SubscriptionRequired, ApprovalRequired, SubscriptionsLimit, State, RefsGroups, RefsApis).GetHashCode();
        }
    }
}
