using EPiServer.Core;
using JonDJones.Com.Core.ViewCore;
using JonDJones.Com.Core.ViewCore.Shared;

namespace JonDJones.Com.Core.Interfaces
{
    public interface IPageViewModel<out T> where T : PageData
    {
        LayoutViewModel Layout { get; set; }

        T CurrentPage { get; }
    }
}