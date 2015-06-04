using EPiServer.Core;
using JonDJones.Com.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JonDJones.com.Core.Extensions;

namespace JonDJones.com.Core.Helpers
{
    public class BlockHelper : IBlockHelper
    {
        private IEpiServerDependencies _epiServerDependencies;
        
        public BlockHelper(IEpiServerDependencies epiServerDependencies)
        {
            _epiServerDependencies = epiServerDependencies;
        }

        public IEnumerable<T> GetContentsOfType<T>(ContentArea contentArea)
        {
            return contentArea.IfNotDefault(ca => ca.Items.EmptyIfNull())
                .EmptyIfNull()
                .Select(item => _epiServerDependencies.ContentRepository.Get<IContentData>(item.ContentLink))
                .OfType<T>();
        }
    }
}
