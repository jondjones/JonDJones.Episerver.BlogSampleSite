using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using JonDJones.Com.Core.Pages.Base;
using EPiServer.DataAbstraction;

namespace JonDJones.Com.Core.Pages
{
    [ContentType(
        DisplayName = "Search Page",
        GUID = "5835D525-77D9-4F5F-BAB9-D0C14E273C15",
        Description = "Search Page",
        GroupName = "Standard")]
    public class SearchPage : GlobalBasePage
    {
        [Display(
            Name = "Page Title",
            Description = "Page Title",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        [CultureSpecific]
        public virtual string PageTitle { get; set; }
    }
}