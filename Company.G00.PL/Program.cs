using Company.G00.BLL.Interfaces;
using Company.G00.BLL.Repositories;
using Company.G00.DAL.Data.Contexts;
using Company.G00.PL.mapping;
using Company.G00.PL.Services;
using Microsoft.EntityFrameworkCore;

namespace Company.G00.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // Register Built-In MVC Services

            builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>(); // Alow DI For DepartmentRepository
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // Alow DI For EmployeeRepository

            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                //options.UseSqlServer(builder.Configuration["DefaultConnection"]); // This If There Is No Built In So This Separate To Key And Value Give Him Key Him Me Value
            }); // Before ) => , ServiceLifetime.Singleton); // Alow DI For CompanyDbContext


            // Life Time :

            // 1. builder.Services.AddScoped();      // Create Only One Object Life Time (Per Request) , After Request => UnReachable Object
            // 2. builder.Services.AddTransient();   // Create Obj Per Use Even If It In The Same Request , (Life Time Per Operation)
            // 3. builder.Services.AddSingleton();   // Create Obj Life Time Per  All The App (Per App) , Even The Request Ended And Start Again It The Same Object

            // Repository  => AddScoped();
            // Cash Data   => AddSingleton();
            // Profile     => AddTransient();

            builder.Services.AddScoped<IScopedService, ScopedService>();          // Per Request
            builder.Services.AddTransient<ITransentService,TransentService>();    // Per Operation
            builder.Services.AddSingleton<ISingletonService,SingletonService>();  // Per App

            // Allow CLR To Make Obj For EmploeeProfile (Mapper):
            builder.Services.AddAutoMapper(M=> M.AddProfile(new EmployeeProfile()));  //Must Inherit From Profile 

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

           
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
