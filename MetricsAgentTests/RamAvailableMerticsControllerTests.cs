using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgentTests
{
    public class RamAvailableMetricsControllerTests
    {
        private RamAvailableMetricsController _RamAvailableMetricsController;

        public RamAvailableMetricsControllerTests()
        {
            _RamAvailableMetricsController = new RamAvailableMetricsController();
        }

        [Fact]
        public void GetRamAvailableMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _RamAvailableMetricsController.GetRamAvailableMetrics(fromTime, toTime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
