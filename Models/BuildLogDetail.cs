using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.Models
{
    public class BuildLogDetail
    {
        public string Url { get; set; }

        public int Id { get; set; }
        public DateTime? CreatedOn { get; set; }

        public bool IsError { get; set; }

        public string HtmlContent { get; set; }

        public string TaskName { get; set; }

        public string TaskColor { get; set; }
    }

    public class ResultModel
    {
        public string TaskColor { get; set; } //red or Green

        public bool IsError { get; set; }

        public string HtmContent { get; set; }

        public string TaskName { get; set; }
    }
}
