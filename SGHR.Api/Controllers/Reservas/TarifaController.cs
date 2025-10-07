using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Reservas
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarifaController : ControllerBase
    {
        // GET: api/<TarifaController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TarifaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TarifaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TarifaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TarifaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
