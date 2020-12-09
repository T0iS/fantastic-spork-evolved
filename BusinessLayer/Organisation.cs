using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Organisation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Person Person_Id { get; set; }
        public Address Address_Id { get; set; }
    }
}
