using Moq;
using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Cliente;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.ServiceAPI.Usuarios;

namespace SGHR.Presentacion.Test.Usuarios
{
    public class ClienteServiceAPI_Tests
    {
        private readonly Mock<IClientAPI<ClienteModel>> _clientMock;
        private readonly Mock<IClienteRepositoryMemory> _memoryMock;
        private readonly ClienteServiceAPI _service;

        public ClienteServiceAPI_Tests()
        {
            _clientMock = new Mock<IClientAPI<ClienteModel>>();
            _memoryMock = new Mock<IClienteRepositoryMemory>();

            _service = new ClienteServiceAPI(_clientMock.Object, _memoryMock.Object);
        }

        // -----------------------------------------------------------
        // 1. GET BY ID (usa memoria)
        // -----------------------------------------------------------
        [Fact]
        public void GetByIDServices_ReturnsModelFromMemory()
        {
            // Arrange
            var expected = ServicesResultModel.Ok(200, "OK");
            _memoryMock.Setup(x => x.GetByIDModel(7)).Returns(expected);

            // Act
            var result = _service.GetByIDServices(7);

            // Assert
            Assert.Equal(expected, result);
            _memoryMock.Verify(x => x.GetByIDModel(7), Times.Once);
        }

        // -----------------------------------------------------------
        // 2. GET LISTA (usa memoria)
        // -----------------------------------------------------------
        [Fact]
        public void GetServices_ReturnsListFromMemory()
        {
            // Arrange
            var list = new List<ClienteModel>()
            {
                new ClienteModel { Id = 1, Nombre = "Juan" }
            };

            _memoryMock.Setup(x => x.GetModels()).Returns(list);

            // Act
            var result = _service.GetServices();

            // Assert
            Assert.Equal(list, result);
            _memoryMock.Verify(x => x.GetModels(), Times.Once);
        }

        // -----------------------------------------------------------
        // 3. GET POR CEDULA (cedula válida)
        // -----------------------------------------------------------
        [Fact]
        public void GetByCedulaCliente_ReturnsModelFromMemory()
        {
            // Arrange
            var expected = ServicesResultModel.Ok(200, "Encontrado");
            _memoryMock.Setup(x => x.GetByCedulaModel("00123456789")).Returns(expected);

            // Act
            var result = _service.GetByCedulaCliente("00123456789");

            // Assert
            Assert.Equal(expected, result);
            _memoryMock.Verify(x => x.GetByCedulaModel("00123456789"), Times.Once);
        }

        // -----------------------------------------------------------
        // 4. GET POR CEDULA (cedula vacía → error)
        // -----------------------------------------------------------
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetByCedulaCliente_InvalidCedula_ReturnsFail(string cedula)
        {
            // Act
            var result = _service.GetByCedulaCliente(cedula);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(400, result.Statuscode);
            Assert.Equal("Tiene que introducir una cedula para comenzar a buscar.", result.Message);

            _memoryMock.Verify(x => x.GetByCedulaModel(It.IsAny<string>()), Times.Never);
        }

        // -----------------------------------------------------------
        // 5. REMOVE (DELETE)
        // -----------------------------------------------------------
        [Fact]
        public async Task RemoveServicesPut_CallsDeleteAsync()
        {
            // Arrange
            var expected = ServicesResultModel.Ok(200, "Eliminado");
            _clientMock
                .Setup(x => x.DeleteAsync("Cliente/Remove-Cliente?id=15"))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.RemoveServicesPut(15);

            // Assert
            Assert.Equal(expected, result);
            _clientMock.Verify(x => x.DeleteAsync("Cliente/Remove-Cliente?id=15"), Times.Once);
        }

        // -----------------------------------------------------------
        // 6. SAVE (POST)
        // -----------------------------------------------------------
        [Fact]
        public async Task SaveServicesPost_CallsPostAsync()
        {
            // Arrange
            var model = new CreateClienteModel { Nombre = "Pedro" };
            var expected = ServicesResultModel.Ok(200, "Creado");

            _clientMock
                .Setup(x => x.PostAsync("Cliente/Create-Cliente", model))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.SaveServicesPost(model);

            // Assert
            Assert.Equal(expected, result);
            _clientMock.Verify(x => x.PostAsync("Cliente/Create-Cliente", model), Times.Once);
        }

        // -----------------------------------------------------------
        // 7. UPDATE (PUT)
        // -----------------------------------------------------------
        [Fact]
        public async Task UpdateServicesPut_CallsPutAsync()
        {
            // Arrange
            var model = new UpdateClienteModel { Id = 3, Nombre = "Nuevo Cliente" };
            var expected = ServicesResultModel.Ok(200, "Actualizado");

            _clientMock
                .Setup(x => x.PutAsync("Cliente/Update-Cliente", model))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.UpdateServicesPut(model);

            // Assert
            Assert.Equal(expected, result);
            _clientMock.Verify(x => x.PutAsync("Cliente/Update-Cliente", model), Times.Once);
        }
    }
}
