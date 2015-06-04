using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using JonDJones.Com.Core.Pages.Base;
using EPiServer.DataAbstraction;

namespace JonDJones.Com.Core.Pages
{

    [ContentType(
        DisplayName = "Content Page",
        GUID = "8dee011f-8dbf-43ab-b4f3-211db5ceb9d5",
        Description = "Content Page",
        GroupName = "Standard")]
    public class ContentPage : GlobalBasePage
    {
        [Display(
            Name = "Page Title",
            Description = "Page Title",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        [CultureSpecific]
        public virtual string PageTitle { get; set; }

        [Display(
            Name = "Main Content Area",
            Description = "Region where content blocks can be placed",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        public virtual ContentArea MainContentArea { get; set; }
    }
}