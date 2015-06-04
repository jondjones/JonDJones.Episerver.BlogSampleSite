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
    [ContentType(DisplayName = "Side Widget",
        GUID = "90FAEA4A-6BDE-4B8C-B78F-B5F76B4672C5",
        Description = "Side Widget")]
    public class SideWidget : SiteBlockData
    {

        [Display(Name = "Widget Title",
             Description = "Widget Title",
             GroupName = SystemTabNames.Content,
             Order = 10)]
        public virtual string WidgetTitle { get; set; }

        [Display(Name = "Main Text",
            Description = "Main Text",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual XhtmlString MainText { get; set; }
    }
}
