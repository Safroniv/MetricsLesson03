using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgentTests
{
    public class DotNetErrorsMetricsControllerTests
    {
        private DotNetErrorsMetricsController _DotNetErrorsMetricsController;

        public DotNetErrorsMetricsControllerTests()
        {
            _DotNetErrorsMetricsController = new DotNetErrorsMetricsController();
        }

        [Fact]
        public void GetDotNetErrorsMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _DotNetErrorsMetricsController.GetDotNetErrorsMetrics(fromTime, toTime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
