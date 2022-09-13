using MetricsAgent.Models.Requests;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using MetricsAgent.Services.Impl;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/hdd/left")]
    [ApiController]
    public class HddLeftMetricsController : ControllerBase
    {

        #region Services

        private readonly ILogger<HddLeftMetricsController> _logger;
        private readonly IHddLeftMetricsRepository _hddLeftMetricsRepository;

        #endregion


        public HddLeftMetricsController(
            IHddLeftMetricsRepository HddLeftMetricsRepository,
            ILogger<HddLeftMetricsController> logger)
        {
            _hddLeftMetricsRepository = HddLeftMetricsRepository;
            _logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] HddLeftMetricCreateRequest request)
        {
            _hddLeftMetricsRepository.Create(new Models.HddLeftMetric
            {
                Value = request.Value,
                Time = (int)request.Time.TotalSeconds
            });
            return Ok();
        }


        /// <summary>
        /// Получить статистику по свободному месту на жестком диске за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<HddLeftMetric>> GetHddLeftMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {

            _logger.LogInformation("Get hddeft metrics call.");
            return Ok(_hddLeftMetricsRepository.GetByTimePeriod(fromTime, toTime));
        }
    }
}
