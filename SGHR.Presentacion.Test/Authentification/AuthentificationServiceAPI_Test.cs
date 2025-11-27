using Microsoft.AspNetCore.Http;
using Moq;
using SGHR.Web.Models;
using SGHR.Web.Models.Sesion;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.ServiceAPI.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Presentacion.Test.Authentification
{
    public class AuthentificationServiceAPI_Test
    {
        private readonly Mock<IClientAPI<SesionLoginModel>> _clientMock;
        private readonly Mock<IHttpContextAccessor> _httpContextMock;
        private readonly AuthentificationServiceAPI _service;

        public AuthentificationServiceAPI_Test()
        {
            _clientMock = new Mock<IClientAPI<SesionLoginModel>>();
            _httpContextMock = new Mock<IHttpContextAccessor>();

            // Simular HttpContext y Session
            var context = new DefaultHttpContext();
            context.Session = new DummySession();

            // Setear un UserId en la sesión
            context.Session.SetInt32("UserId", 10);

            _httpContextMock.Setup(x => x.HttpContext).Returns(context);

            _service = new AuthentificationServiceAPI(_clientMock.Object, _httpContextMock.Object);
        }

        // -----------------------------
        // TEST: CheckSesionAsync
        // -----------------------------
        [Fact]
        public async Task CheckSesionAsync_ShouldCallClientAPI_GetSesionAsync()
        {
            // Arrange
            var expected = ServicesResultModel.Ok(200);
            _clientMock.Setup(x => x.GetSesionAsync(It.IsAny<string>()))
                       .ReturnsAsync(expected);

            // Act
            var result = await _service.CheckSesionAsync();

            // Assert
            Assert.Equal(expected, result);
            _clientMock.Verify(x => x.GetSesionAsync("Sesion/CheckSesionActivityByUserID?userId=10"), Times.Once);
        }

        // -----------------------------
        // TEST: CloseSesionAsync
        // -----------------------------
        [Fact]
        public async Task CloseSesionAsync_ShouldCallClientAPI_PutAsync()
        {
            // Arrange
            var expected = ServicesResultModel.Ok(200);
            _clientMock.Setup(x => x.PutAsync(It.IsAny<string>(), null)).ReturnsAsync(expected);

            // Act
            var result = await _service.CloseSesionAsync();

            // Assert
            Assert.Equal(expected, result);
            _clientMock.Verify(x => x.PutAsync("Authentication/Authentication-CloseSesion?id=10", null), Times.Once);
        }

        // -----------------------------
        // TEST: LoginAsync
        // -----------------------------
        [Fact]
        public async Task LoginAsync_ShouldCallClientAPI_PutAsync_WithUserCredentials()
        {
            // Arrange
            var expected = ServicesResultModel.Ok(200);
            _clientMock.Setup(x => x.PutAsync(It.IsAny<string>(), null)).ReturnsAsync(expected);

            // Act
            var result = await _service.LoginAsync("kevin", "1234");

            // Assert
            Assert.Equal(expected, result);
            _clientMock.Verify(
                x => x.PutAsync("Authentication/Authentication-Login?correo=kevin&contraseña=1234", null),
                Times.Once
            );
        }

        // -----------------------------
        // TEST: RegisterAsync
        // -----------------------------
        [Fact]
        public async Task RegisterAsync_ShouldCallClientAPI_PostAsync()
        {
            // Arrange
            var expected = ServicesResultModel.Ok(200);
            var model = new CreateUsuarioModel { Nombre = "Kevin" };

            _clientMock.Setup(x => x.PostAsync("Authentication/Authentication-Register", model))
                       .ReturnsAsync(expected);

            // Act
            var result = await _service.RegisterAsync(model);

            // Assert
            Assert.Equal(expected, result);
            _clientMock.Verify(
                x => x.PostAsync("Authentication/Authentication-Register", model),
                Times.Once
            );
        }

        // -----------------------------
        // TEST: UpdateActivitySesionAsync
        // -----------------------------
        [Fact]
        public async Task UpdateActivitySesionAsync_ShouldCallClientAPI_PutAsync()
        {
            // Arrange
            var expected = ServicesResultModel.Ok(200);
            _clientMock.Setup(x => x.PutAsync(It.IsAny<string>(), null))
                       .ReturnsAsync(expected);

            // Act
            var result = await _service.UpdateActivitySesionAsync();

            // Assert
            Assert.Equal(expected, result);
            _clientMock.Verify(
                x => x.PutAsync("Sesion/UpdateActivitySesionByUser?userId=10", null),
                Times.Once
            );
        }
    }

    // ---------------------------------------------------------
    // Implementación mínima de ISession para pruebas unitarias
    // ---------------------------------------------------------
    public class DummySession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new();

        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public string Id => "dummy";
        public bool IsAvailable => true;

        public void Clear() => _sessionStorage.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Remove(string key) => _sessionStorage.Remove(key);

        public void Set(string key, byte[] value) => _sessionStorage[key] = value;

        public bool TryGetValue(string key, out byte[] value)
            => _sessionStorage.TryGetValue(key, out value);
    }
}

