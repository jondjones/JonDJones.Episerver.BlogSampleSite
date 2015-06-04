using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using System.Web.Http;

namespace JonDJones.Com
{
    public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Insert(0, new CustomViewEngine());

            AreaRegistration.RegisterAllAreas();
        }

        protected override void RegisterRoutes(RouteCollection routes)
        {
            base.RegisterRoutes(routes);
        }
    }
}