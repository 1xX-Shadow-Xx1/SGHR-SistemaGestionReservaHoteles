using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Reporte;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Domain.Entities.Configuration.Reportes;
using SGHR.Persistence.Interfaces.Reportes;

namespace SGHR.Application.Services.Operaciones
{
    public class ReporteService : IReporteService
    {
        public readonly ILogger<ReporteService> _logger;
        public readonly IReporteRepository _reporteRepository;
        public ReporteService(ILogger<ReporteService> logger, IReporteRepository reporteRepository)
        {
            _logger = logger;
            _reporteRepository = reporteRepository;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result= new ServiceResult();  
            _logger.LogInformation("Iniciando obtención de todos los reportes.");

            try
            {
                var opResult = await _reporteRepository.GetAllAsync();
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtubieron todos los reportes correctamente.");
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Reportes obtenidos correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener los reportes.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los reportes.");
                result.Success = false;
                result.Message = "Error al obtener los reportes.";
            }
            return result;
        }
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de reporte por ID: {Id}", id);

            try
            {
                var opResult = await _reporteRepository.GetByIdAsync(id);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvo una reporte con Id {id}.",id);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Reporte obtenido correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el reporte.");
                result.Success = false;
                result.Message = "Error al obtener el reporte.";
            }
            return result;
        }
        public async Task<ServiceResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de reporte con ID: {Id}", id);

            try
            {
                if (id < 0)
                {
                    result.Success = false;
                    result.Message = "El ID del reporte no es válido.";
                    return result;
                }

                var reportExists = await _reporteRepository.GetByIdAsync(id);
                if (!reportExists.Success)
                {
                    result.Success = false;
                    result.Message = reportExists.Message;
                    return result;
                }

                var opResult = await _reporteRepository.DeleteAsync(reportExists.Data, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation($"Se elimino el reporte con id {id} correctamente.");
                    result.Success = true;
                    result.Message = "Reporte eliminado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar el reporte.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el reporte.");
                result.Success = false;
                result.Message = "Error al eliminar el reporte.";
            }
            return result;
        }
        public async Task<ServiceResult> CreateAsync(CreateReporteDto createReporteDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de nuevo reporte.", createReporteDto);

            try
            {
                Reporte reporte = new Reporte
                {
                    TipoReporte = createReporteDto.TipoReporte,
                    GeneradoPor = createReporteDto.GeneradoPor,
                    RutaArchivo = createReporteDto.RutaArchivo
                };

                var opResult = await _reporteRepository.SaveAsync(reporte, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a registrado un reporte en la ruta {ruta}.", createReporteDto.RutaArchivo);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Reporte creado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el reporte.");
                result.Success = false;
                result.Message = "Error al crear el reporte.";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdateReporteDto updateReporteDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de reporte con ID: {Id}", updateReporteDto.Id);

            try
            {
                var reportExists = await _reporteRepository.GetByIdAsync(updateReporteDto.Id);
                if (!reportExists.Success || reportExists.Data == null)
                {
                    result.Success = false;
                    result.Message = reportExists.Message;
                    return result;
                }

                Reporte reporteToUpdate = reportExists.Data;
                reporteToUpdate.TipoReporte = updateReporteDto.TipoReporte;
                reporteToUpdate.FechaGeneracion = updateReporteDto.FechaGeneracion;
                reporteToUpdate.GeneradoPor = updateReporteDto.GeneradoPor;
                reporteToUpdate.RutaArchivo = updateReporteDto.RutaArchivo;

                var opResult = await _reporteRepository.UpdateAsync(reporteToUpdate, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a actualizado un reporte de tipo {tipo}.", reporteToUpdate.TipoReporte);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Reporte actualizado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar el reporte.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el reporte.");
                result.Success = false;
                result.Message = "Error al actualizar el reporte.";
            }
            return result;
        }
    }
}
