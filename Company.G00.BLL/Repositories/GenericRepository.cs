using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G00.BLL.Interfaces;
using Company.G00.DAL.Data.Contexts;
using Company.G00.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.G00.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseClass
    {

        private readonly CompanyDbContext _context;

        public GenericRepository(CompanyDbContext context)
        {
            _context = context;
        }

        // Asynchronous , synchronous :
        // ToListAsync (Work By Async)
        
        // Return Type (Async):
         // void
         // Emplty Task As Void
         // Task <> <- But in it Return Type 

        // Must But await With Async (To Tell Him To Wait )

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E => E.Department).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _context.Employees.Include(E => E.Department).FirstOrDefaultAsync(E => E.Id == id) as T;
            }
            return _context.Set<T>().Find(id);
        }


        public async Task AddAsync(T model)
        {
           await _context.Set<T>().AddAsync(model);
            //return _context.SaveChanges();
        }

        public void Update(T model)
        {
            _context.Set<T>().Update(model);
            //return _context.SaveChanges();
        }

        public void Delete(T model)
        {
            _context.Set<T>().Remove(model);
            //return _context.SaveChanges();
        }


    }
}
