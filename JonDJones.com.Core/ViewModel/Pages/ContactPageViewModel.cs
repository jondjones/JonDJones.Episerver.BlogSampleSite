using EPiServer.Core;
using JonDJones.Com.Core.Pages;
using JonDJones.Com.Core;
using JonDJones.Com.Core.ViewModel.Base;

namespace JonDJones.Com.Core.ViewModel.Pages
{
    public class ContactPageViewModel : BaseViewModel<ContactPage>
    {
        private readonly IEpiServerDependencies _epiServerDependencies;

        private readonly ContactPage _currentPage;

        public ContactPageViewModel(ContactPage currentPage, IEpiServerDependencies epiServerDependencies)
            : base(currentPage)
        {
            _epiServerDependencies = epiServerDependencies;
        }
    }
}