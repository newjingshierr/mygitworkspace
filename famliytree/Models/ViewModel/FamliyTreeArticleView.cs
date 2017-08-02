﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
  public  class FamliyTreeArticleView
    {

        public long TenantID { get; set; }
        public long ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public long CreatedBy { get; set; }

        public DateTime Modified { get; set; }

        public long ModifiedBy { get; set; }

        public string Cover { get; set; }
    }

    public class FTMessage
    {
        public long ID { get; set; }
        public string title { get; set; }
        public long category { get; set; }
        public string path { get; set; }
    }
}