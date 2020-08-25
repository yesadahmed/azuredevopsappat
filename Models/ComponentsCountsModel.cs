using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.Models
{
    public class ComponentsCountsModel
    {
        public List<ComponentsCountsValue> value { get; set; }
    }

    public class ComponentsCountsValue
    {
        public string Item { get; set; }
        public int Count { get; set; }
    }
}
