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
    public class HddLeftMetricsControllerTests
    {
        private readonly HddLeftMetricsController _hddLeftMetricsController;
        private readonly Mock<IHddLeftMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<HddLeftMetricsController>> _loggerMock;

        public HddLeftMetricsControllerTests()
        {
            _repositoryMock = new Mock<IHddLeftMetricsRepository>();
            _loggerMock = new Mock<ILogger<HddLeftMetricsController>>();

            _hddLeftMetricsController = new HddLeftMetricsController(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Create_HddLeftMetric_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            _repositoryMock.Setup(repository =>
                repository.Create(It.IsAny<HddLeftMetric>())).Verifiable();

            // выполняем действие на контроллере
            var result = _hddLeftMetricsController.Create(new MetricsAgent.Models.Requests.HddLeftMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });

            _repositoryMock.Verify(repository =>
                repository.Create(It.IsAny<HddLeftMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetHddLeftMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _hddLeftMetricsController.GetHddLeftMetrics(fromTime, toTime);
            Assert.IsAssignableFrom<ActionResult<IList<HddLeftMetric>>>(result);
        }
    }
}
