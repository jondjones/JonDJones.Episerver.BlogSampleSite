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
        DisplayName = "Category Root Page",
        GUID = "58A42E12-C501-4340-A7D9-D8FAA4E3CA74",
        Description = "Blog Home Page",
        GroupName = "Blog")]
    public class CategoryRootPage : PageData
    {
    }
}