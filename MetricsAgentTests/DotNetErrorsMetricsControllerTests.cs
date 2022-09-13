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
    public class DotNetErrorsMetricsControllerTests
    {
        private readonly DotNetErrorsMetricsController _dotNetErrorsMetricsController;
        private readonly Mock<IDotNetErrorsMetricsRepository> _repositoryMock;
        private readonly Mock<ILogger<DotNetErrorsMetricsController>> _loggerMock;

        public DotNetErrorsMetricsControllerTests()
        {
            _repositoryMock = new Mock<IDotNetErrorsMetricsRepository>();
            _loggerMock = new Mock<ILogger<DotNetErrorsMetricsController>>();

            _dotNetErrorsMetricsController = new DotNetErrorsMetricsController(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Create_DotNetErrorsMetric_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            _repositoryMock.Setup(repository =>
                repository.Create(It.IsAny<DotNetErrorsMetric>())).Verifiable();

            // выполняем действие на контроллере
            var result = _dotNetErrorsMetricsController.Create(new MetricsAgent.Models.Requests.DotNetErrorsMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });

            _repositoryMock.Verify(repository =>
                repository.Create(It.IsAny<DotNetErrorsMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetDotNetErrorsMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _dotNetErrorsMetricsController.GetDotNetErrorsMetrics(fromTime, toTime);
            Assert.IsAssignableFrom<ActionResult<IList<DotNetErrorsMetric>>>(result);
        }
    }
}
