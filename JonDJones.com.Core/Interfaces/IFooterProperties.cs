using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.Interfaces
{
    public interface IFooterProperties
    {
        string FacebookUrl { get; set; }

        string GitHubUrl { get; set; }

        string TwitterUrl { get; set; }

        string CopyRightNotice { get; set; }
    }
}
