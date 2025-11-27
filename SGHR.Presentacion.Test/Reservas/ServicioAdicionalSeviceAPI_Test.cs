using Moq;
using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.ServicioAdicional;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.ServiceAPI.Reservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Presentacion.Test.Reservas
{
    public class ServicioAdicionalServiceAPI_Tests
    {
        private readonly Mock<IClientAPI<ServicioAdicionalModel>> _clientMock;
        private readonly Mock<IServicioAdicionalRepositoryMemory> _memoryMock;
        private readonly ServicioAdicionalServiceAPI _service;

        public ServicioAdicionalServiceAPI_Tests()
        {
            _clientMock = new Mock<IClientAPI<ServicioAdicionalModel>>();
            _memoryMock = new Mock<IServicioAdicionalRepositoryMemory>();

            _service = new ServicioAdicionalServiceAPI(
                _clientMock.Object,
                _memoryMock.Object
            );
        }

        // ======================================================
        // GET BY ID
        // ======================================================
        [Fact]
        public void GetByIDServices_ReturnsCorrectModel()
        {
            var expected = new ServicesResultModel { Success = true };

            _memoryMock.Setup(m => m.GetByIDModel(3)).Returns(expected);

            var result = _service.GetByIDServices(3);

            Assert.Equal(expected, result);
            _memoryMock.Verify(m => m.GetByIDModel(3), Times.Once);
        }

        // ======================================================
        // GET ALL SERVICES
        // ======================================================
        [Fact]
        public void GetServices_ReturnsList()
        {
            var list = new List<ServicioAdicionalModel>();

            _memoryMock.Setup(m => m.GetModels()).Returns(list);

            var result = _service.GetServices();

            Assert.Equal(list, result);
        }

        // ======================================================
        // DELETE SERVICE
        // ======================================================
        [Fact]
        public async Task RemoveServicesPut_CallsCorrectEndpoint()
        {
            int id = 9;
            var expected = ServicesResultModel.Ok(200);

            string endpoint = $"ServicioAdicional/Remove-Servicio-Adicional?id={id}";

            _clientMock.Setup(c => c.DeleteAsync(endpoint))
                       .ReturnsAsync(expected);

            var result = await _service.RemoveServicesPut(id);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.DeleteAsync(endpoint), Times.Once);
        }

        // ======================================================
        // CREATE SERVICE (POST)
        // ======================================================
        [Fact]
        public async Task SaveServicesPost_CallsCorrectEndpoint()
        {
            var model = new CreateServicioAdicionalModel();
            var expected = ServicesResultModel.Ok(200);
            string endpoint = "ServicioAdicional/Create-Servicio-Adicional";

            _clientMock.Setup(c => c.PostAsync(endpoint, model))
                       .ReturnsAsync(expected);

            var result = await _service.SaveServicesPost(model);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.PostAsync(endpoint, model), Times.Once);
        }

        // ======================================================
        // UPDATE SERVICE (PUT)
        // ======================================================
        [Fact]
        public async Task UpdateServicesPut_CallsCorrectEndpoint()
        {
            var model = new UpdateServicioAdicionalModel();
            var expected = ServicesResultModel.Ok(200);
            string endpoint = "ServicioAdicional/Update-Servicio-Adicional";

            _clientMock.Setup(c => c.PutAsync(endpoint, model))
                       .ReturnsAsync(expected);

            var result = await _service.UpdateServicesPut(model);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.PutAsync(endpoint, model), Times.Once);
        }
    }
}
