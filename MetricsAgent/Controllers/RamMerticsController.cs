using MetricsAgent.Models.Requests;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MetricsAgent.Services.Impl;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram/available")]
    [ApiController]
    public class RamAvailableMetricsController : ControllerBase
    {
        #region Services

        private readonly ILogger<RamAvailableMetricsController> _logger;
        private readonly IRamAvailableMetricsRepository _ramAvailableMetricsRepository;

        #endregion


        public RamAvailableMetricsController(
            IRamAvailableMetricsRepository RamAvailableMetricsRepository,
            ILogger<RamAvailableMetricsController> logger)
        {
            _ramAvailableMetricsRepository = RamAvailableMetricsRepository;
            _logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] RamAvailableMetricCreateRequest request)
        {
            _ramAvailableMetricsRepository.Create(new Models.RamAvailableMetric
            {
                Value = request.Value,
                Time = (int)request.Time.TotalSeconds
            });
            return Ok();
        }


        /// <summary>
        /// Получить статистику по свободному месту в оперативной памяти за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<RamAvailableMetric>> GetRamAvailableMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {

            _logger.LogInformation("Get networkmetric metrics call.");
            return Ok(_ramAvailableMetricsRepository.GetByTimePeriod(fromTime, toTime));
        }
    }
}
