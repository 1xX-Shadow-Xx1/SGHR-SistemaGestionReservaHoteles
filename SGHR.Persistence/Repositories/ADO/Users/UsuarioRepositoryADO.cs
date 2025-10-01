using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Repository;
using SGHR.Domain.Validators.Users;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Repositories.EF.Users;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace SGHR.Persistence.Repositories.ADO.Users
{
    public sealed class UsuarioRepositoryADO : IUsuarioRepository
    {
        private readonly ILogger<UsuarioRepositoryADO> _logger;
        private readonly IConfiguration _configuration;

        public UsuarioRepositoryADO(ILogger<UsuarioRepositoryADO> logger,
                                 IConfiguration configuration)  
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Task<OperationResult<Usuario>> Delete(Usuario entity)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public Task<OperationResult<bool>> ExistsAsync(Expression<Func<Usuario, bool>> filter)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public Task<OperationResult<List<Usuario>>> GetAll()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public Task<OperationResult<List<Usuario>>> GetAll(Expression<Func<Usuario, bool>> filter)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<OperationResult<Usuario>> GetByCorreoAsync(string correo)
        {
            try
            {

            }
            catch (Exception ex)
            {
                
            }
        }

        public Task<OperationResult<Usuario>> GetById(int id)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<OperationResult<Usuario>> Save(Usuario entity)
        {
            var result = UsuarioValidator.Validate(entity);
            if (!result.Success) return result;

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration["SghrConnString"]))
                {
                    using (SqlCommand command = new SqlCommand("usp_Usuario_Guardar", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@nombre", entity.Nombre);
                        command.Parameters.AddWithValue("@correo", entity.Correo);
                        command.Parameters.AddWithValue("@contraseña", entity.Contrasena);
                        command.Parameters.AddWithValue("@rol", entity.Rol);
                        command.Parameters.AddWithValue("@estado", entity.Estado);

                        SqlParameter p_result = new SqlParameter("@Preresult", System.Data.SqlDbType.VarChar)
                        {
                            Size = 1000,
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_result);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        result.Success = true;
                        result.Message = p_result.Value?.ToString() ?? "Usuario guardado correctamente";
                        result.Data = entity;

                    }
                }
            }
            catch (Exception ex)
            {
                return OperationResult<Usuario>.Fail($"Error al crear: {ex.Message}");
            }
            return result;
        }

        public Task<OperationResult<Usuario>> Update(Usuario entity)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
    }
}
