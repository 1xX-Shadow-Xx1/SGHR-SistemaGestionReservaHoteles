using Moq;
using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Reporte;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.ServiceAPI.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Presentacion.Test.Operaciones
{
    public class ReporteServiceAPI_Tests
    {
        private readonly Mock<IReporteRepositoryMemory> _memoryMock;
        private readonly Mock<IClientAPI<ReporteModel>> _clientMock;
        private readonly ReporteServiceAPI _service;

        public ReporteServiceAPI_Tests()
        {
            _memoryMock = new Mock<IReporteRepositoryMemory>();
            _clientMock = new Mock<IClientAPI<ReporteModel>>();

            _service = new ReporteServiceAPI(
                _memoryMock.Object,
                _clientMock.Object
            );
        }

        // ======================================================
        // GET BY ID
        // ======================================================
        [Fact]
        public void GetByIDServices_ReturnsCorrectModel()
        {
            var expected = new ServicesResultModel { Success = true };

            _memoryMock.Setup(m => m.GetByIDModel(10)).Returns(expected);

            var result = _service.GetByIDServices(10);

            Assert.Equal(expected, result);
            _memoryMock.Verify(m => m.GetByIDModel(10), Times.Once);
        }

        // ======================================================
        // GET ALL
        // ======================================================
        [Fact]
        public void GetServices_ReturnsListFromMemory()
        {
            var list = new List<ReporteModel>();

            _memoryMock.Setup(m => m.GetModels()).Returns(list);

            var result = _service.GetServices();

            Assert.Equal(list, result);
            _memoryMock.Verify(m => m.GetModels(), Times.Once);
        }

        // ======================================================
        // DELETE REPORT
        // ======================================================
        [Fact]
        public async Task RemoveServicesPut_CallsCorrectEndpoint()
        {
            int id = 5;
            var expected = ServicesResultModel.Ok(200);

            string endpoint = $"Reporte/Remove-Reporte?id={id}";

            _clientMock.Setup(c => c.DeleteAsync(endpoint))
                       .ReturnsAsync(expected);

            var result = await _service.RemoveServicesPut(id);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.DeleteAsync(endpoint), Times.Once);
        }

        // ======================================================
        // CREATE REPORT (POST)
        // ======================================================
        [Fact]
        public async Task SaveServicesPost_CallsCorrectEndpoint()
        {
            var model = new CreateReporteModel();
            var expected = ServicesResultModel.Ok(200);

            string endpoint = "Reporte/create-Reporte";

            _clientMock.Setup(c => c.PostAsync(endpoint, model))
                       .ReturnsAsync(expected);

            var result = await _service.SaveServicesPost(model);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.PostAsync(endpoint, model), Times.Once);
        }

        // ======================================================
        // UPDATE REPORT (PUT)
        // ======================================================
        [Fact]
        public async Task UpdateServicesPut_CallsCorrectEndpoint()
        {
            var model = new UpdateReporteModel();
            var expected = ServicesResultModel.Ok(200);

            string endpoint = "Reporte/update-Reporte";

            _clientMock.Setup(c => c.PutAsync(endpoint, model))
                       .ReturnsAsync(expected);

            var result = await _service.UpdateServicesPut(model);

            Assert.Equal(expected, result);
            _clientMock.Verify(c => c.PutAsync(endpoint, model), Times.Once);
        }
    }
}
