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
    public class BlogPageController : BasePageController<BlogPage>, IRenderTemplate<BlogPage>
    {
        public ActionResult Index(BlogPage currentPage)
        {
            var newsContent = Request.RequestContext.GetRoutedData<BlogPage>();
            return View(new BlogPageViewModel(newsContent, EpiServerDependencies));
        }
    }
}