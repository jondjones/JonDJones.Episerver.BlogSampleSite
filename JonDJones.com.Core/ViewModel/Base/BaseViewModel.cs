using EPiServer.Core;
using JonDJones.Com.Core.Interfaces;
using JonDJones.Com.Core.Pages;
using JonDJones.Com.Core.Pages.Base;
using JonDJones.Com.Core.ViewCore;
using JonDJones.Com.Core.ViewCore.Shared;

namespace JonDJones.Com.Core.ViewModel.Base
{
    public class BaseViewModel<T> : IPageViewModel<T> where T : GlobalBasePage
    {
        public BaseViewModel(T currentPage)
        {
            CurrentPage = currentPage;
        }

        public T CurrentPage { get; private set; }

        public LayoutViewModel Layout { get; set; }
    }
}