using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace JonDJones.Com
{
    public class CustomViewEngine : RazorViewEngine
    {
        public CustomViewEngine()
        {
            var viewLocations = new[]
            {
                "~/Views/{1}/{0}.aspx",
                "~/Views/{1}/{0}.ascx",
                "~/Views/Shared/{0}.aspx",
                "~/Views/Shared/{0}.ascx",
                "~/Views/Pages/{1}/{0}.cshtml",
                "~/Views/Blocks/{1}/{0}.cshtml"
            };

            PartialViewLocationFormats = viewLocations;
            ViewLocationFormats = viewLocations;
        }
    } 
}