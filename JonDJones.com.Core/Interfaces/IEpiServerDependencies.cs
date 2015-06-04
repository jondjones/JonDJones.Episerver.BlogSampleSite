using EPiServer;
using EPiServer.Core;
using JonDJones.Com.Core.ContextResolver;
using JonDJones.Com.Core.Routing;

namespace JonDJones.Com.Core
{
    public interface IEpiServerDependencies
    {
        ILinkResolver LinkResolver { get; }

        IContentRepository ContentRepository { get; }

        IContextResolver ContextResolver { get; }

        PageData CurrentPage { get; set; }
    }
}
