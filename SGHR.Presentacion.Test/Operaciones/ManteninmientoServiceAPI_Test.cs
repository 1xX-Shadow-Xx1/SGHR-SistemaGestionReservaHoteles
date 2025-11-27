using Moq;
using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Mantenimiento;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.ServiceAPI.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Presentacion.Test.Operaciones
{
    public class MantenimientoServiceAPI_Tests
    {
        private readonly Mock<IMantenimientoRepositoryMemory> _memoryMock;
        private readonly Mock<IClientAPI<MantenimientoModel>> _clientApiMock;
        private readonly MantenimientoServiceAPI _service;

        public MantenimientoServiceAPI_Tests()
        {
            _memoryMock = new Mock<IMantenimientoRepositoryMemory>();
            _clientApiMock = new Mock<IClientAPI<MantenimientoModel>>();

            _service = new MantenimientoServiceAPI(
                _memoryMock.Object,
                _clientApiMock.Object
            );
        }

        // -----------------------------------------------------
        // 🔹 Memory: GetByIDServices
        // -----------------------------------------------------
        [Fact]
        public void GetByIDServices_ReturnsValueFromMemory()
        {
            var expected = new ServicesResultModel { Success = true };

            _memoryMock.Setup(m => m.GetByIDModel(1)).Returns(expected);

            var result = _service.GetByIDServices(1);

            Assert.Equal(expected, result);
            _memoryMock.Verify(m => m.GetByIDModel(1), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 Memory: GetServices
        // -----------------------------------------------------
        [Fact]
        public void GetServices_ReturnsListFromMemory()
        {
            var list = new List<MantenimientoModel>
            {
                new MantenimientoModel { Id = 1 }
            };

            _memoryMock.Setup(m => m.GetModels()).Returns(list);

            var result = _service.GetServices();

            Assert.Equal(list, result);
            _memoryMock.Verify(m => m.GetModels(), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 API: RemoveServicesPut
        // -----------------------------------------------------
        [Fact]
        public async Task RemoveServicesPut_CallsApiWithCorrectUrl()
        {
            var expected = new ServicesResultModel { Success = true };

            _clientApiMock
                .Setup(api => api.DeleteAsync("Mantenimiento/Remove-Mantenimiento?id=5"))
                .ReturnsAsync(expected);

            var result = await _service.RemoveServicesPut(5);

            Assert.Equal(expected, result);
            _clientApiMock.Verify(api => api.DeleteAsync("Mantenimiento/Remove-Mantenimiento?id=5"), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 API: SaveServicesPost
        // -----------------------------------------------------
        [Fact]
        public async Task SaveServicesPost_CallsApiWithModel()
        {
            var input = new CreateMantenimientoModel
            {
                Descripcion = "Cambio de aire acondicionado"
            };

            var expected = new ServicesResultModel { Success = true };

            _clientApiMock
                .Setup(api => api.PostAsync("Mantenimiento/Create-Mantenimiento", input))
                .ReturnsAsync(expected);

            var result = await _service.SaveServicesPost(input);

            Assert.Equal(expected, result);

            _clientApiMock.Verify(api => api.PostAsync("Mantenimiento/Create-Mantenimiento", input), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 API: UpdateServicesPut
        // -----------------------------------------------------
        [Fact]
        public async Task UpdateServicesPut_CallsApiWithModel()
        {
            var input = new UpdateMantenimientoModel
            {
                Id = 3,
                Descripcion = "Actualización del sistema eléctrico"
            };

            var expected = new ServicesResultModel { Success = true };

            _clientApiMock
                .Setup(api => api.PutAsync("Mantenimiento/Update-Mantenimiento", input))
                .ReturnsAsync(expected);

            var result = await _service.UpdateServicesPut(input);

            Assert.Equal(expected, result);

            _clientApiMock.Verify(api => api.PutAsync("Mantenimiento/Update-Mantenimiento", input), Times.Once);
        }
    }
}
