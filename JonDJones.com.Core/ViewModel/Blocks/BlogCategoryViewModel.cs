using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using JonDJones.com.Core.Blocks;
using JonDJones.com.Core.Entities;
using JonDJones.com.Core.Repository;
using JonDJones.com.Core.ViewModel.Pages;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Pages;
using JonDJones.Com.Core.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.ViewModel.Blocks
{
    public class BlogCategoryViewModel : BlockViewModel<BlogCategory>
    {
        public BlogCategoryViewModel(BlogCategory currentBlock, IEpiServerDependencies epiServerDependencies)
            : base(currentBlock, epiServerDependencies)
        {
        }
        public IEnumerable<LinkDto> Categories
        {
            get
            {
                var category = new CategoryRepository(_epiServerDependencies);
                return category.GetCategoryPages().Select(x => new LinkDto
                { 
                    Name = x.CategoryName,
                    Url = _epiServerDependencies.LinkResolver.GetFriendlyURL(x.ContentLink)
                });
            }
        }
    }
}
