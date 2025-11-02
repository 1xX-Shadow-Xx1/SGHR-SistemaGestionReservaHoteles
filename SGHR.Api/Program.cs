
using Microsoft.EntityFrameworkCore;
using SGHR.IOC.Builders;
using SGHR.Persistence.Context;


namespace SGHR.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuraccion de SGHRContex
            builder.Services.AddDbContext<SGHRContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SghrConnString")));

            // Add services to the container.
            builder.Services.AddDependeces();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
