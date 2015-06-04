using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using JonDJones.Com.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.SelectionFactory
{
    public class CategoriesFactory : ISelectionFactory
    {
        public Injected<IContentRepository> contentRepository;

        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return GetCategories().Select(x => (new SelectItem { Text = x.CategoryName, Value = x.ContentLink.ID.ToString() }));
        }

        protected IEnumerable<CategoryPage> GetCategories()
        {
            var categoryRoot = GetCategoryRootPage();

            var pages =  contentRepository.Service.GetChildren<CategoryPage>(categoryRoot.ContentLink);
            return pages;
        }

        private CategoryRootPage GetCategoryRootPage()
        {
            var rootPage = contentRepository.Service.Get<CategoryRootPage>(new ContentReference(40));

            if (rootPage == null)
                throw new InvalidOperationException();

            return rootPage;
        }
    }
}
