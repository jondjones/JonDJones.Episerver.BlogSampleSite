using System.Web.Mvc;
using JonDJones.Com.Core.Pages;

using EPiServer.Core;
using JonDJones.Com.Core.ViewModel;
using JonDJones.Com.Controllers.Base;
using JonDJones.Com.Core.ViewModel.Pages;

namespace JonDJones.Com.Controllers.Pages
{
    public class ContentPageController : BasePageController<ContentPage>
    {
        public ActionResult Index(ContentPage currentPage)
        {
            return View("Index", new ContentPageViewModel(currentPage, EpiServerDependencies));
        }
    }
}