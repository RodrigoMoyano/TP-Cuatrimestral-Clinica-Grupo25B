using System;
using System.Web;
using System.Web.Routing;

namespace presentacion // <--- ¿Dice 'presentacion'?
{
    public class Global : HttpApplication // <--- ¿La clase se llama 'Global'?
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Código de inicio de la aplicación
            // RouteConfig.RegisterRoutes(RouteTable.Routes);
            // BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}