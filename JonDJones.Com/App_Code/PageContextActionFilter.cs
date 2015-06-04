using EPiServer.ServiceLocation;
using JonDJones.com.Core.Helpers;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Interfaces;
using JonDJones.Com.Core.Pages;
using JonDJones.Com.Core.Pages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JonDJones.Com.Initialization
{
    public class PageContextActionFilter : IResultFilter
    {
        internal Injected<IEpiServerDependencies> EpiServerDependencies { get; set; }

        internal Injected<IBlockHelper> BlockHelper { get; set; } 

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var model = filterContext.Controller.ViewData.Model;

            var layoutModel = model as IPageViewModel<GlobalBasePage>;
            if (layoutModel != null)
            {
                var viewModel = new ViewModelFactory(BlockHelper.Service, EpiServerDependencies.Service);
                var layout = viewModel.CreateLayoutViewModel(layoutModel.CurrentPage);
                layoutModel.Layout = layout;
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}