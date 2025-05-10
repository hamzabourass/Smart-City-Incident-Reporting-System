using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCIRS.Dtos;
using SCIRS.Services.Interfaces;

namespace SCIRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all reports
        /// </summary>
        /// <returns>List of reports</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReportDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                _logger.LogInformation("Getting all reports");
                var reports = await _reportService.GetAllReportsAsync();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all reports");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Get a report by id
        /// </summary>
        /// <param name="id">Report id</param>
        /// <returns>Report</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReportDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReportById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid report ID: {Id}", id);
                    return BadRequest("Invalid report ID. ID must be greater than 0.");
                }

                _logger.LogInformation("Getting report with ID: {Id}", id);
                var report = await _reportService.GetReportByIdAsync(id);

                if (report == null)
                {
                    _logger.LogWarning("Report with ID: {Id} not found", id);
                    return NotFound($"Report with ID {id} not found.");
                }

                return Ok(report);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Bad request for report ID: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting report with ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}