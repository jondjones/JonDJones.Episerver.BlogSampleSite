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
        DisplayName = "Tag Root Page",
        GUID = "F381CC57-D60E-4670-BFC5-95DFF93F1F39",
        Description = "Tag Root Page",
        GroupName = "Blog")]
    public class TagRootPage : GlobalBasePage
    {
    }
}