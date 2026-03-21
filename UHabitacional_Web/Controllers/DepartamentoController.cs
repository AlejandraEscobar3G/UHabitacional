using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UHabitacional_Web.Models;

namespace UHabitacional_Web.Controllers
{
    public class DepartamentoController : Controller
    {
        private readonly UHabitacionalContext _context;
        public DepartamentoController(UHabitacionalContext context)
        {
            _context = context;
        }

        // GET: DepartamentoController
        public async Task<IActionResult> Index()
        {
            List<Departamento> departamentos = await _context.Departamento
                .Include(d => d.Edificio)
                .ToListAsync();

            return View(departamentos);
        }

        // GET: DepartamentoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DepartamentoController/Create
        public async Task<IActionResult> Create()
        {
            List<Edificio> edificios = await _context.Edificio
                .Where(e => e.Departamentos.Count < e.TotalDeptos)
                .ToListAsync();

            ViewBag.Edificios = new SelectList(edificios, "Id", "Id");
            return PartialView("_Create");
        }

        // POST: DepartamentoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Departamento departamento)
        {
            try
            {
                departamento.CreatedAt = DateTime.Now;
                departamento.CreatedBy = 0;

                _context.Add(departamento);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: DepartamentoController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Departamento? departamento = await _context.Departamento
                .Where(d => d.Id == id)
                .Include(d => d.Edificio)
                .FirstOrDefaultAsync();

            if (departamento == null)
            {
                return View(departamento);
            }

            List<Edificio> edificios = await _context.Edificio
                .Where(e => e.Id == departamento.EdificioId || e.Departamentos.Count < e.TotalDeptos)
                .ToListAsync();

            ViewBag.Edificios = new SelectList(edificios, "Id", "Id", departamento.EdificioId);

            return PartialView("_Edit", departamento);
        }

        // POST: DepartamentoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Departamento departamento)
        {
            try
            {
                Departamento? departamentoDto = await _context.Departamento
                .Where(d => d.Id == id)
                .Include(d => d.Edificio)
                .FirstOrDefaultAsync();

                if (departamentoDto == null)
                {
                    return View(departamento);
                }

                departamentoDto.NumeroInt = departamento.NumeroInt;
                departamentoDto.Piso = departamento.Piso;
                departamentoDto.EdificioId = departamento.EdificioId;
                departamentoDto.ModifyAt = DateTime.Now;
                departamentoDto.ModifyBy = 0;

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
