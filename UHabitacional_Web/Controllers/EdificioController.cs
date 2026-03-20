using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using UHabitacional_Web.Models;

namespace UHabitacional_Web.Controllers
{
    public class EdificioController : Controller
    {
        private readonly UHabitacionalContext _context;
        public EdificioController(UHabitacionalContext context)
        {
            _context = context;
        }

        // GET: EdificioController
        public async Task<IActionResult> Index()
        {
            List<Edificio> edificios = await _context.Edificio.ToListAsync();

            return View(edificios);
        }

        // GET: EdificioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EdificioController/Create
        public async Task<IActionResult> Create()
        {
            return PartialView("_Create", new Edificio());
        }

        // POST: EdificioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Edificio edificio)
        {
            try
            {
                edificio.CreatedAt = DateTime.Now;
                edificio.CreatedBy = 0;

                _context.Edificio.Add(edificio);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: EdificioController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            Edificio? edificio = await _context.Edificio
                .FirstOrDefaultAsync(e => e.Id == id);

            return PartialView("_Edit", edificio);
        }

        // POST: EdificioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Edificio edificio)
        {
            try
            {
                Edificio? edificioDto = await _context.Edificio
                .FirstOrDefaultAsync(e => e.Id == id);

                if (edificioDto == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                edificioDto.Estatus = edificio.Estatus;
                edificioDto.Calle = edificio.Calle;
                edificioDto.NumeroPisos = edificio.NumeroPisos;
                edificioDto.TotalDeptos = edificio.TotalDeptos;
                edificioDto.ModifyAt = DateTime.Now;
                edificioDto.ModifyBy = 0;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
