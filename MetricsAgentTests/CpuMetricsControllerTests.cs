using MetricsAgent.Controllers;
using MetricsAgent.Services.Impl;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetricsAgent.Models;
using Moq;

namespace MetricsAgentTests
{
    public class CpuMetricsControllerTests
    {
        private readonly CpuMetricsController _cpuMetricsController;
        private readonly Mock<ICpuMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<CpuMetricsController>> _loggerMock;

        public CpuMetricsControllerTests()
        {
            _repositoryMock = new Mock<ICpuMetricsRepository>();
            _loggerMock = new Mock<ILogger<CpuMetricsController>>();

            _cpuMetricsController = new CpuMetricsController(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Create_CpuMetric_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            _repositoryMock.Setup(repository =>
                repository.Create(It.IsAny<CpuMetric>())).Verifiable();

            // выполняем действие на контроллере
            var result = _cpuMetricsController.Create(new MetricsAgent.Models.Requests.CpuMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });

            _repositoryMock.Verify(repository =>
                repository.Create(It.IsAny<CpuMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetCpuMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _cpuMetricsController.GetCpuMetrics(fromTime, toTime);
            Assert.IsAssignableFrom<ActionResult<IList<CpuMetric>>>(result);
        }
    }
}
