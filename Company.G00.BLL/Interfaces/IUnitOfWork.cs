using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G00.BLL.Interfaces
{
    public interface IUnitOfWork  : IAsyncDisposable // Will Be On It All The Repository
    {
         IDepartmentRepository DepartmentRepository { get;}
         IEmployeeRepository EmployeeRepository { get; }

        Task<int> CompleteAsync();

    }
}
