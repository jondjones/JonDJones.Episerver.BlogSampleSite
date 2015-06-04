using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonDJones.com.Core.Entities
{
    public class BlogSummary
    {
        public string Author { get; set; }

        public string Created { get; set; }

        public  string Name { get; set; }

        public  string Url { get; set; }

        public XhtmlString Summary { get; set; }

        public string ImageUrl { get; set; }
    }
}
