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
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _categoriaService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<CategoriaController>/5
        [HttpGet("Get-Categoria-ByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _categoriaService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<CategoriaController>
        [HttpPost("Create-Categoria")]
        public async Task<IActionResult> Post([FromBody] CreateCategoriaDto categoriaDto)
        {
            ServiceResult result = await _categoriaService.Save(categoriaDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<CategoriaController>/5
        [HttpPut("Update-Categoria")]
        public async Task<IActionResult> Put([FromBody] UpdateCategoriaDto categoriaDto)
        {
            ServiceResult result = await _categoriaService.Update(categoriaDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<CategoriaController>/5
        [HttpDelete("Delete-Categoria")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResult result = await _categoriaService.Remove(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
