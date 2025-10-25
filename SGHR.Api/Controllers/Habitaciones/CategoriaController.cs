using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Categoria;
using SGHR.Application.Interfaces.Habitaciones;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Habitaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET: api/<CategoriaController>
        [HttpGet("Get-Categorias")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _categoriaService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<CategoriaController>/5
        [HttpGet("Get-Categoria-ByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _categoriaService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<CategoriaController>
        [HttpPost("Create-Categoria")]
        public async Task<IActionResult> PostAsync([FromBody] CreateCategoriaDto categoriaDto, int? idsesion = null)
        {
            ServiceResult result = await _categoriaService.CreateAsync(categoriaDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<CategoriaController>/5
        [HttpPut("Update-Categoria")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateCategoriaDto categoriaDto, int? idsesion = null)
        {
            ServiceResult result = await _categoriaService.UpdateAsync(categoriaDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<CategoriaController>/5
        [HttpDelete("Delete-Categoria")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = await _categoriaService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
