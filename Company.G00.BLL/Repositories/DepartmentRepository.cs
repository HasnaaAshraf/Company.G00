using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G00.BLL.Interfaces;
using Company.G00.DAL.Data.Contexts;
using Company.G00.DAL.Models;

namespace Company.G00.BLL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    // We Make DepartmentRepository For More Organized Code 
    {

        private readonly CompanyDbContext _context; // Null , ReadOnly For Only Make Only One Connection 

        public DepartmentRepository()
        {
            _context = new CompanyDbContext(); // Refer To This Instead Of Null 
        }

        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public Department? Get(int id)
        {
            return _context.Departments.Find(id);
        }

        public int Add(Department model)
        {
            _context.Departments.Add(model);
            return _context.SaveChanges();
        }

        public int Update(Department model)
        {
            _context.Departments.Update(model);
            return _context.SaveChanges();
        }

        public int Delete(Department model)
        {
            _context.Departments.Remove(model);
            return _context.SaveChanges();
        }

    }
}
