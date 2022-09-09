using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgentTests
{
    public class HddLeftMetricsControllerTests
    {
        private HddLeftMetricsController _HddLeftMetricsController;

        public HddLeftMetricsControllerTests()
        {
            _HddLeftMetricsController = new HddLeftMetricsController();
        }

        [Fact]
        public void GetHddLeftMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _HddLeftMetricsController.GetHddLeftMetrics(fromTime, toTime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
