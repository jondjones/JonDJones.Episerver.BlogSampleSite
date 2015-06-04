using EPiServer.Core;
using JonDJones.com.Core.Media;
using JonDJones.com.Core.ViewModel.Pages;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JonDJones.com.Core.Extensions;
using JonDJones.com.Core.Entities;

namespace JonDJones.com.Core.Repository
{
    public class BlogRepository
    {
        private IEpiServerDependencies _dependencies;

        public BlogRepository(IEpiServerDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public IEnumerable<BlogPage> GetBlogPages()
        {
            var blogContainer = GetBlogContinerPage();

            if (blogContainer == null)
                return null;

            var pages = _dependencies.ContentRepository
                                .GetChildren<BlogPage>(blogContainer.ContentLink);

            return pages;
        }

        public IEnumerable<BlogSummary> GetBlogSummaries()
        {
            return GetBlogPages().Select(x => new BlogSummary
            {
                Name = x.Name,
                Url = HttpContext.Current.Request.ApplicationPath + _dependencies.LinkResolver.GetFriendlyURL(x.ContentLink),
                Summary = CreateSummaryText(x.BlogPost),
                ImageUrl = HttpContext.Current.Request.ApplicationPath + GetImageUrl(x.BlogImage),
                Author = x.BlogAuthor
            });
        }

        private XhtmlString CreateSummaryText(XhtmlString xhtmlString)
        {
            if (xhtmlString.ToString().Length > 250)
            {
                return new XhtmlString(xhtmlString.ToString().Substring(0, 250) + "...");
            }

            return xhtmlString;
        }

        public BlogPage GetBlogPageByRoute(string slug)
        {
            return GetBlogPages().Where(n => n.URLSegment.ToLower() == slug.ToLower()).FirstOrDefault();
        }
       
        public BlogHomePage GetBlogContinerPage()
        {
            return _dependencies.ContentRepository
                                .GetChildren<BlogHomePage>(ContentReference.RootPage)
                                .FirstOrDefault();
        }

        public BlogHomePage GetBlogHomePage()
        {
            return _dependencies.ContentRepository
                                .GetChildren<BlogHomePage>(ContentReference.StartPage)
                                .FirstOrDefault();
        }

        private string GetImageUrl(ContentReference contentReference)
        {
            if (contentReference != null)
            {
                var asset = _dependencies.ContentRepository.Get<ImageFile>(contentReference);
                return "globalassets/" + asset.RouteSegment;
            }

            return string.Empty;
        }

        internal IEnumerable<BlogSummary> GetBlogSummaries(string id)
        {
            return GetBlogPages().Where(n => n.BlogCategory == id)
                .Select(x => new BlogSummary
                {
                    Name = x.Name,
                    Url = HttpContext.Current.Request.ApplicationPath + _dependencies.LinkResolver.GetFriendlyURL(x.ContentLink),
                    Summary = CreateSummaryText(x.BlogPost),
                    ImageUrl = HttpContext.Current.Request.ApplicationPath + GetImageUrl(x.BlogImage),
                    Author = x.BlogAuthor
                });
        }

        internal IEnumerable<BlogSummary> GetTaggedPosts(string tagId)
        {
            var list = new List<BlogPage>();

            foreach (var blogPage in GetBlogPages())
            {
                if (blogPage.Tag.Split(',').Contains(tagId))
                    list.Add(blogPage);
            }

            return list.Select(x => new BlogSummary
                {
                    Name = x.Name,
                    Url = HttpContext.Current.Request.ApplicationPath + _dependencies.LinkResolver.GetFriendlyURL(x.ContentLink),
                    Summary = CreateSummaryText(x.BlogPost),
                    ImageUrl = HttpContext.Current.Request.ApplicationPath + GetImageUrl(x.BlogImage),
                    Author = x.BlogAuthor
                });
        }
    }
}
