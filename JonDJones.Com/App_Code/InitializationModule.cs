using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using JonDJones.com.Core.PartialRouter;
using JonDJones.com.Core.Repository;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using EPiServer.Web.Routing;
using JonDJones.com.Core.Segment;
using EPiServer.Web.Routing.Segments;

namespace JonDJones.Com.Initialization
{
    [InitializableModule]
    public class DataInitialization : IInitializableModule
    {
        Injected<IEpiServerDependencies> epiServerDependencies;

        public void Initialize(InitializationEngine context)
        {
            var dependency = epiServerDependencies.Service;

            var segment = new CustomSegment("custom");
            var routingParameters = new MapContentRouteParameters()
            {
                SegmentMappings = new Dictionary<string, ISegment>()
            };

            routingParameters.SegmentMappings.Add("custom", segment);

            RouteTable.Routes.MapContentRoute(
              name: "tags",
              url: "{language}/{custom}",
              defaults: new { action = "index" },
              parameters: routingParameters);

            var blogRouter = new BlogPartialRouter(dependency, new CategoryRepository(dependency), new BlogRepository(dependency), new ContentReference(39));
            RouteTable.Routes.RegisterPartialRouter<BlogHomePage, PageData>(blogRouter);

            var tagRouter = new TagPartialRouter(dependency, new TagRepository(dependency));
            RouteTable.Routes.RegisterPartialRouter<TagRootPage, TagPage>(tagRouter);
        }
        public void Uninitialize(InitializationEngine context)
        {

        }
        public void Preload(string[] parameters)
        {
        }
    }
}