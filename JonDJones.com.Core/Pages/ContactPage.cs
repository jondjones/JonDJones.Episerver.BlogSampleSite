using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using JonDJones.Com.Core.Pages.Base;
using EPiServer.DataAbstraction;

namespace JonDJones.Com.Core.Pages
{
    [ContentType(
        DisplayName = "Contact Page",
        GUID = "997792F4-9392-40E7-ACB6-611C233850AF",
        Description = "Contact Page",
        GroupName = "Standard")]
    public class ContactPage : GlobalBasePage
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