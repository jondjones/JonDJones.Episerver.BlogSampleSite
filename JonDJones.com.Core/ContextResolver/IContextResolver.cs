using EPiServer.Core;

namespace JonDJones.Com.Core.ContextResolver
{
    public interface IContextResolver
    {
        PageReference StartPage { get; }

        PageReference RootPage { get; }

        ContentReference GlobalBlockFolder { get; }
    }
}