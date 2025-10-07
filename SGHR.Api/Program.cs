
using SGHR.Api.Builders;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Interfaces.Users;
using SGHR.Application.Services.Operaciones;
using SGHR.Application.Services.Users;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Reportes;
using SGHR.Persistence.Repositories.ADO;
using SGHR.Persistence.Repositories.EF.Users;

namespace SGHR.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddApplicationService();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
