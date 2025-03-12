using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Company.G00.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.G00.DAL.Data.Contexts
{
    public class CompanyDbContext : DbContext 
    {

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
                
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = . ; Database = CompanyG00 ; Trusted_Connection = True ; TrustServerCertificate = True ");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());

            base.OnModelCreating(modelBuilder); 
        }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }


    }
}
