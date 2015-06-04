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
    [ContentType(DisplayName = "Blog Category",
        GUID = "D8D9F22D-A051-4E0A-84F0-147ABBAB7ECB",
        Description = "Blog Category")]
    public class BlogCategory : SiteBlockData
    {
        [Display(Name = "Widget Title",
             Description = "Widget Title",
             GroupName = SystemTabNames.Content,
             Order = 10)]
        public virtual string WidgetTitle { get; set; }
    }
}
