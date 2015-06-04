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
        DisplayName = "Category Page",
        GUID = "68C1BCCC-34E9-42CE-98C5-05BE3A0EBAF0",
        Description = "Category Name",
        GroupName = "Blog")]
    public class CategoryPage : GlobalBasePage
    {
        [Display(
            Name = "Category Name",
            Description = "Category Name",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string CategoryName { get; set; }
    }
}