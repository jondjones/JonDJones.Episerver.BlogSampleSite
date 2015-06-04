using EPiServer.Core;
using JonDJones.Com.Core.Pages;
using JonDJones.Com.Core;
using JonDJones.Com.Core.ViewModel.Base;

namespace JonDJones.Com.Core.ViewModel.Pages
{
    public class ContentPageViewModel : BaseViewModel<ContentPage>
    {
        private readonly IEpiServerDependencies _epiServerDependencies;

        private readonly ContentPage _currentPage;

        public ContentPageViewModel(ContentPage currentPage, IEpiServerDependencies epiServerDependencies)
            : base(currentPage)
        {
            _epiServerDependencies = epiServerDependencies;
        }
    }
}