using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonDJones.com.Core.Helpers
{
    public interface IBlockHelper
    {
        IEnumerable<T> GetContentsOfType<T>(ContentArea contentArea);
    }
}
