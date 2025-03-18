using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G00.DAL.Models
{
    public class Department : BaseClass
    {
     
        public string Code { get; set; }

        public string Name { get; set; }

        public List<Employee> Employees { get; set; }

    }
}
