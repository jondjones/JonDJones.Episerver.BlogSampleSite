using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using JonDJones.com.Core.Interfaces;
using JonDJones.com.Core.Resources;
using EPiServer.Web;

namespace JonDJones.Com.Core.Pages.Base
{

    public abstract class GlobalBasePage : PageData, IPageMetaDataProperties
    {
        #region Meta Data Settings
        [Display(
            Name = "Meta Title",
            GroupName = ResourceDefinitions.TabNames.MetaData,
            Order = 2000)]
        [Required]
        public virtual string SeoTitle { get; set; }

        [Display(
            Name = "Meta Keywords",
            GroupName = ResourceDefinitions.TabNames.MetaData,
            Order = 2010)]
        [Required]
        public virtual string Keywords { get; set; }

        [Display(
            Name = "Meta Description",
            GroupName = ResourceDefinitions.TabNames.MetaData,
            Order = 2020)]
        [Required]
        public virtual XhtmlString Description { get; set; }


        [Display(
            Name = "Author",
            GroupName = ResourceDefinitions.TabNames.MetaData,
            Order = 2030)]
        [Required]
        public virtual string Author { get; set; }

        #endregion
 
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
        }
    }
}
