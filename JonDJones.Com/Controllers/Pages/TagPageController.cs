using System.Web.Mvc;
using JonDJones.Com.Core.Pages;

using EPiServer.Core;
using JonDJones.Com.Core.ViewModel;
using JonDJones.Com.Controllers.Base;
using JonDJones.Com.Core.ViewModel.Pages;
using EPiServer.Web;
using EPiServer.Web.Routing;
using JonDJones.com.Core.ViewModel.Pages;

namespace JonDJones.Com.Controllers.Pages
{
    public class TagPageController : BasePageController<TagPage>, IRenderTemplate<TagPage>
    {
        public ActionResult Index(TagPage currentPage)
        {
            var tagPage = Request.RequestContext.GetRoutedData<TagPage>();
            return View(new TagPageViewModel(tagPage, EpiServerDependencies));
        }
    }
}