using System.Web;
using System.Web.Mvc;

namespace Petopia
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // from Ben Foster article about doing custom error pages:
            // "Ditch the MVC HandleErrorAttribute global filter 
            //   and configure ASP.NET’s custom errors (in root-level Web.config)"
            //
            //filters.Add(new HandleErrorAttribute());
            //
            // but i didn't wanna get rid of it all the way, haha
            //
            // okay -- so tried it with that in & out.....
            // with various combinations in Web.config, of what the "Demystifying"
            // article says to do.....
            // no more "YSOD"..... but also no Error Kitty!!!
            // Dammit.  stupid fucking custom error pages on MVC!!!!!
        }
    }
}
