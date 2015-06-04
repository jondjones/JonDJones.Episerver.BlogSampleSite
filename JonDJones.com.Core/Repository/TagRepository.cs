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
    public class TagRepository
    {
        private IEpiServerDependencies _dependencies;

        public TagRepository(IEpiServerDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public IEnumerable<TagPage> GetTagPages()
        {
            var tagContainer = GetTagContinerPage();

            if (tagContainer == null)
                return null;

            var tags = _dependencies.ContentRepository
                                .GetChildren<TagPage>(tagContainer.ContentLink);

            return tags;
        }

        public TagRootPage GetTagContinerPage()
        {
            return _dependencies.ContentRepository
                                .GetChildren<TagRootPage>(ContentReference.RootPage)
                                .FirstOrDefault();
        }

        public TagRootPage GetTagHomePage()
        {
            return _dependencies.ContentRepository
                                .GetChildren<TagRootPage>(ContentReference.StartPage)
                                .FirstOrDefault();
        }

        internal TagPage GetTagPageByRoute(string slug)
        {
            return GetTagPages().Where(n => n.URLSegment.ToLower() == slug.ToLower()).FirstOrDefault();
        }
    }
}
