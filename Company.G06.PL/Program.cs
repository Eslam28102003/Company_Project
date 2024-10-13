using Company.G06.BLL;
using Company.G06.BLL.Interfaces;
using Company.G06.BLL.Repositories;
using Company.G06.DAL.Data.Contexts;
using Company.G06.DAL.Models;
using Company.G06.PL.Mapping.Employees;
using Company.G06.PL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.G06.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));




            }); // Allow DI For AppDbContext


            //builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>(); // Allow DI for DepartmentRepository
            //builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>(); // Allow DI for EmployeeRepositoryk
            builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
            });
            // LifeTime
            //builder.Services.AddScoped();     // LifeTime Per Request
            //builder.Services.AddTransient();  // LifeTime Per Operations , Object UnReachable
            //builder.Services.AddSingleton();  // LifeTime Per Application


            builder.Services.AddScoped<IScopedService, ScopedService>();
            builder.Services.AddTransient<ITransientService, TransientService>();
            builder.Services.AddSingleton<ISingletonService, SingletonService>();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
