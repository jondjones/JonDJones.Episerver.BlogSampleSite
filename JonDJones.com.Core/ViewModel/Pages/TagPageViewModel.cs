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
using JonDJones.com.Core.Repository;

namespace JonDJones.com.Core.ViewModel.Pages
{
    public class TagPageViewModel : BaseViewModel<TagPage>
    {
        private IEpiServerDependencies _dependencies;

        public TagPageViewModel(TagPage currentPage, IEpiServerDependencies dependencies)
            : base(currentPage)
        {
            _dependencies = dependencies;
        }

        public IEnumerable<BlogSummary> GetBlogPages()
        {
            var blogRepository = new BlogRepository(_dependencies);
            return blogRepository.GetTaggedPosts(CurrentPage.ContentLink.ID.ToString());
        }
    }
}
