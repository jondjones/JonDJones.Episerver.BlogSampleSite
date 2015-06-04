using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using JonDJones.Com.Core.Pages.Base;
using EPiServer.DataAbstraction;
using System;

namespace JonDJones.Com.Core.Pages
{

    [ContentType(
        DisplayName = "Tag Page",
        GUID = "BCE07368-CFBD-4306-802B-8BA2EE3BC652",
        Description = "Tag Page",
        GroupName = "Blog")]
    public class TagPage : GlobalBasePage
    {
        [Display(
            Name = "Tag Name",
            Description = "Tag Name",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string TagName { get; set; }
    }
}