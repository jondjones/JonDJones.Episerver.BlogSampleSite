using EPiServer;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.ServiceLocation;
using EPiServer.Web;
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

namespace JonDJones.com.Core.Segment
{
    public class CustomSegment : SegmentBase
    {
        Injected<IEpiServerDependencies> _dependencies;

        public CustomSegment(string name)
            : base(name)
        {
        }

        public override bool RouteDataMatch(SegmentContext context)
        {
            return false;
        }

        public override string GetVirtualPathSegment(
          RequestContext requestContext,
          RouteValueDictionary values)
        {
            if (!IsInDeafultView(requestContext.HttpContext, requestContext.RouteData))
                return null;
   

            var contentLink = ContentRoute.GetValue("node", requestContext, values)
                                as ContentReference;

            if (ContentReference.IsNullOrEmpty(contentLink))
                return null;

            var catPage = _dependencies.Service.ContentRepository.Get<IContent>(contentLink) as CategoryPage;
            if (catPage != null)
            {
                ClearRouteValues(values);
                return catPage.CategoryName;
            }

            var blogPage = _dependencies.Service.ContentRepository.Get<IContent>(contentLink) as BlogPage;
            if (blogPage != null)
            {
                ClearRouteValues(values);

                var categoryRepository = new CategoryRepository(_dependencies.Service);
                var blogRepository = new BlogRepository(_dependencies.Service);

                var categoryPage = categoryRepository.GetCategoryPage(blogPage.BlogCategory);

                var blogHomepage = _dependencies.Service.ContentRepository
                                                        .Get<IContent>(contentLink) as BlogHomePage;

                var route = string.Empty;

                if (blogHomepage == null)
                    route = blogRepository.GetBlogHomePage().URLSegment + "/";

                return string.Format("{0}{1}/{2}", route, categoryPage.CategoryName, blogPage.URLSegment);
            }

            return null;
        }

        private static void ClearRouteValues(RouteValueDictionary values)
        {

            values.Remove("node");
            values.Remove("controller");
            values.Remove("action");
            values.Remove("routedData");
        }

        private static bool IsInDeafultView(
                              HttpContextBase httpContext,
                              RouteData routeData)
        {
            var contextModeKey = "contextmode";

            var contextMode = ContextMode.Undefined;

            if (routeData.DataTokens.ContainsKey(contextModeKey))
            {
                contextMode = (ContextMode)routeData.DataTokens[contextModeKey];
            }

            if ((httpContext == null) || (httpContext.Request == null) || contextMode == ContextMode.Default)
            {
                return true;
            }
            return false;
        }
    }

}
