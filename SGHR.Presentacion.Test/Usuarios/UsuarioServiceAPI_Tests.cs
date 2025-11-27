using Moq;
using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.ServiceAPI.Usuarios;

namespace SGHR.Presentacion.Test.Usuarios
{
    public class UsuarioServiceAPI_Tests
    {
        private readonly Mock<IClientAPI<UsuarioModel>> _clientMock;
        private readonly Mock<IUsuarioRepositoryMemory> _memoryMock;
        private readonly UsuarioServiceAPI _service;

        public UsuarioServiceAPI_Tests()
        {
            _clientMock = new Mock<IClientAPI<UsuarioModel>>();
            _memoryMock = new Mock<IUsuarioRepositoryMemory>();

            _service = new UsuarioServiceAPI(_clientMock.Object, _memoryMock.Object);
        }

        // -----------------------------------------------------------
        // 1. GET BY ID (usa memoria)
        // -----------------------------------------------------------
        [Fact]
        public void GetByIDServices_ReturnsModelFromMemory()
        {
            // Arrange
            var expected = ServicesResultModel.Ok(200, "OK");
            _memoryMock.Setup(x => x.GetByIDModel(5)).Returns(expected);

            // Act
            var result = _service.GetByIDServices(5);

            // Assert
            Assert.Equal(expected, result);
            _memoryMock.Verify(x => x.GetByIDModel(5), Times.Once);
        }

        // -----------------------------------------------------------
        // 2. GET LIST (usa memoria)
        // -----------------------------------------------------------
        [Fact]
        public void GetServices_ReturnsListFromMemory()
        {
            // Arrange
            var list = new List<UsuarioModel> {
                new UsuarioModel { Id = 1, Nombre = "Kevin" }
            };

            _memoryMock.Setup(x => x.GetModels()).Returns(list);

            // Act
            var result = _service.GetServices();

            // Assert
            Assert.Equal(list, result);
            _memoryMock.Verify(x => x.GetModels(), Times.Once);
        }

        // -----------------------------------------------------------
        // 3. REMOVE (PUT) llama API
        // -----------------------------------------------------------
        [Fact]
        public async Task RemoveServicesPut_CallsClientDeleteAsync()
        {
            // Arrange
            var expected = ServicesResultModel.Ok(200, "Eliminado");

            _clientMock
                .Setup(x => x.DeleteAsync("Usuario/Remove-Usuario?id=10"))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.RemoveServicesPut(10);

            // Assert
            Assert.Equal(expected, result);

            _clientMock.Verify(
                x => x.DeleteAsync("Usuario/Remove-Usuario?id=10"),
                Times.Once
            );
        }

        // -----------------------------------------------------------
        // 4. CREATE (POST)
        // -----------------------------------------------------------
        [Fact]
        public async Task SaveServicesPost_CallsClientPostAsync()
        {
            // Arrange
            var model = new CreateUsuarioModel { Nombre = "Kevin" };
            var expected = ServicesResultModel.Ok(200, "Creado");

            _clientMock
                .Setup(x => x.PostAsync("Usuario/create-Usuario", model))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.SaveServicesPost(model);

            // Assert
            Assert.Equal(expected, result);

            _clientMock.Verify(
                x => x.PostAsync("Usuario/create-Usuario", model),
                Times.Once
            );
        }

        // -----------------------------------------------------------
        // 5. UPDATE (PUT)
        // -----------------------------------------------------------
        [Fact]
        public async Task UpdateServicesPut_CallsClientPutAsync()
        {
            // Arrange
            var model = new UpdateUsuarioModel { Id = 1, Nombre = "Nuevo Nombre" };
            var expected = ServicesResultModel.Ok(200, "Actualizado");

            _clientMock
                .Setup(x => x.PutAsync("Usuario/update-Usuario", model))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.UpdateServicesPut(model);

            // Assert
            Assert.Equal(expected, result);

            _clientMock.Verify(
                x => x.PutAsync("Usuario/update-Usuario", model),
                Times.Once
            );
        }
    }
}