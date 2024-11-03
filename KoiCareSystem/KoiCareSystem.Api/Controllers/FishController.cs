using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Api.Controllers.BaseController;
using AutoMapper;
using KoiCareSystem.Service.Helper;
using KoiCareSystem.Service;

namespace KoiCareSystem.Api.Controllers
{
    [Route("koifish")]
    [ApiController]
    public class FishController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly AuthenticateService _authenticateService;
        private readonly PondService _pondService;
        private readonly IUrlHelperService _urlHelperService;
        private readonly IMapper _mapper;

        public FishController(ApplicationDbContext context, UserService userService, RoleService roleService, AuthenticateService authenticateService, PondService pondService, IUrlHelperService urlHelperService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _roleService = roleService;
            _authenticateService = authenticateService;
            _pondService = pondService;
            _urlHelperService = urlHelperService;
            _mapper = mapper;
        }
        // GET: api/koi-care-system/fish/getall
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KoiFish>>> GetAllFish()
        {
            var fishList = await _context.KoiFishes.Include(f => f.Pond).ToListAsync();

            if (fishList == null || !fishList.Any())
            {
                return Ok("No fish records found.");
            }

            return Ok(fishList);
        }

        // GET: api/koi-care-system/fish/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<KoiFish>>> GetByUserId(int userId)
        {
            var fishList = await _context.KoiFishes.Where(f => f.UserId == userId).ToListAsync();
            return Ok(fishList);
        }

        // GET: api/koi-care-system/fish/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<KoiFish>> GetFish(int id)
        {
            var fish = await _context.KoiFishes.Include(f => f.Pond).FirstOrDefaultAsync(f => f.FishId == id);

            if (fish == null)
            {
                return NotFound();
            }

            return Ok(fish);
        }

        // POST: api/koi-care-system/fish
        [HttpPost]
        public async Task<ActionResult<KoiFish>> CreateFish([FromBody] KoiFish fish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.KoiFishes.Add(fish);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFish), new { id = fish.FishId }, fish);
        }

        // PUT: api/koi-care-system/fish/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFish(int id, [FromBody] KoiFish fish)
        {
            if (id != fish.FishId)
            {
                return BadRequest("Fish ID mismatch");
            }

            _context.Entry(fish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FishExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/koi-care-system/fish/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFish(int id)
        {
            var fish = await _context.KoiFishes.FindAsync(id);
            if (fish == null)
            {
                return NotFound();
            }

            _context.KoiFishes.Remove(fish);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FishExists(int id)
        {
            return _context.KoiFishes.Any(e => e.FishId == id);
        }
    }
}
