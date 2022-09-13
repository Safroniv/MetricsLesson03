using MetricsAgent.Controllers;
using MetricsAgent.Models;
using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
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
    public class NetworkMetricsControllerTests
    {
        private readonly NetworkMetricsController _networkMetricsController;
        private readonly Mock<INetworkMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<NetworkMetricsController>> _loggerMock;

        public NetworkMetricsControllerTests()
        {
            _repositoryMock = new Mock<INetworkMetricsRepository>();
            _loggerMock = new Mock<ILogger<NetworkMetricsController>>();

            _networkMetricsController = new NetworkMetricsController(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Create_NetworkMetric_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            _repositoryMock.Setup(repository =>
                repository.Create(It.IsAny<NetworkMetric>())).Verifiable();

            // выполняем действие на контроллере
            var result = _networkMetricsController.Create(new MetricsAgent.Models.Requests.NetworkMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });

            _repositoryMock.Verify(repository =>
                repository.Create(It.IsAny<NetworkMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetNetworkMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _networkMetricsController.GetNetworkMetrics(fromTime, toTime);
            Assert.IsAssignableFrom<ActionResult<IList<NetworkMetric>>>(result);
        }
    }
}
