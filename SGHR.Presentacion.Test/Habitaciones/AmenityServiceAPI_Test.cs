using Moq;
using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Amenity;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.ServiceAPI.Habitaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Presentacion.Test.Habitaciones
{
    public class AmenityServiceAPI_Test
    {
        private readonly Mock<IAmenityRepositoryMemory> _memoryMock;
        private readonly Mock<IClientAPI> _clientAPIMock;
        private readonly AmenityServiceAPI _service;

        public AmenityServiceAPI_Test()
        {
            _memoryMock = new Mock<IAmenityRepositoryMemory>();
            _clientAPIMock = new Mock<IClientAPI>();

            _service = new AmenityServiceAPI(
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
            var expected = new ApiResult<AmenityModel> { Success = true };

            _memoryMock.Setup(m => m.GetByIDModel(10)).Returns(expected);

            var result = _service.GetByIDServices(10);

            Assert.Equal(expected, result);
            _memoryMock.Verify(m => m.GetByIDModel(10), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 Memory: GetServices
        // -----------------------------------------------------
        [Fact]
        public void GetServices_ReturnsMemoryList()
        {
            var list = new List<AmenityModel>
            {
                new AmenityModel { Id = 1, Nombre = "Piscina" }
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
            var expected = new ApiResult<AmenityModel> { Success = true };
            string endpoint = "Amenity/Remove-Amenity?id=5";

            _clientAPIMock.Setup(api => api.DeleteAsync<AmenityModel>(endpoint))
                          .ReturnsAsync(expected);

            var result = await _service.RemoveServicesPut(5);

            Assert.Equal(expected, result);
            _clientAPIMock.Verify(api => api.DeleteAsync<AmenityModel>(endpoint), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 API: SaveServicesPost
        // -----------------------------------------------------
        [Fact]
        public async Task SaveServicesPost_CallsApiWithCorrectModel()
        {
            var model = new CreateAmenityModel
            {
                Nombre = "Jacuzzi"
            };

            var expected = new ApiResult<AmenityModel> { Success = true };
            string endpoint = "Amenity/Create-Amenity";

            _clientAPIMock.Setup(api => api.PostAsJsonAsync<CreateAmenityModel, AmenityModel>(endpoint, model))
                          .ReturnsAsync(expected);

            var result = await _service.SaveServicesPost(model);

            Assert.Equal(expected, result);
            _clientAPIMock.Verify(api => api.PostAsJsonAsync<CreateAmenityModel, AmenityModel>(endpoint, model), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 API: UpdateServicesPut
        // -----------------------------------------------------
        [Fact]
        public async Task UpdateServicesPut_CallsApiWithCorrectModel()
        {
            var model = new UpdateAmenityModel
            {
                Id = 3,
                Nombre = "Gimnasio"
            };

            var expected = new ApiResult<AmenityModel> { Success = true };
            string endpoint = "Amenity/Update-Amenity";

            _clientAPIMock.Setup(api => api.PutAsJsonAsync<UpdateAmenityModel, AmenityModel>(endpoint, model))
                          .ReturnsAsync(expected);

            var result = await _service.UpdateServicesPut(model);

            Assert.Equal(expected, result);
            _clientAPIMock.Verify(api => api.PutAsJsonAsync<UpdateAmenityModel, AmenityModel>(endpoint, model), Times.Once);
        }

    }
}
