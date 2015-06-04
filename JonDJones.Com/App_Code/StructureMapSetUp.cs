using System.Web.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using StructureMap;
using JonDJones.Com.Core.ContentRepository;
using JonDJones.Com.Core.Routing;
using JonDJones.Com.Core.ContextResolver;
using JonDJones.Com.Core;
using JonDJones.com.Core.Helpers;

namespace JonDJones.Com.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class StructureMapSetUp : IConfigurableModule
    {
        private IContainer _container;

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Container.Configure(ConfigureContainer);

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(context.Container));
            _container = context.Container;
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        private static void ConfigureContainer(ConfigurationExpression container)
        {
            container.For<IEpiServerDependenciesResolver>().Use<EpiServerDependenciesResolver>();
            container.For<IBlockHelper>().Use<BlockHelper>();
            container.For<IContextResolver>().Use<ContextResolver>();
            container.For<ILinkResolver>().Use<LinkResolver>()
                .Ctor<IEpiServerDependenciesResolver>().Is<EpiServerDependenciesResolver>();
 
            container.For<IEpiServerDependencies>()
                .Use<EpiServerDependencies>();
        }
    }
}