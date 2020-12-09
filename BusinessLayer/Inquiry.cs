using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Inquiry
    {
        public Inquiry(string desc, string text)
        {
            this.desc = desc;
            this.text = text;
        }

        public string desc { get; set; }
        public string text { get; set; }
    }
}
