using System.Web.Mvc;
using JonDJones.Com.Core.Pages;

using EPiServer.Core;
using JonDJones.Com.Core.ViewModel;
using JonDJones.Com.Controllers.Base;
using JonDJones.Com.Core.ViewModel.Pages;

namespace JonDJones.Com.Controllers.Pages
{
    public class ContactPageController : BasePageController<ContactPage>
    {
        public ActionResult Index(ContactPage currentPage)
        {
            return View("Index", new ContactPageViewModel(currentPage, EpiServerDependencies));
        }
    }
}