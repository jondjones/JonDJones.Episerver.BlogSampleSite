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
    public class TagsFactory : ISelectionFactory
    {
        public Injected<IContentRepository> contentRepository;

        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return GetCategories().Select(x => (new SelectItem { Text = x.TagName, Value = x.ContentLink.ID.ToString() }));
        }

        protected IEnumerable<TagPage> GetCategories()
        {
            var categoryRoot = GetTagsRootPage();

            var pages =  contentRepository.Service.GetChildren<TagPage>(categoryRoot.ContentLink);
            return pages;
        }

        public TagRootPage GetTagsRootPage()
        {
            var rootPage = contentRepository.Service.GetChildren<TagRootPage>(ContentReference.RootPage);

            if (rootPage == null)
                throw new InvalidOperationException();

            return rootPage.FirstOrDefault();
        }
    }
}
