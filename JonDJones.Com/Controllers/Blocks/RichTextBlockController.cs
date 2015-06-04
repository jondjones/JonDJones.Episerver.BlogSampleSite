
using EPiServer.Framework.DataAnnotations;
using JonDJones.com.Core.Blocks;
using JonDJones.com.Core.Resources;
using JonDJones.com.Core.ViewModel.Blocks;
using JonDJones.Com.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JonDJones.Com.Controllers.Blocks
{
    public class RichTextBlockController : BaseBlockController<RichTextBlock>
    {
        public override ActionResult Index(RichTextBlock currentBlock)
        {
            return PartialView("Index", new RichTextBlockViewModel(currentBlock, EpiServerDependencies));
        }
    }
}