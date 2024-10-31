using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Api.Controllers.BaseController;
using AutoMapper;
using KoiCareSystem.Service.Helper;
using KoiCareSystem.Service;
using NETCore.MailKit.Core;

namespace KoiCareSystem.Api.Controllers
{
    public class PondsController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly AuthenticateService _authenticateService;
        private readonly PondService _pondService;
        private readonly IUrlHelperService _urlHelperService;
        private readonly IMapper _mapper;

        public PondsController(ApplicationDbContext context, UserService userService, RoleService roleService, AuthenticateService authenticateService, PondService pondService, IUrlHelperService urlHelperService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _roleService = roleService;
            _authenticateService = authenticateService;
            _pondService = pondService;
            _urlHelperService = urlHelperService;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Pond>>> GetByUserId(int userId)
        {
            var ponds = await _context.Ponds.Where(p => p.UserId == userId).ToListAsync();
            return Ok(ponds);
        }

        // GET: api/Ponds/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Pond>> GetPond(int id)
        {
            var pond = await _context.Ponds.Include(p => p.User).FirstOrDefaultAsync(m => m.PondId == id);

            if (pond == null)
            {
                return NotFound();
            }

            return Ok(pond);
        }

        // POST: api/Ponds
        [HttpPost]
        public async Task<ActionResult<Pond>> CreatePond([FromBody] Pond pond)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Add(pond);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPond), new { id = pond.PondId }, pond);
        }

        // PUT: api/Ponds/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePond(int id, [FromBody] Pond pond)
        {
            if (id != pond.PondId)
            {
                return BadRequest("Pond ID mismatch");
            }

            _context.Entry(pond).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PondExists(id))
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

        // DELETE: api/Ponds/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePond(int id)
        {
            var pond = await _context.Ponds.FindAsync(id);
            if (pond == null)
            {
                return NotFound();
            }

            _context.Ponds.Remove(pond);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PondExists(int id)
        {
            return _context.Ponds.Any(e => e.PondId == id);
        }
    }
}
