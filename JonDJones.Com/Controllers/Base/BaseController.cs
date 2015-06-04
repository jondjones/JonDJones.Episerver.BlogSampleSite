using EPiServer.Core;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using JonDJones.Com.Core;
using JonDJones.Com.Core.ViewModel;
using System;
using System.Threading;
using System.Web.Mvc;

namespace JonDJones.Com.Controllers.Base
{
    public class BasePageController<T> : PageController<T>
        where T : PageData
    {
        internal Injected<IEpiServerDependencies> EpiServerDependenciesService;

        internal IEpiServerDependencies EpiServerDependencies
        {
            get { return EpiServerDependenciesService.Service; }
        }

        [HttpPost]
        public RedirectResult SetLanguage(string selectedCountryCode, PageData currentPage)
        {
            var currentCulture = new System.Globalization.CultureInfo(selectedCountryCode);

            ContentLanguage.PreferredCulture = currentCulture;
            Thread.CurrentThread.CurrentCulture = currentCulture;

            var url = EPiServer.UriSupport.AddLanguageSelection(currentPage.LinkURL, selectedCountryCode);

            return Redirect(url);
        }
    }
}