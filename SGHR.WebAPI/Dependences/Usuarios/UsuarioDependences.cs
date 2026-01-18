using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Data.Repositories.Usuarios;
using SGHR.Web.Services.Interfaces.Usuarios;
using SGHR.Web.Services.ServiceAPI.Usuarios;
using SGHR.Web.Services.SeviceMonitor.Interface.Usuarios;
using SGHR.Web.Services.SeviceTwoPlane.Usuarios;

namespace SGHR.Web.Dependences.Usuarios
{
    public static class UsuarioDependences
    {
        public static IServiceCollection AddUsuarioDependences(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioMemoryCheck, UsuarioMemoryCheck>();
            services.AddScoped<IUsuarioRepositoryMemory, UsuarioRepositoryMemory>();
            services.AddScoped<IUsuarioServiceAPI, UsuarioServiceAPI>();
            

            return services;
        }
    }
}
