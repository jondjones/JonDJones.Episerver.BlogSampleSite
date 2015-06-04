using System.Web.Mvc;
using JonDJones.Com.Core.Pages;

using EPiServer.Core;
using JonDJones.Com.Core.ViewModel;
using JonDJones.Com.Controllers.Base;
using JonDJones.Com.Core.ViewModel.Pages;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace JonDJones.Com.Controllers.Pages
{
    public class CategoryPageController : BasePageController<CategoryPage>, IRenderTemplate<CategoryPage>
    {
        public ActionResult Index(CategoryPage currentPage)
        {
            var categoryPage = Request.RequestContext.GetRoutedData<CategoryPage>();
            return View(new CategoryPageViewModel(currentPage, EpiServerDependencies));
        }
    }
}