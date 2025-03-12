using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G00.DAL.Models;

namespace Company.G00.BLL.Interfaces
{
    internal interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();

        Employee? Get(int id);

        int Add (Employee employee);
        int Update (Employee employee);
        int Delete (Employee employee);

    }
}
