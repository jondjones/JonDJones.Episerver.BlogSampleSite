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
    public class BlogPartialRouter : IPartialRouter<BlogHomePage, PageData>
    {
        private IEpiServerDependencies _dependencies;

        private BlogRepository _blogRepository;

        private CategoryRepository _categoryRepository;

        private ContentReference _blogPage;

        public BlogPartialRouter(IEpiServerDependencies dependencies,
                                 CategoryRepository categoryRepository,
                                 BlogRepository blogRepository,
                                 ContentReference blogPage)
        {
            _dependencies = dependencies;
            _categoryRepository = categoryRepository;
            _blogRepository = blogRepository;
            _blogPage = blogPage;
        }

        public object RoutePartial(BlogHomePage blogHomePage, SegmentContext segmentContext)
        {
            var categoryPageSegment = TryGetCategoryPageSegment(segmentContext);

            var blogPageSegment = TryGetBlogPageSegment(segmentContext);

            if (!string.IsNullOrEmpty(blogPageSegment))
            {
                return _blogRepository.GetBlogPageByRoute(blogPageSegment);
            }

            var categoryPage = _categoryRepository.GetCategoryPageByRoute(categoryPageSegment.ToLower());

            if (categoryPage != null)
            {
                segmentContext.RoutedContentLink = categoryPage.ContentLink;
            }

            return categoryPage;
        }

        private static string TryGetBlogPageSegment(SegmentContext segmentContext)
        {
            var segment = segmentContext.GetNextValue(segmentContext.RemainingPath);
            var blogSegment = segment.Next;
            segmentContext.RemainingPath = segment.Remaining;

            return blogSegment;
        }

        private static string TryGetCategoryPageSegment(SegmentContext segmentContext)
        {
            var segment = segmentContext.GetNextValue(segmentContext.RemainingPath);
            var categorySegment = segment.Next;
            segmentContext.RemainingPath = segment.Remaining;

            return categorySegment;
        }

        public PartialRouteData GetPartialVirtualPath(PageData page, string language, RouteValueDictionary routeValues, RequestContext requestContext)
        {
            return null;
        }

    }
}
