using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using JonDJones.Com.Core.ContextResolver;

namespace JonDJones.Com.Core.ContentRepository
{
    public class ContextResolver : IContextResolver
    {
        public PageReference RootPage 
        { 
            get
            {
                return ContentReference.RootPage;
            }
        }

        public PageReference StartPage
        {
            get
            {
                return ContentReference.StartPage;
            }
        }

        public ContentReference GlobalBlockFolder
        {
            get
            {
                return ContentReference.GlobalBlockFolder;
            }
        }
    }
}