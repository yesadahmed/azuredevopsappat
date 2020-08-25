using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.ObjectPooling
{
    public class DevOpsConnectionPool
    {
        public VssCredentials VssCredentials { get; set; }

        public VssConnection VssConnection { get; set; }

        public string CollUrl { get; set; }
    }
}
