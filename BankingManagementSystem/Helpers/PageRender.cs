using System.IO;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace BankingManagementSystem.Helpers
{
    public static class PageRenderer
    {
        public static string RenderPage(string virtualPath)
        {
            var page = BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(Page)) as Page;
            if (page == null)
                throw new HttpException("Page not found: " + virtualPath);

            using (var output = new StringWriter())
            {
                HttpContext.Current.Server.Execute(virtualPath, output, false);
                return output.ToString();
            }
        }
    }
}
