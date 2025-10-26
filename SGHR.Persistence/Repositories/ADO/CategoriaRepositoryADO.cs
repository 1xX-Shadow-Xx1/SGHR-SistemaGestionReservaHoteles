namespace SGHR.Persistence.Repositories.ADO
{
    /*public class CategoriaRepositoryADO : ICategoriaRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoriaRepositoryADO> _logger;
        private readonly string _connectionString;

        public CategoriaRepositoryADO(IConfiguration configuration,
                                      ILogger<CategoriaRepositoryADO> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("SghrConnString");
        }

        public async Task<OperationResult<Categoria>> Save(Categoria entity)
        {
            var result = CategoriaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("usp_Categoria_Save", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nombre", entity.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", entity.Descripcion);

                    SqlParameter p_result = new SqlParameter("@Preresult", SqlDbType.VarChar, 1000)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(p_result);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    result.Success = true;
                    result.Message = p_result.Value?.ToString() ?? "Categoría guardada correctamente";
                    result.Data = entity;

                    _logger.LogInformation("Categoría guardada: {Nombre}", entity.Nombre);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar categoría");
                return OperationResult<Categoria>.Fail($"Error al guardar categoría: {ex.Message}");
            }

            return result;
        }
        public async Task<OperationResult<Categoria>> Update(Categoria entity)
        {
            var result = CategoriaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("usp_Categoria_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_categoria", entity.ID);
                    cmd.Parameters.AddWithValue("@nombre", entity.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", entity.Descripcion);

                    SqlParameter p_result = new SqlParameter("@Preresult", SqlDbType.VarChar, 1000)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(p_result);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    result.Success = true;
                    result.Message = p_result.Value?.ToString() ?? "Categoría actualizada correctamente";
                    result.Data = entity;

                    _logger.LogInformation("Categoría actualizada: {Id}", entity.ID);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar categoría");
                return OperationResult<Categoria>.Fail($"Error al actualizar categoría: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<Categoria>> Delete(Categoria entity)
        {
            var result = CategoriaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("usp_Categoria_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_categoria", entity.ID);

                    SqlParameter p_result = new SqlParameter("@Preresult", SqlDbType.VarChar, 1000)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(p_result);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    entity.is_deleted = true;
                    result.Success = true;
                    result.Message = p_result.Value?.ToString() ?? "Categoría eliminada correctamente";
                    result.Data = entity;

                    _logger.LogInformation("Categoría eliminada: {Id}", entity.ID);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar categoría");
                return OperationResult<Categoria>.Fail($"Error al eliminar categoría: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<Categoria>> GetById(int id)
        {
            var result = new OperationResult<Categoria>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Categorias WHERE id_categoria = @id AND is_deleted = 0", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var categoria = new Categoria
                            {
                                ID = Convert.ToInt32(reader["id_categoria"]),
                                Nombre = reader["nombre"].ToString(),
                                Descripcion = reader["descripcion"]?.ToString(),
                                is_deleted = Convert.ToBoolean(reader["is_deleted"])
                            };
                            result.Success = true;
                            result.Data = categoria;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Categoría no encontrada";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener categoría por id");
                result = OperationResult<Categoria>.Fail($"Error: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<List<Categoria>>> GetAll()
        {
            var result = new OperationResult<List<Categoria>>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Categorias WHERE is_deleted = 0", conn))
                {
                    await conn.OpenAsync();
                    var lista = new List<Categoria>();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new Categoria
                            {
                                ID = Convert.ToInt32(reader["id_categoria"]),
                                Nombre = reader["nombre"].ToString(),
                                Descripcion = reader["descripcion"]?.ToString(),
                                is_deleted = Convert.ToBoolean(reader["is_deleted"])
                            });
                        }
                    }

                    result.Success = true;
                    result.Data = lista;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener categorías");
                result = OperationResult<List<Categoria>>.Fail($"Error: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<List<Categoria>>> GetAllBY(Expression<Func<Categoria, bool>> filter)
        {
            var result = new OperationResult<List<Categoria>>();

            try
            {
                var allResult = await GetAll();
                if (!allResult.Success)
                    return OperationResult<List<Categoria>>.Fail(allResult.Message);

                var filtered = allResult.Data.AsQueryable().Where(filter).ToList();
                result.Success = true;
                result.Data = filtered;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener categorías filtradas");
                result = OperationResult<List<Categoria>>.Fail($"Error: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<bool>> ExistsAsync(Expression<Func<Categoria, bool>> filter)
        {
            var result = new OperationResult<bool>();

            try
            {
                var allResult = await GetAll(); 
                if (!allResult.Success)
                    return OperationResult<bool>.Fail(allResult.Message);

                var exists = allResult.Data.AsQueryable().Any(filter);
                result.Success = true;
                result.Data = exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia de categoría");
                result = OperationResult<bool>.Fail($"Error: {ex.Message}");
            }

            return result;
        }
    }*/
}
