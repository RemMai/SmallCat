using System.Reflection;
using SmallCat.DynamicWebApi;

namespace SmallCat.DynamicWebApi
{
    public static class AppConsts
    {
        public static string DefaultHttpVerb { get; set; }

        public static string DefaultAreaName { get; set; }

        public static string DefaultApiPreFix { get; set; }

        public static List<string> ControllerPostfixes { get; set; }
        public static List<string> ActionPostfixes { get; set; }

        public static List<Type> FormBodyBindingIgnoredTypes { get; set; }

        public static Dictionary<string, string> HttpVerbs { get; set; }

        public static Func<string, string> GetRestFulActionName { get; set; }

        public static Dictionary<Assembly, AssemblyDynamicWebApiOptions> AssemblyDynamicWebApiOptions { get; set; }

        static AppConsts()
        {
            HttpVerbs = new Dictionary<string, string>
            {
                // POST
                ["add"]    = "POST",
                ["create"] = "POST",
                ["post"]   = "POST",

                // GET
                ["get"]    = "GET",
                ["find"]   = "GET",
                ["fetch"]  = "GET",
                ["query"]  = "GET",

                // PUT
                ["update"] = "PUT",
                ["put"]    = "PUT",

                // DELETE
                ["delete"] = "DELETE",
                ["remove"] = "DELETE",
            };
        }
    }
}