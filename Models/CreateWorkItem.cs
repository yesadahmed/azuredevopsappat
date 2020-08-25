using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.Models
{
    public class CreateWorkItem
    {
        public string projectid { get; set; }
        public string projectname { get; set; }
        public string assignedTo { get; set; }
        public string description { get; set; }
        public string title { get; set; }

        public string bugtest { get; set; }
        public string type { get; set; }
        public int parentWit { get; set; }
        public int WitId { get; set; }//new one
        public int buildId { get; set; }//
        public string crmsolutioname { get; set; }
        public string crmorgurl { get; set; }

    }
}
