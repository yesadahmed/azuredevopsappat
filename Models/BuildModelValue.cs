using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.Models
{
    public class BuildModelValue
    {
        public int BuildID { get; set; }
        public string BuildName { get; set; }
        public string BuildUrl { get; set; }
        public string BuildPath { get; set; }
        public DateTime DateCreated { get; set; }

        public string ProjectName { get; set; }
        public string ProjectUrl { get; set; }
        public Guid ProjectId { get; set; }


    }
}
