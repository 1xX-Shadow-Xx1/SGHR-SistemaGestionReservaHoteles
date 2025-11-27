using Moq;
using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Tarifa;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.ServiceAPI.Reservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Presentacion.Test.Reservas
{
    public class TarifaServiceAPI_Tests
    {
        private readonly Mock<IClientAPI<TarifaModel>> _clientMock;
        private readonly Mock<ITarifaRepositoryMemory> _memoryMock;
        private readonly TarifaServiceAPI _service;

        public TarifaServiceAPI_Tests()
        {
            _clientMock = new Mock<IClientAPI<TarifaModel>>();
            _memoryMock = new Mock<ITarifaRepositoryMemory>();

            _service = new TarifaServiceAPI(
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

            _memoryMock.Setup(m => m.GetByIDModel(2)).Returns(expected);

            var result = _service.GetByIDServices(2);

            Assert.Equal(expected, result);
            _memoryMock.Verify(m => m.GetByIDModel(2), Times.Once);
        }

        // ======================================================
        // GET ALL
        // ======================================================
        [Fact]
        public void GetServices_ReturnsMemoryList()
        {
            var list = new List<TarifaModel>();

            _memoryMock.Setup(m => m.GetModels()).Returns(list);

            var result = _service.GetServices();

            Assert.Equal(list, result);
        }

        // ======================================================
        // DELETE TARIFF
        // ======================================================
        [Fact]
        public async Task RemoveServicesPut_CallsCorrectEndpoint()
        {
            int id = 7;
            var expected = ServicesResultModel.Ok(200);

            string endpoint = $"Tarifa/Remove-Tarifa?id={id}";

            _clientMock.Setup(c => c.DeleteAsync(endpoint))
                       .ReturnsAsync(expected);

            var result = await _service.RemoveServicesPut(id);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.DeleteAsync(endpoint), Times.Once);
        }

        // ======================================================
        // CREATE TARIFF (POST)
        // ======================================================
        [Fact]
        public async Task SaveServicesPost_CallsCorrectEndpoint()
        {
            var model = new CreateTarifaModel();
            var expected = ServicesResultModel.Ok(200);

            string endpoint = "Tarifa/Create-Tarifa";

            _clientMock.Setup(c => c.PostAsync(endpoint, model))
                       .ReturnsAsync(expected);

            var result = await _service.SaveServicesPost(model);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.PostAsync(endpoint, model), Times.Once);
        }

        // ======================================================
        // UPDATE TARIFF (PUT)
        // ======================================================
        [Fact]
        public async Task UpdateServicesPut_CallsCorrectEndpoint()
        {
            var model = new UpdateTarifaModel();
            var expected = ServicesResultModel.Ok(200);

            string endpoint = "Tarifa/Update-Tarifa";

            _clientMock.Setup(c => c.PutAsync(endpoint, model))
                       .ReturnsAsync(expected);

            var result = await _service.UpdateServicesPut(model);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.PutAsync(endpoint, model), Times.Once);
        }
    }
}
