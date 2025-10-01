using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reportes;
using SGHR.Persistence.Interfaces.Reportes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.ADO
{
    public class ReporteRepositoryADO: IReporteRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReporteRepositoryADO> _logger;
        private readonly string _connectionString;

        public ReporteRepositoryADO(IConfiguration configuration,
                                    ILogger<ReporteRepositoryADO> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("SghrConnString");
        }


        public async Task<OperationResult<Reporte>> Save(Reporte entity)
        {
            var result = new OperationResult<Reporte>();
            if (entity == null)
                return OperationResult<Reporte>.Fail("El reporte no puede ser nulo");

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("usp_Reporte_Save", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@id_reporte", entity.Id);
                cmd.Parameters.AddWithValue("@tipo_reporte", entity.TipoReporte);
                cmd.Parameters.AddWithValue("@fecha_generacion", entity.FechaGeneracion);
                cmd.Parameters.AddWithValue("@generado_por", entity.GeneradoPor);
                cmd.Parameters.AddWithValue("@ruta_archivo", entity.RutaArchivo ?? (object)DBNull.Value);

                SqlParameter p_result = new SqlParameter("@Preresult", SqlDbType.VarChar, 1000)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(p_result);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                result.Success = true;
                result.Message = p_result.Value?.ToString() ?? "Reporte guardado correctamente";
                result.Data = entity;

                _logger.LogInformation("Reporte registrado: {IdReporte}", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar reporte");
                return OperationResult<Reporte>.Fail($"Error al guardar reporte: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<Reporte>> Update(Reporte entity)
        {
            var result = new OperationResult<Reporte>();
            if (entity == null)
                return OperationResult<Reporte>.Fail("El reporte no puede ser nulo");

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("usp_Reporte_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@id_reporte", entity.Id);
                cmd.Parameters.AddWithValue("@tipo_reporte", entity.TipoReporte);
                cmd.Parameters.AddWithValue("@fecha_generacion", entity.FechaGeneracion);
                cmd.Parameters.AddWithValue("@generado_por", entity.GeneradoPor);
                cmd.Parameters.AddWithValue("@ruta_archivo", entity.RutaArchivo ?? (object)DBNull.Value);

                SqlParameter p_result = new SqlParameter("@Preresult", SqlDbType.VarChar, 1000)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(p_result);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                result.Success = true;
                result.Message = p_result.Value?.ToString() ?? "Reporte actualizado correctamente";
                result.Data = entity;

                _logger.LogInformation("Reporte actualizado: {IdReporte}", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar reporte");
                return OperationResult<Reporte>.Fail($"Error al actualizar reporte: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<Reporte>> Delete(Reporte entity)
        {
            var result = new OperationResult<Reporte>();
            if (entity == null)
                return OperationResult<Reporte>.Fail("El reporte no puede ser nulo");

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("usp_Reporte_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@id_reporte", entity.Id);

                SqlParameter p_result = new SqlParameter("@Preresult", SqlDbType.VarChar, 1000)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(p_result);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                entity.IsDeleted = true;
                result.Success = true;
                result.Message = p_result.Value?.ToString() ?? "Reporte eliminado correctamente";
                result.Data = entity;

                _logger.LogInformation("Reporte eliminado: {IdReporte}", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar reporte");
                return OperationResult<Reporte>.Fail($"Error al eliminar reporte: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<Reporte>> GetById(int id)
        {
            var result = new OperationResult<Reporte>();

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Reportes WHERE id_reporte = @id AND is_deleted = 0", conn);
                cmd.Parameters.AddWithValue("@id", id);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var reporte = new Reporte
                    {
                        Id = Convert.ToInt32(reader["id_reporte"]),
                        TipoReporte = reader["tipo_reporte"].ToString(),
                        FechaGeneracion = Convert.ToDateTime(reader["fecha_generacion"]),
                        GeneradoPor = Convert.ToInt32(reader["generado_por"]),
                        RutaArchivo = reader["ruta_archivo"]?.ToString(),
                        IsDeleted = Convert.ToBoolean(reader["is_deleted"])
                    };

                    result.Success = true;
                    result.Data = reporte;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Reporte no encontrado";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reporte por id");
                result = OperationResult<Reporte>.Fail($"Error: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<List<Reporte>>> GetAll()
        {
            var result = new OperationResult<List<Reporte>>();

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Reportes WHERE is_deleted = 0", conn);

                await conn.OpenAsync();
                var lista = new List<Reporte>();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    lista.Add(new Reporte
                    {
                        Id = Convert.ToInt32(reader["id_reporte"]),
                        TipoReporte = reader["tipo_reporte"].ToString(),
                        FechaGeneracion = Convert.ToDateTime(reader["fecha_generacion"]),
                        GeneradoPor = Convert.ToInt32(reader["generado_por"]),
                        RutaArchivo = reader["ruta_archivo"]?.ToString(),
                        IsDeleted = Convert.ToBoolean(reader["is_deleted"])
                    });
                }

                result.Success = true;
                result.Data = lista;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reportes");
                result = OperationResult<List<Reporte>>.Fail($"Error: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<List<Reporte>>> GetAll(Expression<Func<Reporte, bool>> filter)
        {
            var result = new OperationResult<List<Reporte>>();

            try
            {
                var allResult = await GetAll();
                if (!allResult.Success)
                    return OperationResult<List<Reporte>>.Fail(allResult.Message);

                result.Data = allResult.Data.AsQueryable().Where(filter).ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reportes filtrados");
                result = OperationResult<List<Reporte>>.Fail($"Error: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<bool>> ExistsAsync(Expression<Func<Reporte, bool>> filter)
        {
            var result = new OperationResult<bool>();

            try
            {
                var allResult = await GetAll();
                if (!allResult.Success)
                    return OperationResult<bool>.Fail(allResult.Message);

                result.Data = allResult.Data.AsQueryable().Any(filter);
                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia de reporte");
                result = OperationResult<bool>.Fail($"Error: {ex.Message}");
            }

            return result;
        }
    }
}
