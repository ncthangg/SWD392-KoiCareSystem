using AutoMapper;
using KoiCareSystem.Api.Controllers.BaseController;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Service.Helper;
using KoiCareSystem.Service;
using Microsoft.AspNetCore.Mvc;
using KoiCareSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystem.Api.Controllers
{

    [Route("waterstatus")]
    [ApiController]
    public class WaterController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly AuthenticateService _authenticateService;

        private readonly PondService _pondService;
        private readonly WaterParameterService _waterParameterService;

        private readonly IUrlHelperService _urlHelperService;
        private readonly IMapper _mapper;

        public WaterController(ApplicationDbContext context, UserService userService, RoleService roleService,
            AuthenticateService authenticateService, PondService pondService, WaterParameterService waterParameterService,
        IUrlHelperService urlHelperService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _roleService = roleService;
            _authenticateService = authenticateService;

            _pondService = pondService;
            _waterParameterService = waterParameterService;

            _urlHelperService = urlHelperService;
            _mapper = mapper;
        }

        [HttpGet("{pondId}")]
        public async Task<IActionResult> GetWaterParametersAsync(int pondId)
        {
            // Check if pond exists
            var pond = await _pondService.GetById(pondId);
            if (pond.Data == null)
            {
                return NotFound(new { Message = "Pond not found" });
            }

            // Fetch the most recent WaterParameter for the specified PondId
            var waterParameter = await _context.WaterParameters
                .Include(x => x.Status)
                .Where(m => m.PondId == pondId)
                .OrderByDescending(m => m.UpdatedAt)
                .FirstOrDefaultAsync();

            if (waterParameter == null)
            {
                // Return default WaterParameter if no record is found
                var defaultWaterParameter = new WaterParameter()
                {
                    ParameterId = 0,
                    PondId = pondId,
                    MeasurementDate = null,
                    Temperature = 0,
                    Salinity = 0,
                    Ph = 0,
                    O2 = 0,
                    No2 = 0,
                    No3 = 0,
                    Po4 = 0,
                    WaterVolume = 0,
                    CreatedAt = null,
                    UpdatedAt = null,
                    StatusId = 0
                };

                return Ok(new
                {
                    WaterParameter = defaultWaterParameter,
                    Status = "unknown",
                    Evaluations = new Dictionary<string, string>()
                });
            }
            else
            {
                // If WaterParameter exists, get evaluations
                var statusName = waterParameter.Status?.StatusName ?? "unknown";
                var evaluations = await _waterParameterService.Evaluate(waterParameter);

                return Ok(new
                {
                    WaterParameter = waterParameter,
                    Status = statusName,
                    Evaluations = evaluations
                });
            }
        }

    }
}
