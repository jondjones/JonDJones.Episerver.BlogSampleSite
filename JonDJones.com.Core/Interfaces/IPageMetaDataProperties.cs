using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.Interfaces
{
    public interface IPageMetaDataProperties
    {
        string SeoTitle { get; }

        string Keywords { get; }

        XhtmlString Description { get; }

        string Author { get; set; }
    }
}
