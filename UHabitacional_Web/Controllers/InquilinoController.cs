using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UHabitacional_Web.Models;

namespace UHabitacional_Web.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly UHabitacionalContext _context;
        public InquilinoController(UHabitacionalContext context)
        {
            _context = context;
        }

        // GET: InquilinoController
        public async Task<IActionResult> Index()
        {
            List<Inquilino> inquilinos = await _context.Inquilino
                .Include(i => i.Usuario)
                .Include(i => i.Departamento)
                    .ThenInclude(d => d.Edificio)
                .ToListAsync();

            return View(inquilinos);
        }

        // GET: InquilinoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InquilinoController/Create
        public IActionResult Create()
        {
            List<TipoUsuario> tipoUsuarios = _context.TiposUsuario.Where(u => u.Id == 2).ToList();
            List<Edificio> edificiosDisponibles = _context.Edificio
                .Where(e =>
                    e.Departamentos.Any(
                        d => !d.Inquilinos.Any(i => i.FechaFin == null))
                    )
                .ToList();
            ViewBag.TipoUsuarios = new SelectList(tipoUsuarios, "Id", "Descripcion");
            ViewBag.Edificios = new SelectList(edificiosDisponibles, "Id", "Id");
            return View();
        }

        // POST: InquilinoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InquilinoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InquilinoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InquilinoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InquilinoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetDepartamentosByEdificio(string edificioId)
        {
            var departamentos = _context.Departamento
                .Where(d =>
                    d.EdificioId == edificioId
                    && !d.Inquilinos.Any()
                )
                .Select(d => new { id = d.Id, numeroInt = d.NumeroInt })
                .ToList();

            return Json(departamentos);
        }
    }
}
