using EPiServer.Core;
using JonDJones.com.Core.SelectionFactory;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Pages;
using JonDJones.Com.Core.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JonDJones.com.Core.Extensions;
using EPiServer.Editor;
using JonDJones.com.Core.Entities;
namespace JonDJones.com.Core.ViewModel.Pages
{
    public class BlogPageViewModel : BaseViewModel<BlogPage>
    {
        private IEpiServerDependencies _dependencies;

        public BlogPageViewModel(BlogPage currentPage, IEpiServerDependencies dependencies)
            : base(currentPage)
        {
            _dependencies = dependencies;
        }

        public LinkDto Category
        {
            get
            {
                var category = _dependencies.ContentRepository.Get<CategoryPage>(new ContentReference(CurrentPage.BlogCategory));

                return (category != null) ? new LinkDto { Name = category.CategoryName, Url = category.LinkURL } : null;
            }
        }

        public List<TagPage> GetTags()
        {
            var list = new List<TagPage>();
            var tagFactory = new TagsFactory();

            foreach (var tag in CurrentPage.Tag.Split(','))
            {
                var tagpage = _dependencies.ContentRepository.Get<TagPage>(new ContentReference(tag));

                if (tagpage != null)
                    list.Add(tagpage);

            }

            return list;
        }

        public string ImageUrl
        {
            get
            {
                return CurrentPage.BlogImage.IfNotDefault(_dependencies.LinkResolver.ResolveUrl) ?? string.Empty;
            }
        }
    }
}
