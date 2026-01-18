using Microsoft.EntityFrameworkCore;
using SGHR.IOC.Builders;
using SGHR.Persistence.Context;
using SGHR.Web.Data;
using SGHR.Web.Dependences;
using SGHR.Web.Services.ClienteAPIService;

namespace SGHR.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SGHRContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SghrConnString")));

            // Add services to the container.
            builder.Services.AddDependeces();
            builder.Services.AddDependences();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<HttpSesion>();

            var apiBaseUrl = builder.Configuration["ApiSettings:SGHRAPI"];


            builder.Services.AddHttpClient("SGHRAPI", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller}/{action}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=AuthenticationAPI}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
