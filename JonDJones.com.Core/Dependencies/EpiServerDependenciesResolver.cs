using EPiServer;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using JonDJones.Com.Core.ContextResolver;
using JonDJones.Com.Core.Routing;

namespace JonDJones.Com.Core.ContentRepository
{
    public class EpiServerDependenciesResolver : IEpiServerDependenciesResolver
    {
        internal Injected<UrlResolver> UrlResolverService;

        internal Injected<IContentRepository> ContentRepositoryService;

        internal Injected<IContextResolver> ContextResolver;

        internal Injected<ILinkResolver> LinkResolver;

        public UrlResolver GetUrlResolver()
        {
            return UrlResolverService.Service;
        }

        public ILinkResolver GetLinkResolver()
        {
            return LinkResolver.Service;
        }

        public IContextResolver GetContextResolver()
        {
            return ContextResolver.Service;
        }

        public IContentRepository GetContentRepository()
        {
            return ContentRepositoryService.Service;
        }
    }
}