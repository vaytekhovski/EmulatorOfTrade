using System.IO;
using System.Web.Hosting;

namespace QuickType
{
    

    public static class JsonToString
    {
        public static string GetString(string site, string fileName)
        {
            string path = HostingEnvironment.MapPath("/Content/" + fileName + ".txt");
            LoadFromPoliniex.GetStringHtml(site, path);
            return File.ReadAllText(path);
        }
    }
}
