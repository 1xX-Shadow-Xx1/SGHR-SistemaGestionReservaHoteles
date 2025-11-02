using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Reporte;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Domain.Entities.Configuration.Reportes;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Reportes;

namespace SGHR.Application.Services.Operaciones
{
    public class ReporteServices : IReporteServices
    {
        private readonly ILogger<ReporteServices> _logger;
        private readonly IReporteRepository _reporteRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ReporteServices(ILogger<ReporteServices> logger, 
                               IReporteRepository reporteRepository,
                               IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _reporteRepository = reporteRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateReporteDto CreateDto)
        {
            ServiceResult result = new ServiceResult();
            if (CreateDto == null)
            {
                result.Message = "El reporte no puede ser nulo.";
                return result;
            }
            try
            {
                var Reporte = await _reporteRepository.GetAllAsync();
                if (!Reporte.Success)
                {
                    result.Message = Reporte.Message;
                    return result;
                }
                var Usuarios = await _usuarioRepository.GetAllAsync();
                if (!Usuarios.Success)
                {
                    result.Message = Usuarios.Message;
                    return result;
                }

                var ExistUsuario = Usuarios.Data.FirstOrDefault(u => u.Correo == CreateDto.GeneradoPor);
                if (ExistUsuario == null)
                {
                    result.Message = $"No se encontro el usuario, introduce un correo de usuario ya registrado.";
                    return result;
                }

                Reporte reporte = new Reporte()
                {
                    TipoReporte = CreateDto.TipoReporte,
                    GeneradoPor = ExistUsuario.Id,
                    RutaArchivo = CreateDto.RutaArchivo
                };

                var opResult = await _reporteRepository.SaveAsync(reporte);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                ReporteDto reportedto = new ReporteDto()
                {
                    Id = opResult.Data.Id,
                    TipoReporte = opResult.Data.TipoReporte,
                    GeneradoPor = CreateDto.GeneradoPor,
                    RutaArchivo = opResult.Data.RutaArchivo,
                    FechaGeneracion = opResult.Data.FechaGeneracion
                };

                result.Success = true;
                result.Data = reportedto;
                result.Message = $"Se a registrado el reporte correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al registrar el reporte: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            if (id <= 0)
            {
                result.Message = "El id es invalido.";
                return result;
            }
            try
            {
                var existReporte = await _reporteRepository.GetByIdAsync(id);
                if (!existReporte.Success)
                {
                    result.Message = existReporte.Message;
                    return result;
                }

                var OpResult = await _reporteRepository.DeleteAsync(existReporte.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Reporte con id {existReporte.Data.Id} eliminada correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al eliminar el reporte: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListaReportes = await _reporteRepository.GetAllAsync();
                if (!ListaReportes.Success)
                {
                    result.Message = ListaReportes.Message;
                    return result;
                }
                var ListaUsuarios = await _usuarioRepository.GetAllAsync();
                if (!ListaUsuarios.Success)
                {
                    result.Message = ListaUsuarios.Message;
                    return result;
                }

                var reporteDtos = (
                    from r in ListaReportes.Data
                    join u in ListaUsuarios.Data on r.GeneradoPor equals u.Id into userGroup
                    from u in userGroup.DefaultIfEmpty()
                    select new ReporteDto
                    {
                        Id = r.Id,
                        TipoReporte = r.TipoReporte,
                        FechaGeneracion = r.FechaGeneracion,
                        GeneradoPor = u.Correo,
                        RutaArchivo = r.RutaArchivo
                    }
                ).ToList();

                result.Success = true;
                result.Data = reporteDtos;
                result.Message = "Se obtuvieron los reportes correctamente.";
            }
            catch (Exception ex)
            {
                result.Message = $"Erro al obtener los reportes.";
            }
            return result;
        }
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            if (id <= 0)
            {
                result.Message = "El id es invalido.";
                return result;
            }
            try
            {
                var opResult = await _reporteRepository.GetByIdAsync(id);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                var usuario = await _usuarioRepository.GetByIdAsync(opResult.Data.GeneradoPor);
                if (!usuario.Success)
                {
                    result.Message = usuario.Message;
                    return result;
                }

                ReporteDto reporteDto = new ReporteDto()
                {
                    Id = opResult.Data.Id,
                    TipoReporte = opResult.Data.TipoReporte,
                    GeneradoPor = usuario.Data.Correo,
                    RutaArchivo = opResult.Data.RutaArchivo,
                    FechaGeneracion = opResult.Data.FechaGeneracion
                };

                result.Success = true;
                result.Data = reporteDto;
                result.Message = $"Se obtuvo el reporte con id {id} correctamnete.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener el reporte: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdateReporteDto UpdateDto)
        {
            ServiceResult result = new ServiceResult();
            if (UpdateDto == null)
            {
                result.Message = "El reporte no puede ser nula.";
                return result;
            }
            if (UpdateDto.Id <= 0)
            {
                result.Message = "El id es invalido.";
                return result;
            }
            try
            {
                var reporte = await _reporteRepository.GetByIdAsync(UpdateDto.Id);
                if (!reporte.Success)
                {
                    result.Message = reporte.Message;
                    return result;
                }

                var Usuarios = await _usuarioRepository.GetAllAsync();
                if (!Usuarios.Success)
                {
                    result.Message = Usuarios.Message;
                    return result;
                }               

                var usuario = Usuarios.Data.FirstOrDefault(u => u.Correo == UpdateDto.GeneradoPor);
                if (usuario == null)
                {
                    result.Message = $"No se encontro el correo, ingrese un correo ya registrado.";
                    return result;
                }

                reporte.Data.RutaArchivo = UpdateDto.RutaArchivo;
                reporte.Data.TipoReporte = UpdateDto.TipoReporte;
                reporte.Data.GeneradoPor = usuario.Id;

                var opResult = await _reporteRepository.UpdateAsync(reporte.Data);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                ReporteDto reporteDto = new ReporteDto()
                {
                    Id = opResult.Data.Id,
                    TipoReporte = opResult.Data.TipoReporte,
                    GeneradoPor = usuario.Correo,
                    RutaArchivo = opResult.Data.RutaArchivo,
                    FechaGeneracion = opResult.Data.FechaGeneracion
                };

                result.Success = true;
                result.Data = reporteDto;
                result.Message = $"Se a actualizado el reporte correctamente.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al actualiza el reporte: {ex.Message}";
            }
            return result;
        }
    }
}
