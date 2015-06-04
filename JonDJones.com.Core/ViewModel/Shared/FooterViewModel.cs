using JonDJones.com.Core.Interfaces;
using JonDJones.Com.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.ViewModel.Shared
{
    public class FooterViewModel
    {
        private readonly IEpiServerDependencies _epiServerDependencies;

        public FooterViewModel(IFooterProperties footerProperties, IEpiServerDependencies epiServerDependencies)
        {
            Current = footerProperties;
            _epiServerDependencies = epiServerDependencies;
        }

        public IFooterProperties Current { get; private set; }

        public bool DisplayFacebookIcon
        {
            get { return !string.IsNullOrEmpty(Current.FacebookUrl); }
        }

        public bool DisplayGithubIcon
        {
            get { return !string.IsNullOrEmpty(Current.GitHubUrl); }
        }

        public bool DisplayTwiiterIcon
        {
            get { return !string.IsNullOrEmpty(Current.TwitterUrl); }
        }
    }
}
