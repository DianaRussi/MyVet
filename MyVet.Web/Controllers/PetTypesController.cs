using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVet.Web.Data;
using MyVet.Web.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PetTypesController : Controller
    {
        private readonly DataContext _context;

        public PetTypesController(DataContext context)
        {
            _context = context;
        }

        // GET: PetTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PetTypes.ToListAsync());
        }

        // GET: PetTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petType = await _context.PetTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (petType == null)
            {
                return NotFound();
            }

            return View(petType);
        }

        // GET: PetTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PetTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PetType petType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(petType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(petType);
        }

        // GET: PetTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petType = await _context.PetTypes.FindAsync(id);
            if (petType == null)
            {
                return NotFound();
            }
            return View(petType);
        }

        // POST: PetTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] PetType petType)
        //public async Task<IActionResult> Edit(PetType petType)
        {
            if (id != petType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetTypeExists(petType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(petType);
        }

        // GET: PetTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petType = await _context.PetTypes
                .Include(pt => pt.Pets)
                .FirstOrDefaultAsync(pt => pt.Id == id);
            if (petType == null)
            {
                return NotFound();
            }
            if(petType.Pets.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "El tipo de mascota no puede ser borrado.");
                return RedirectToAction(nameof(Index));
            }
            _context.PetTypes.Remove(petType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetTypeExists(int id)
        {
            return _context.PetTypes.Any(e => e.Id == id);
        }
    }
}