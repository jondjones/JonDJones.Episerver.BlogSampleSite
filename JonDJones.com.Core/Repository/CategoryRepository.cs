using EPiServer.Core;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.Repository
{
    public class CategoryRepository
    {
        private IEpiServerDependencies _dependencies;

        public CategoryRepository(IEpiServerDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public IEnumerable<CategoryPage> GetCategoryPages()
        {
            var categoryHomePage = GetCategoryRootPage();

            if (categoryHomePage == null)
                return null;

            var pages = _dependencies.ContentRepository.GetChildren<CategoryPage>(categoryHomePage);
            return pages;
        }

        public CategoryPage GetCategoryPageByRoute(string name, string slug)
        {
            return GetCategoryPages().Where(n => n.Name == name
                && n.URLSegment == slug).FirstOrDefault();
        }

        public ContentReference GetCategoryRootPage()
        {
            return _dependencies.ContentRepository.Get<CategoryRootPage>(new ContentReference(40)).ContentLink;
        }


        public CategoryPage GetCategoryPageByRoute(string slug)
        {
            return GetCategoryPages().Where(n => n.URLSegment == slug).FirstOrDefault();
        }

        public CategoryPage GetCategoryPage(string pageId)
        {
            return _dependencies.ContentRepository.Get<CategoryPage>(new ContentReference(pageId));
        }
    }
}
