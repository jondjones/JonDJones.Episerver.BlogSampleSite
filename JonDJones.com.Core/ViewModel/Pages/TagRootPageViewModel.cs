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
    public class TagRootPageViewModel : BaseViewModel<TagRootPage>
    {
        private readonly IEpiServerDependencies _epiServerDependencies;

        public TagRootPageViewModel(TagRootPage currentPage, IEpiServerDependencies epiServerDependencies)
            : base(currentPage)
        {
            _epiServerDependencies = epiServerDependencies;
        }

        public IEnumerable<TagPage> GetAllTagPages()
        {
            var tagRepository = new TagRepository(_epiServerDependencies);
            return tagRepository.GetTagPages();
        }
    }
}