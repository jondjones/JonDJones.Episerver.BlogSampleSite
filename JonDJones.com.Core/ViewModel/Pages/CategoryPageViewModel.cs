using EPiServer.Core;
using JonDJones.Com.Core.Pages;
using JonDJones.Com.Core;
using JonDJones.Com.Core.ViewModel.Base;
using JonDJones.com.Core.Entities;
using System.Collections.Generic;
using JonDJones.com.Core.Repository;

namespace JonDJones.Com.Core.ViewModel.Pages
{
    public class CategoryPageViewModel : BaseViewModel<CategoryPage>
    {
        private readonly IEpiServerDependencies _epiServerDependencies;

        private readonly CategoryPage _currentPage;

        public CategoryPageViewModel(CategoryPage currentPage, IEpiServerDependencies epiServerDependencies)
            : base(currentPage)
        {
            _currentPage = currentPage;
            _epiServerDependencies = epiServerDependencies;
        }


        public IEnumerable<BlogSummary> GetBlogPages()
        {
            var blogRepository = new BlogRepository(_epiServerDependencies);
            return blogRepository.GetBlogSummaries(_currentPage.ContentLink.ID.ToString());
        }
    }
}