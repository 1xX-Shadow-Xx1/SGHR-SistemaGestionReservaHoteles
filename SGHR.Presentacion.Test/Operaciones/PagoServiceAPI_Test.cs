using Moq;
using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Pago;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.ServiceAPI.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Presentacion.Test.Operaciones
{
    public class PagoServiceAPI_Tests
    {
        private readonly Mock<IPagoRepositoryMemory> _memoryMock;
        private readonly Mock<IClientAPI<PagoModel>> _clientApiMock;
        private readonly PagoServiceAPI _service;

        public PagoServiceAPI_Tests()
        {
            _memoryMock = new Mock<IPagoRepositoryMemory>();
            _clientApiMock = new Mock<IClientAPI<PagoModel>>();

            _service = new PagoServiceAPI(
                _memoryMock.Object,
                _clientApiMock.Object
            );
        }

        // -----------------------------------------------------
        // 🔹 GET MEMORY - getPagoById
        // -----------------------------------------------------
        [Fact]
        public void GetPagoById_ReturnsFromMemory()
        {
            var expected = new ServicesResultModel { Success = true };
            _memoryMock.Setup(m => m.GetByIDModel(1)).Returns(expected);

            var result = _service.getPagoById(1);

            Assert.Equal(expected, result);
            _memoryMock.Verify(m => m.GetByIDModel(1), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 GET MEMORY LIST - getPagoList
        // -----------------------------------------------------
        [Fact]
        public void GetPagoList_ReturnsMemoryList()
        {
            var expectedList = new List<PagoModel>
            {
                new PagoModel { Id = 1 }
            };

            _memoryMock.Setup(m => m.GetModels()).Returns(expectedList);

            var result = _service.getPagoList();

            Assert.Equal(expectedList, result);
            _memoryMock.Verify(m => m.GetModels(), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 API: AnularPago
        // -----------------------------------------------------
        [Fact]
        public async Task AnularPago_CallsApiAndReturnsResult()
        {
            var response = new ServicesResultModel { Success = true };

            _clientApiMock
                .Setup(api => api.DeleteAsync("Pago/Anular-Pago?idPago=5"))
                .ReturnsAsync(response);

            var result = await _service.AnularPago(5);

            Assert.Equal(response, result);
            _clientApiMock.Verify(api => api.DeleteAsync("Pago/Anular-Pago?idPago=5"), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 API: GetResumenDePagos
        // -----------------------------------------------------
        [Fact]
        public async Task GetResumenDePagos_CallsApi()
        {
            var expected = new ServicesResultModel { Success = true };

            _clientApiMock
                .Setup(api => api.GetResumenPagoAsync("Pago/Get-Resumen-Pagos"))
                .ReturnsAsync(expected);

            var result = await _service.GetResumenDePagos();

            Assert.Equal(expected, result);
            _clientApiMock.Verify(api => api.GetResumenPagoAsync("Pago/Get-Resumen-Pagos"), Times.Once);
        }

        // -----------------------------------------------------
        // 🔹 API: RealizarPago
        // -----------------------------------------------------
        [Fact]
        public async Task RealizarPago_CallsApiWithCorrectModel()
        {
            var input = new RealizarPagoModel
            {
                IdReserva = 10,
                Monto = 5000
            };

            var expected = new ServicesResultModel { Success = true };

            _clientApiMock
                .Setup(api => api.PostAsync("Pago/Realizar-Pago", input))
                .ReturnsAsync(expected);

            var result = await _service.RealizarPago(input);

            Assert.Equal(expected, result);
            _clientApiMock.Verify(api => api.PostAsync("Pago/Realizar-Pago", input), Times.Once);
        }
    }
}
