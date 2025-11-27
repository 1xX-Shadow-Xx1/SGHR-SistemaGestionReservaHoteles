using Moq;
using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Categoria;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.ServiceAPI.Habitaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Presentacion.Test.Habitaciones
{
    public class CategoriaServiceAPI_Test
    {
        private readonly Mock<ICategoriaRepositoryMemory> _memoryMock;
        private readonly Mock<IClientAPI<CategoriaModel>> _clientAPIMock;
        private readonly CategoriaServiceAPI _service;

        public CategoriaServiceAPI_Test()
        {
            _memoryMock = new Mock<ICategoriaRepositoryMemory>();
            _clientAPIMock = new Mock<IClientAPI<CategoriaModel>>();

            _service = new CategoriaServiceAPI(
                _memoryMock.Object,
                _clientAPIMock.Object
            );
        }

        // -----------------------------------------------------
        // 🔹 Memory: GetByIDServices
        // -----------------------------------------------------
        [Fact]
        public void GetByIDServices_ReturnsCorrectModel()
        {
            var expected = new ServicesResultModel { Success = true };

            _memoryMock.Setup(m => m.GetByIDModel(2)).Returns(expected);

            var result = _service.GetByIDServices(2);

            Assert.Equal(expected, result);
            _memoryMock.Verify(m => m.GetByIDModel(2), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 Memory: GetServices
        // -----------------------------------------------------
        [Fact]
        public void GetServices_ReturnsMemoryList()
        {
            var list = new List<CategoriaModel>
            {
                new CategoriaModel { Id = 1, Nombre = "Suite" }
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
        public async Task RemoveServicesPut_CallsCorrectApiEndpoint()
        {
            var expected = new ServicesResultModel { Success = true };

            string endpoint = "Categoria/Remove-Categoria?id=3";

            _clientAPIMock.Setup(api => api.DeleteAsync(endpoint))
                          .ReturnsAsync(expected);

            var result = await _service.RemoveServicesPut(3);

            Assert.Equal(expected, result);
            _clientAPIMock.Verify(api => api.DeleteAsync(endpoint), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 API: SaveServicesPost
        // -----------------------------------------------------
        [Fact]
        public async Task SaveServicesPost_CallsApiWithCorrectModel()
        {
            var model = new CreateCategoriaModel
            {
                Nombre = "Premium"
            };

            var expected = new ServicesResultModel { Success = true };

            string endpoint = "Categoria/Create-Categoria";

            _clientAPIMock.Setup(api => api.PostAsync(endpoint, model))
                          .ReturnsAsync(expected);

            var result = await _service.SaveServicesPost(model);

            Assert.Equal(expected, result);
            _clientAPIMock.Verify(api => api.PostAsync(endpoint, model), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 API: UpdateServicesPut
        // -----------------------------------------------------
        [Fact]
        public async Task UpdateServicesPut_CallsApiWithCorrectModel()
        {
            var model = new UpdateCategoriaModel
            {
                Id = 4,
                Nombre = "Deluxe"
            };

            var expected = new ServicesResultModel { Success = true };

            string endpoint = "Categoria/Update-Categoria";

            _clientAPIMock.Setup(api => api.PutAsync(endpoint, model))
                          .ReturnsAsync(expected);

            var result = await _service.UpdateServicesPut(model);

            Assert.Equal(expected, result);
            _clientAPIMock.Verify(api => api.PutAsync(endpoint, model), Times.Once);
        }
    }

}
