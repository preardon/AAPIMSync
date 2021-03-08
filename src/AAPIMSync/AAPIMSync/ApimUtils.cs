using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PReardon.AAPIMSync
{
    public static class ApimUtils
    {
        public const string ConfigurationFileName = "configuration.json";
        public static bool ListsAreEqual<T>(List<T> list1, List<T> list2)
        {
            if (list1 == null && list2 == null) return false;
            if (list1.Count != list2.Count) return false;
            foreach(var item in list1)
            {
                if (!list2.Contains(item)) return false;
            }

            return true;
        }

        public async static Task CheckRefsAsync<T>(T entity, string basePath)
        {
            var props = entity.GetType().GetProperties();
            var filteredProps = props.Where(e => e.Name.StartsWith("Ref")).ToList();

            foreach(var prop in filteredProps)
            {
                if(prop.PropertyType == typeof(string))
                {
                    string reference = (string)prop.GetValue(entity);
                    if (reference == null) break;

                    var refLocation = Path.Combine(basePath, reference);
                }
                else if (prop.PropertyType == typeof(List<string>))
                {
                    List<string> refs = (List<string>)prop.GetValue(entity);
                    if (refs == null) break;
                }
            }
        }
    }
}
