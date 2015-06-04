using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using JonDJones.Com.Core.Pages.Base;
using EPiServer.DataAbstraction;
using System;
using JonDJones.com.Core.SelectionFactory;
using EPiServer.Web;

namespace JonDJones.Com.Core.Pages
{

    [ContentType(
        DisplayName = "Blog Page",
        GUID = "374441cd-8412-4cea-9ff4-0e3e73c6988e",
        Description = "Blog Page",
        GroupName = "Blog")]
    public class BlogPage : GlobalBasePage
    {
        [Display(Name = "Tag",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        [SelectMany(SelectionFactoryType = typeof(TagsFactory))]
        public virtual string Tag { get; set; }


        [Display(Name = "Blog Category",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        [SelectOne(SelectionFactoryType = typeof(CategoriesFactory))]
        public virtual string BlogCategory { get; set; }

        [UIHint(UIHint.Image)]
        [Display(
            Name = "Image",
            GroupName = SystemTabNames.Content,
            Order = 125)]
        public virtual ContentReference BlogImage { get; set; }

        [Display(
            Name = "Blog Title",
            Description = "Blog Title",
            GroupName = SystemTabNames.Content,
            Order = 150)]
        [CultureSpecific]
        public virtual string BlogTitle { get; set; }

        [Display(
            Name = "Blog Author",
            Description = "Blog Author",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        [CultureSpecific]
        public virtual string BlogAuthor { get; set; }

        [Display(
            Name = "Blog Post",
            Description = "Blog Post",
            GroupName = SystemTabNames.Content,
            Order = 250)]
        public virtual XhtmlString BlogPost { get; set; }
    }
}