using System.Reflection;

namespace SmallCat.Helpers
{
    internal static class ResourceHelper
    {
        /// <summary>
        /// 从项目嵌入的资源中读取指定的资源文件。
        /// </summary>
        /// <param name="resourceName">指定的资源文件名称</param>
        /// <returns>返回的资源文件流</returns>
        public static Stream GetResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var nameSpace = assembly.GetName().Name;
            if (!resourceName.StartsWith($"{nameSpace}."))
            {
                resourceName = $"{nameSpace}.{resourceName}";
            }
            var resource = assembly.GetManifestResourceStream(resourceName);
            return resource;
        }

        public static string StreamReader(Stream resource)
        {
            string data = null;
            using (var sr = new StreamReader(resource))
            {
                data = sr.ReadToEnd();
            }
            return data;
        }

        public static List<Stream?> GetJsonResources()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resources = assembly.GetManifestResourceNames().ToList()
                .Where(e => e.ToLower().EndsWith(".json"))
                .Select(e => assembly.GetManifestResourceStream(e))
                .Where(e => e != null)
                .ToList();
            return resources;
        }
    }
}
