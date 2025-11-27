using Moq;
using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Habitacion;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.ServiceAPI.Habitaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Presentacion.Test.Habitaciones
{
    public class HabitacionServiceAPI_Test
    {
        private readonly Mock<IHabitacionRepositoryMemory> _memoryMock;
        private readonly Mock<IClientAPI<HabitacionModel>> _clientApiMock;
        private readonly HabitacionServiceAPI _service;

        public HabitacionServiceAPI_Test()
        {
            _memoryMock = new Mock<IHabitacionRepositoryMemory>();
            _clientApiMock = new Mock<IClientAPI<HabitacionModel>>();
            _service = new HabitacionServiceAPI(_memoryMock.Object, _clientApiMock.Object);
        }

        // --------------------------------------------------------------------
        // GET BY ID
        // --------------------------------------------------------------------
        [Fact]
        public void GetByIDServices_ShouldReturnResultFromMemory()
        {
            var expected = ServicesResultModel.Ok(200);
            _memoryMock.Setup(m => m.GetByIDModel(1)).Returns(expected);

            var result = _service.GetByIDServices(1);

            Assert.Equal(expected, result);
            _memoryMock.Verify(m => m.GetByIDModel(1), Times.Once);
        }

        // --------------------------------------------------------------------
        // GET ALL
        // --------------------------------------------------------------------
        [Fact]
        public void GetServices_ShouldReturnListFromMemory()
        {
            var list = new List<HabitacionModel> { new HabitacionModel { Id = 1 } };
            _memoryMock.Setup(m => m.GetModels()).Returns(list);

            var result = _service.GetServices();

            Assert.Equal(list, result);
            _memoryMock.Verify(m => m.GetModels(), Times.Once);
        }

        // --------------------------------------------------------------------
        // GET BY NUMERO
        // --------------------------------------------------------------------
        [Fact]
        public void GetHabitacionByNumero_ShouldReturnResultFromMemory()
        {
            var expected = ServicesResultModel.Fail(400, "");
            _memoryMock.Setup(m => m.GetHabitacionByNumero("101")).Returns(expected);

            var result = _service.GetHabitacionByNumero("101");

            Assert.Equal(expected, result);
            _memoryMock.Verify(m => m.GetHabitacionByNumero("101"), Times.Once);
        }

        // --------------------------------------------------------------------
        // REMOVE (PUT DELETE)
        // --------------------------------------------------------------------
        [Fact]
        public async Task RemoveServicesPut_ShouldCallDeleteAsync()
        {
            var expected = ServicesResultModel.Ok(200);
            _clientApiMock
                .Setup(api => api.DeleteAsync("Habitacion/Remove-Habitacion?id=1"))
                .ReturnsAsync(expected);

            var result = await _service.RemoveServicesPut(1);

            Assert.Equal(expected, result);
            _clientApiMock.Verify(api => api.DeleteAsync("Habitacion/Remove-Habitacion?id=1"), Times.Once);
        }

        // --------------------------------------------------------------------
        // SAVE POST
        // --------------------------------------------------------------------
        [Fact]
        public async Task SaveServicesPost_ShouldCallPostAsync()
        {
            var model = new CreateHabitacionModel();
            var expected = ServicesResultModel.Ok(200);

            _clientApiMock
                .Setup(api => api.PostAsync("Habitacion/Create-Habitacion", model))
                .ReturnsAsync(expected);

            var result = await _service.SaveServicesPost(model);

            Assert.Equal(expected, result);
            _clientApiMock.Verify(api => api.PostAsync("Habitacion/Create-Habitacion", model), Times.Once);
        }

        // --------------------------------------------------------------------
        // UPDATE PUT
        // --------------------------------------------------------------------
        [Fact]
        public async Task UpdateServicesPut_ShouldCallPutAsync()
        {
            var model = new UpdateHabitacionModel();
            var expected = ServicesResultModel.Ok(200);

            _clientApiMock
                .Setup(api => api.PutAsync("Habitacion/Update-Habitacion", model))
                .ReturnsAsync(expected);

            var result = await _service.UpdateServicesPut(model);

            Assert.Equal(expected, result);
            _clientApiMock.Verify(api => api.PutAsync("Habitacion/Update-Habitacion", model), Times.Once);
        }

        // --------------------------------------------------------------------
        // GET HABITACIONES DISPONIBLES
        // --------------------------------------------------------------------
        [Fact]
        public async Task GetHabitacionesDisponibles_ShouldCallGetListAsync()
        {
            var expected = ServicesResultModel.Ok(200);

            _clientApiMock
                .Setup(api => api.GetListAsync("Habitacion/Get-Habitaciones-disponibles"))
                .ReturnsAsync(expected);

            var result = await _service.GetHabitacionesDisponibles();

            Assert.Equal(expected, result);
            _clientApiMock.Verify(api => api.GetListAsync("Habitacion/Get-Habitaciones-disponibles"), Times.Once);
        }

        // --------------------------------------------------------------------
        // GET DISPONIBLES POR RANGO DE FECHAS
        // --------------------------------------------------------------------
        [Fact]
        public async Task GetHabitacionesDisponiblesRangeDate_ShouldCallGetListAsync()
        {
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 5);

            var expected = ServicesResultModel.Ok(200);

            string expectedUrl =
                $"Habitacion/Get-Habitaciones-disponibles-date?fechainicio={start}&fechafin={end}";

            _clientApiMock
                .Setup(api => api.GetListAsync(expectedUrl))
                .ReturnsAsync(expected);

            var result = await _service.GetHabitacionesDisponiblesRangeDate(start, end);

            Assert.Equal(expected, result);
            _clientApiMock.Verify(api => api.GetListAsync(expectedUrl), Times.Once);
        }
    }
}
