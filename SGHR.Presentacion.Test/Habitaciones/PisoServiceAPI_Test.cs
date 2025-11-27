using Moq;
using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Piso;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.ServiceAPI.Habitaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Presentacion.Test.Habitaciones
{
    public class PisoServiceAPI_Tests
    {
        private readonly Mock<IPisoRepositoryMemory> _memoryMock;
        private readonly Mock<IClientAPI<PisoModel>> _clientApiMock;
        private readonly PisoServiceAPI _service;

        public PisoServiceAPI_Tests()
        {
            _memoryMock = new Mock<IPisoRepositoryMemory>();
            _clientApiMock = new Mock<IClientAPI<PisoModel>>();
            _service = new PisoServiceAPI(_memoryMock.Object, _clientApiMock.Object);
        }

        // ---------------------------------------------------------
        // GET BY ID
        // ---------------------------------------------------------
        [Fact]
        public void GetByIDServices_ShouldReturnResultFromMemory()
        {
            // Arrange
            var expected = ServicesResultModel.Ok(200);
            _memoryMock.Setup(m => m.GetByIDModel(1)).Returns(expected);

            // Act
            var result = _service.GetByIDServices(1);

            // Assert
            Assert.Equal(expected, result);
            _memoryMock.Verify(m => m.GetByIDModel(1), Times.Once);
        }

        // ---------------------------------------------------------
        // GET ALL
        // ---------------------------------------------------------
        [Fact]
        public void GetServices_ShouldReturnListFromMemory()
        {
            // Arrange
            var list = new List<PisoModel> { new PisoModel { Id = 1 } };
            _memoryMock.Setup(m => m.GetModels()).Returns(list);

            // Act
            var result = _service.GetServices();

            // Assert
            Assert.Equal(list, result);
            _memoryMock.Verify(m => m.GetModels(), Times.Once);
        }

        // ---------------------------------------------------------
        // REMOVE (PUT DELETE)
        // ---------------------------------------------------------
        [Fact]
        public async Task RemoveServicesPut_ShouldCallDeleteAsync()
        {
            // Arrange
            var expected = ServicesResultModel.Ok(200);
            _clientApiMock
                .Setup(api => api.DeleteAsync("Piso/Remove-Piso?id=1"))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.RemoveServicesPut(1);

            // Assert
            Assert.Equal(expected, result);
            _clientApiMock.Verify(api => api.DeleteAsync("Piso/Remove-Piso?id=1"), Times.Once);
        }

        // ---------------------------------------------------------
        // SAVE POST
        // ---------------------------------------------------------
        [Fact]
        public async Task SaveServicesPost_ShouldCallPostAsync()
        {
            // Arrange
            var model = new CreatePisoModel();
            var expected = ServicesResultModel.Ok(200);

            _clientApiMock
                .Setup(api => api.PostAsync("Piso/Create-Piso", model))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.SaveServicesPost(model);

            // Assert
            Assert.Equal(expected, result);
            _clientApiMock.Verify(api => api.PostAsync("Piso/Create-Piso", model), Times.Once);
        }

        // ---------------------------------------------------------
        // UPDATE PUT
        // ---------------------------------------------------------
        [Fact]
        public async Task UpdateServicesPut_ShouldCallPutAsync()
        {
            // Arrange
            var model = new UpdatePisoModel();
            var expected = ServicesResultModel.Ok(200);

            _clientApiMock
                .Setup(api => api.PutAsync("Piso/Update-Piso", model))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.UpdateServicesPut(model);

            // Assert
            Assert.Equal(expected, result);
            _clientApiMock.Verify(api => api.PutAsync("Piso/Update-Piso", model), Times.Once);
        }
    }
}
