using Moq;
using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Reserva;
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
    public class ReservaServiceAPI_Tests
    {
        private readonly Mock<IClientAPI<ReservaModel>> _clientMock;
        private readonly Mock<IReservaRepositoryMemory> _memoryMock;
        private readonly Mock<IServicioAdicionalRepositoryMemory> _servicioMemoryMock;
        private readonly ReservaServiceAPI _service;

        public ReservaServiceAPI_Tests()
        {
            _clientMock = new Mock<IClientAPI<ReservaModel>>();
            _memoryMock = new Mock<IReservaRepositoryMemory>();
            _servicioMemoryMock = new Mock<IServicioAdicionalRepositoryMemory>();

            _service = new ReservaServiceAPI(
                _clientMock.Object,
                _memoryMock.Object,
                _servicioMemoryMock.Object
            );
        }


        // ======================================================
        // GET BY ID
        // ======================================================
        [Fact]
        public void GetByIDServices_ReturnsCorrectModel()
        {
            var expected = new ServicesResultModel { Success = true };
            _memoryMock.Setup(m => m.GetByIDModel(5)).Returns(expected);

            var result = _service.GetByIDServices(5);

            Assert.Equal(expected, result);
            _memoryMock.Verify(m => m.GetByIDModel(5), Times.Once);
        }


        // ======================================================
        // GET ALL
        // ======================================================
        [Fact]
        public void GetServices_ReturnsList()
        {
            var expectedList = new List<ReservaModel>();
            _memoryMock.Setup(m => m.GetModels()).Returns(expectedList);

            var result = _service.GetServices();

            Assert.Equal(expectedList, result);
        }


        // ======================================================
        // DELETE
        // ======================================================
        [Fact]
        public async Task RemoveServicesPut_CallsCorrectEndpoint()
        {
            var expected = ServicesResultModel.Ok(200);
            _clientMock.Setup(c => c.DeleteAsync("Reserva/Remove-Reserva?id=10"))
                       .ReturnsAsync(expected);

            var result = await _service.RemoveServicesPut(10);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.DeleteAsync("Reserva/Remove-Reserva?id=10"), Times.Once);
        }


        // ======================================================
        // POST - CREATE
        // ======================================================
        [Fact]
        public async Task SaveServicesPost_CallsCorrectEndpoint()
        {
            var model = new CreateReservaModel();
            var expected = ServicesResultModel.Ok(200);

            _clientMock.Setup(c => c.PostAsync("Reserva/Create-Reserva", model))
                       .ReturnsAsync(expected);

            var result = await _service.SaveServicesPost(model);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.PostAsync("Reserva/Create-Reserva", model), Times.Once);
        }


        // ======================================================
        // PUT - UPDATE
        // ======================================================
        [Fact]
        public async Task UpdateServicesPut_CallsCorrectEndpoint()
        {
            var model = new UpdateReservaModel();
            var expected = ServicesResultModel.Ok(200);

            _clientMock.Setup(c => c.PutAsync("Reserva/Update-Reserva", model))
                       .ReturnsAsync(expected);

            var result = await _service.UpdateServicesPut(model);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.PutAsync("Reserva/Update-Reserva", model), Times.Once);
        }


        // ======================================================
        // PUT - ADD SERVICIO
        // ======================================================
        [Fact]
        public async Task AddServicio_ReservaPut_CallsCorrectEndpoint()
        {
            string name = "Spa";
            int id = 5;
            var expected = ServicesResultModel.Ok(200);

            string endpoint = $"Reserva/Add-Servicio-Adicional-to-Reserva?id={id}&nameServicio={name}";

            _clientMock.Setup(c => c.PutAsync(endpoint, null))
                       .ReturnsAsync(expected);

            var result = await _service.AddServicio_ReservaPut(name, id);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.PutAsync(endpoint, null), Times.Once);
        }


        // ======================================================
        // PUT - REMOVE SERVICIO
        // ======================================================
        [Fact]
        public async Task RemoveServicio_ReservaPut_CallsCorrectEndpoint()
        {
            string name = "Wifi";
            int id = 12;

            var expected = ServicesResultModel.Ok(200);
            string endpoint = $"Reserva/Remove-Servicio-Adicional-to-Reserva?id={id}&nombreServicio={name}";

            _clientMock.Setup(c => c.PutAsync(endpoint, null))
                       .ReturnsAsync(expected);

            var result = await _service.RemoveServicio_ReservaPut(name, id);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.PutAsync(endpoint, null), Times.Once);
        }


        // ======================================================
        // GET LIST BY RESERVA
        // ======================================================
        [Fact]
        public async Task GetServicesbyReserva_CallsCorrectEndpoint()
        {
            int id = 8;
            var expected = ServicesResultModel.Ok(200);
            string endpoint = $"Reserva/Get-Servicios-By-ReservaID?id={id}";

            _clientMock.Setup(c => c.GetListAsync(endpoint))
                       .ReturnsAsync(expected);

            var result = await _service.GetServicesbyReserva(id);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.GetListAsync(endpoint), Times.Once);
        }


        // ======================================================
        // GET SERVICIOS ADICIONALES DISPONIBLES
        // ======================================================
        [Fact]
        public void GetServiciosAdicionalesdisponibles_ReturnsMemoryList()
        {
            var list = new List<ServicioAdicionalModel>();
            _servicioMemoryMock.Setup(m => m.GetModels()).Returns(list);

            var result = _service.GetServiciosAdicionalesdisponibles();

            Assert.Equal(list, result);
        }
    }
}
