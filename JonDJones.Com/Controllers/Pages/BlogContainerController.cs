using System.Web.Mvc;
using JonDJones.Com.Core.Pages;

using EPiServer.Core;
using JonDJones.Com.Core.ViewModel;
using JonDJones.Com.Controllers.Base;
using JonDJones.Com.Core.ViewModel.Pages;
using EPiServer.Web;
using JonDJones.com.Core.Repository;

namespace JonDJones.Com.Controllers.Pages
{
    public class BlogHomePageController : BasePageController<BlogHomePage>
    {
        public ActionResult Index(BlogHomePage currentPage)
        {
            return View("Index", new BlogHomePageViewModel(currentPage, EpiServerDependencies));
        }
    }
}