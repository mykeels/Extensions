using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Models
{
    public class KeyValue
    {
        public string key { get; set; }
        public dynamic value { get; set; }
        public KeyValue()
        {
            this.key = "";
            this.value = "";
        }

        public KeyValue(string key = null, dynamic value = null)
        {
            if (key != null) this.key = key;
            if (value != null) this.value = value;
        }
    }
}
