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
    [ContentType(DisplayName = "Blog Tags",
        GUID = "B3486083-ADD1-4AA9-800A-7BA42A585EE1",
        Description = "Blog Search")]
    public class BlogTags : SiteBlockData
    {
        [Display(Name = "Widget Title",
            Description = "Widget Title",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string WidgetTitle { get; set; }
    }
}
