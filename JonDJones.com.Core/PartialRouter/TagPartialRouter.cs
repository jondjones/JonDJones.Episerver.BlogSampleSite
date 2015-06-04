using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using EPiServer.Web.Routing.Segments;
using JonDJones.com.Core.Repository;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace JonDJones.com.Core.PartialRouter
{
    public class TagPartialRouter : IPartialRouter<TagRootPage, TagPage>
    {
        private IEpiServerDependencies _dependencies;

        private TagRepository _tagRepository;

        public TagPartialRouter(IEpiServerDependencies dependencies,
                                 TagRepository tagRepository)
        {
            _dependencies = dependencies;
            _tagRepository = tagRepository;
        }

        public object RoutePartial(TagRootPage tagRootPage, SegmentContext segmentContext)
        {
            var segment = segmentContext.GetNextValue(segmentContext.RemainingPath);
            var tagSegment = segment.Next;
            segmentContext.RemainingPath = segment.Remaining;

            return _tagRepository.GetTagPageByRoute(tagSegment); ;
        }

        public PartialRouteData GetPartialVirtualPath(TagPage page, string language, RouteValueDictionary routeValues, RequestContext requestContext)
        {
            return null;
        }

    }
}
