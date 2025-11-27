using Moq;
using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Reserva;
using SGHR.Web.Models.Reservas.ServicioAdicional;
using SGHR.Web.Models.Usuarios.Cliente;
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
        private readonly Mock<IClientAPI> _clientMock;
        private readonly Mock<IReservaRepositoryMemory> _memoryMock;
        private readonly Mock<IServicioAdicionalRepositoryMemory> _servicioMemoryMock;
        private readonly ReservaServiceAPI _service;

        public ReservaServiceAPI_Tests()
        {
            _clientMock = new Mock<IClientAPI>();
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
            var expected = new ApiResult<ReservaModel> { Success = true };
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
            var expected = ApiResult<ReservaModel>.Ok(new ReservaModel());
            _clientMock.Setup(c => c.DeleteAsync<ReservaModel>("Reserva/Remove-Reserva?id=10"))
                       .ReturnsAsync(expected);

            var result = await _service.RemoveServicesPut(10);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.DeleteAsync<ReservaModel>("Reserva/Remove-Reserva?id=10"), Times.Once);
        }


        // ======================================================
        // POST - CREATE
        // ======================================================
        [Fact]
        public async Task SaveServicesPost_CallsCorrectEndpoint()
        {
            var model = new CreateReservaModel();
            var expected = ApiResult<ReservaModel>.Ok(new ReservaModel());

            _clientMock.Setup(c => c.PostAsJsonAsync<CreateReservaModel, ReservaModel>("Reserva/Create-Reserva", model))
                       .ReturnsAsync(expected);

            var result = await _service.SaveServicesPost(model);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.PostAsJsonAsync<CreateReservaModel, ReservaModel>("Reserva/Create-Reserva", model), Times.Once);
        }


        // ======================================================
        // PUT - UPDATE
        // ======================================================
        [Fact]
        public async Task UpdateServicesPut_CallsCorrectEndpoint()
        {
            var model = new UpdateReservaModel();
            var expected = ApiResult<ReservaModel>.Ok(new ReservaModel());

            _clientMock.Setup(c => c.PutAsJsonAsync<UpdateReservaModel, ReservaModel>("Reserva/Update-Reserva", model))
                       .ReturnsAsync(expected);

            var result = await _service.UpdateServicesPut(model);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.PutAsJsonAsync<UpdateReservaModel, ReservaModel>("Reserva/Update-Reserva", model), Times.Once);
        }


        // ======================================================
        // GET LIST BY RESERVA
        // ======================================================
        [Fact]
        public async Task GetServicesbyReserva_CallsCorrectEndpoint()
        {
            int id = 8;
            var expected = ApiResult<List<ServicioAdicionalModel>>.Ok(new List<ServicioAdicionalModel>());
            string endpoint = $"Reserva/Get-Servicios-By-ReservaID?id={id}";

            _clientMock.Setup(c => c.GetAsync<List<ServicioAdicionalModel>>(endpoint))
                       .ReturnsAsync(expected);

            var result = await _service.GetServicesbyReserva(id);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.GetAsync<List<ServicioAdicionalModel>>(endpoint), Times.Once);
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
