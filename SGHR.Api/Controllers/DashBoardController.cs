using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashboardServices _dashboardServices;

        public DashBoardController(IDashboardServices dashboardServices)
        {
            _dashboardServices = dashboardServices;
        }

        // GET: api/<DashBoardController>
        [HttpGet("GetDataDashBoard")]
        public async Task<IActionResult> GetDashboardData()
        {
            var result = await _dashboardServices.GetDashboardDataAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
