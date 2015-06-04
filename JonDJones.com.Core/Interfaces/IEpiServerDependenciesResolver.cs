using EPiServer;
using EPiServer.Web.Routing;
using JonDJones.Com.Core.ContextResolver;
using JonDJones.Com.Core.Routing;
namespace JonDJones.Com.Core
{
    public interface IEpiServerDependenciesResolver
    {
        UrlResolver GetUrlResolver();

        ILinkResolver GetLinkResolver();

        IContextResolver GetContextResolver();

        IContentRepository GetContentRepository();
    }
}