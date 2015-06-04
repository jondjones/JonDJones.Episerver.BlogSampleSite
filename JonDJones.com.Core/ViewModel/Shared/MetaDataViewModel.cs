using JonDJones.com.Core.Interfaces;
using JonDJones.Com.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.ViewModel.Shared
{
    public class MetaDataViewModel
    {
        private readonly ILinkResolver _linkResolver;

        public MetaDataViewModel(IPageMetaDataProperties model, ILinkResolver linkResolver)
        {
            Current = model;
            _linkResolver = linkResolver;
        }

        public IPageMetaDataProperties Current { get; private set; }

        public bool HasTitle
        {
            get { return string.IsNullOrEmpty(Current.SeoTitle); }
        }
    }
}
