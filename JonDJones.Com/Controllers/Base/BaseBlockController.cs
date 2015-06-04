using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using JonDJones.Com.Core;

namespace JonDJones.Com.Controllers.Base
{
    public abstract class BaseBlockController<TBlockData> : BlockController<TBlockData>
         where TBlockData : BlockData
    {
        private Injected<EpiServerDependencies> epiServerDependencies;

        private Injected<PageRouteHelper> routeHelper;

        protected PageData CurrentPage
        {
            get
            {
                return routeHelper.Service.Page;
            }
        }

        protected EpiServerDependencies EpiServerDependencies
        {
            get
            {

                var value = epiServerDependencies.Service;
                value.CurrentPage = CurrentPage;

                return value;
            }
        }
    }
}