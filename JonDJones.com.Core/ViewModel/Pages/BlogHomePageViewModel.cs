using EPiServer.Core;
using JonDJones.Com.Core.Pages;
using JonDJones.Com.Core;
using JonDJones.Com.Core.ViewModel.Base;
using JonDJones.com.Core.Repository;
using System.Collections.Generic;
using System.Linq;
using JonDJones.com.Core.ViewModel.Pages;
using System.Web.UI;
using System.Web;
using JonDJones.com.Core.Entities;

namespace JonDJones.Com.Core.ViewModel.Pages
{
    public class BlogHomePageViewModel : BaseViewModel<BlogHomePage>
    {
        private readonly IEpiServerDependencies _epiServerDependencies;

        public BlogHomePageViewModel(BlogHomePage currentPage, IEpiServerDependencies epiServerDependencies)
            : base(currentPage)
        {
            _epiServerDependencies = epiServerDependencies;
        }

        public IEnumerable<BlogSummary> GetBlogPages()
        {
            var blogRepository = new BlogRepository(_epiServerDependencies);
            return blogRepository.GetBlogSummaries();
        }
    }
}