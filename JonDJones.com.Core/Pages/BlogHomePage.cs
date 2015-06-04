using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using JonDJones.Com.Core.Pages.Base;
using EPiServer.DataAbstraction;
using EPiServer.Shell.ViewComposition;

namespace JonDJones.Com.Core.Pages
{

    [ContentType(
        DisplayName = "Blog Home Page",
        GUID = "858b76f0-e295-4f48-995d-19da8f47d3c0",
        Description = "Blog Home Page",
        GroupName = "Blog")]
    public class BlogHomePage : GlobalBasePage
    {
        [Display(
            Name = "Main Title",
            Description = "Main Title",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        public virtual string MainTitle { get; set; }


        [Display(
            Name = "Main Content Area",
            Description = "Region where content blocks can be placed",
            GroupName = SystemTabNames.Content,
            Order = 300)]
        public virtual ContentArea MainContentArea { get; set; }

        [Display(
            Name = "Sidebar",
            Description = "Sidebar",
            GroupName = SystemTabNames.Content,
            Order = 400)]
        public virtual ContentArea Sidebar { get; set; }
    }
}