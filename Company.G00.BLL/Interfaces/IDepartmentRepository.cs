using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G00.DAL.Models;

namespace Company.G00.BLL.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department> // We Still Save This Repository Bec If I Want Increase SomeThing Special For Department Only 
    {
        //IEnumerable<Department> GetAll();

        //Department? Get(int id);

        //int Add(Department model);
        //int Update(Department model);
        //int Delete(Department model);

    }
}
