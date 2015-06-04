using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using JonDJones.com.Core.Blocks.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.Blocks
{
    [ContentType(DisplayName = "Blog Listing",
        GUID = "8FEC3E5C-04B8-4E52-9F22-4B37A45D9DB3",
        Description = "Blog Listing")]
    public class BlogListing : SiteBlockData
    {

        [Display(
            Name = "Display Thumbnails",
            Description = "Display Thumbnails",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual bool DisplayThumbNails { get; set; }
    }
}
