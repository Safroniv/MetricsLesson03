using MetricsAgent.Converters;
using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/dotnet/errors-count")]
    [ApiController]
    public class DotNetErrorsMetricsController : ControllerBase
    {

        #region Services

        private readonly ILogger<DotNetErrorsMetricsController> _logger;
        private readonly IDotNetErrorsMetricsRepository _dotNetErrorsMetricsRepository;

        #endregion


        public DotNetErrorsMetricsController(
            IDotNetErrorsMetricsRepository DotNetErrorsMetricsRepository,
            ILogger<DotNetErrorsMetricsController> logger)
        {
            _dotNetErrorsMetricsRepository = DotNetErrorsMetricsRepository;
            _logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetErrorsMetricCreateRequest request)
        {
            _dotNetErrorsMetricsRepository.Create(new Models.DotNetErrorsMetric
            {
                Value = request.Value,
                Time = (int)request.Time.TotalSeconds
            });
            return Ok();
        }


        /// <summary>
        /// Получить статистику по ошибкам в сети DOT.NET за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<DotNetErrorsMetric>> GetDotNetErrorsMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {

            _logger.LogInformation("Get dotneterrors metrics call.");
            return Ok(_dotNetErrorsMetricsRepository.GetByTimePeriod(fromTime, toTime));
        }
    }
}
