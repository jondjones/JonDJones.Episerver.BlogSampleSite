using JonDJones.com.Core.Blocks;
using JonDJones.com.Core.ViewModel.Blocks;
using JonDJones.Com.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JonDJones.Com.Controllers.Blocks
{
    public class SideWidgetController : BaseBlockController<SideWidget>
    {
        public override ActionResult Index(SideWidget currentBlock)
        {
            return PartialView("Index", new SideWidgetViewModel(currentBlock, EpiServerDependencies));
        }
    }
}
