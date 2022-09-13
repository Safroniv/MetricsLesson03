using MetricsAgent.Controllers;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgentTests
{
    public class RamAvailableMetricsControllerTests
    {
        private readonly RamAvailableMetricsController _ramAvailableMetricsController;
        private readonly Mock<IRamAvailableMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<RamAvailableMetricsController>> _loggerMock;

        public RamAvailableMetricsControllerTests()
        {
            _repositoryMock = new Mock<IRamAvailableMetricsRepository>();
            _loggerMock = new Mock<ILogger<RamAvailableMetricsController>>();

            _ramAvailableMetricsController = new RamAvailableMetricsController(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Create_RamAvailableMetric_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            _repositoryMock.Setup(repository =>
                repository.Create(It.IsAny<RamAvailableMetric>())).Verifiable();

            // выполняем действие на контроллере
            var result = _ramAvailableMetricsController.Create(new MetricsAgent.Models.Requests.RamAvailableMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });

            _repositoryMock.Verify(repository =>
                repository.Create(It.IsAny<RamAvailableMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetRamAvailableMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _ramAvailableMetricsController.GetRamAvailableMetrics(fromTime, toTime);
            Assert.IsAssignableFrom<ActionResult<IList<RamAvailableMetric>>>(result);
        }

    }
}
