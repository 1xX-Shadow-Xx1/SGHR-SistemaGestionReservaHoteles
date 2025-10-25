using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
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
    /*public class PagoRepositoryADO : IPagoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PagoRepositoryADO> _logger;
        private readonly string _connectionString;

        public PagoRepositoryADO(IConfiguration configuration,
                                 ILogger<PagoRepositoryADO> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("SghrConnString");
        }


        public async Task<OperationResult<Pago>> Save(Pago entity)
        {
            var result = PagoValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("usp_Pago_Save", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@id_reserva", entity.IdReserva);
                cmd.Parameters.AddWithValue("@monto", entity.Monto);
                cmd.Parameters.AddWithValue("@metodo_pago", entity.MetodoPago);
                cmd.Parameters.AddWithValue("@fecha_pago", entity.FechaPago);

                SqlParameter p_result = new SqlParameter("@Preresult", SqlDbType.VarChar, 1000)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(p_result);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                result.Success = true;
                result.Message = p_result.Value?.ToString() ?? "Pago guardado correctamente";
                result.Data = entity;

                _logger.LogInformation("Pago registrado: {IdPago}", entity.ID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar pago");
                return OperationResult<Pago>.Fail($"Error al guardar pago: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<Pago>> Update(Pago entity)
        {
            var result = PagoValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("usp_Pago_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@id_pago", entity.ID);
                cmd.Parameters.AddWithValue("@id_reserva", entity.IdReserva);
                cmd.Parameters.AddWithValue("@monto", entity.Monto);
                cmd.Parameters.AddWithValue("@metodo_pago", entity.MetodoPago);
                cmd.Parameters.AddWithValue("@fecha_pago", entity.FechaPago);

                SqlParameter p_result = new SqlParameter("@Preresult", SqlDbType.VarChar, 1000)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(p_result);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                result.Success = true;
                result.Message = p_result.Value?.ToString() ?? "Pago actualizado correctamente";
                result.Data = entity;

                _logger.LogInformation("Pago actualizado: {IdPago}", entity.ID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar pago");
                return OperationResult<Pago>.Fail($"Error al actualizar pago: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<Pago>> Delete(Pago entity)
        {
            var result = PagoValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("usp_Pago_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@id_pago", entity.ID);

                SqlParameter p_result = new SqlParameter("@Preresult", SqlDbType.VarChar, 1000)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(p_result);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                entity.is_deleted = true;
                result.Success = true;
                result.Message = p_result.Value?.ToString() ?? "Pago eliminado correctamente";
                result.Data = entity;

                _logger.LogInformation("Pago eliminado: {IdPago}", entity.ID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar pago");
                return OperationResult<Pago>.Fail($"Error al eliminar pago: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<Pago>> GetById(int id)
        {
            var result = new OperationResult<Pago>();

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("SELECT * FROM Pagos WHERE id_pago = @id AND is_deleted = 0", conn);
                cmd.Parameters.AddWithValue("@id", id);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var pago = new Pago
                    {
                        ID = Convert.ToInt32(reader["id_pago"]),
                        IdReserva = Convert.ToInt32(reader["id_reserva"]),
                        Monto = Convert.ToDecimal(reader["monto"]),
                        MetodoPago = reader["metodo_pago"].ToString(),
                        FechaPago = Convert.ToDateTime(reader["fecha_pago"]),
                        is_deleted = Convert.ToBoolean(reader["is_deleted"])
                    };

                    result.Success = true;
                    result.Data = pago;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Pago no encontrado";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pago por id");
                result = OperationResult<Pago>.Fail($"Error: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<List<Pago>>> GetAll()
        {
            var result = new OperationResult<List<Pago>>();

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("SELECT * FROM Pagos WHERE is_deleted = 0", conn);

                await conn.OpenAsync();
                var lista = new List<Pago>();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    lista.Add(new Pago
                    {
                        ID = Convert.ToInt32(reader["id_pago"]),
                        IdReserva = Convert.ToInt32(reader["id_reserva"]),
                        Monto = Convert.ToDecimal(reader["monto"]),
                        MetodoPago = reader["metodo_pago"].ToString(),
                        FechaPago = Convert.ToDateTime(reader["fecha_pago"]),
                        is_deleted = Convert.ToBoolean(reader["is_deleted"])
                    });
                }

                result.Success = true;
                result.Data = lista;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pagos");
                result = OperationResult<List<Pago>>.Fail($"Error: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<List<Pago>>> GetAllBY(Expression<Func<Pago, bool>> filter)
        {
            var result = new OperationResult<List<Pago>>();

            try
            {
                var allResult = await GetAll();
                if (!allResult.Success)
                    return OperationResult<List<Pago>>.Fail(allResult.Message);

                result.Data = allResult.Data.AsQueryable().Where(filter).ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pagos filtrados");
                result = OperationResult<List<Pago>>.Fail($"Error: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<bool>> ExistsAsync(Expression<Func<Pago, bool>> filter)
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
                _logger.LogError(ex, "Error al verificar existencia de pago");
                result = OperationResult<bool>.Fail($"Error: {ex.Message}");
            }

            return result;
        }
    }*/
}
