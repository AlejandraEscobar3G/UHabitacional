using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UHabitacional_Web.Models;

namespace UHabitacional_Web.Controllers
{
    public class IdentificacionController : Controller
    {
        private readonly UHabitacionalContext _context;
        public IdentificacionController(UHabitacionalContext context)
        {
            _context = context;
        }

        // GET: IdentificacionController
        public async Task<IActionResult> Index()
        {
            List<Identificacion> identificaciones = await _context.Identificacion
                .ToListAsync();

            return View(identificaciones);
        }

        // GET: IdentificacionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: IdentificacionController/Create
        public async Task<IActionResult> Create()
        {
            return PartialView("_Create", new Identificacion());
        }

        // POST: IdentificacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Identificacion identificacion)
        {
            try
            {
                identificacion.Estatus = EstatusIdentificacion.Activo;
                identificacion.CreatedAt = DateTime.Now;
                identificacion.CreatedBy = 0;

                _context.Identificacion.Add(identificacion);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: IdentificacionController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Identificacion? identificacion = await _context.Identificacion
                .FirstOrDefaultAsync(i => i.Id == id);

            return PartialView("_Edit", identificacion);
        }

        // POST: IdentificacionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Identificacion identificacion)
        {
            try
            {
                Identificacion? identificacionDto = await _context.Identificacion
                .FirstOrDefaultAsync(i => i.Id == id);

                if (identificacionDto == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                identificacionDto.Descripcion = identificacion.Descripcion;
                identificacionDto.ModifyAt = DateTime.Now;
                identificacionDto.ModifyBy = 0;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: IdentificacionController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Identificacion? identificacion = await _context.Identificacion
                .FirstOrDefaultAsync(i => i.Id == id);

            return PartialView("_Delete", identificacion);
        }

        // POST: IdentificacionController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Identificacion? identificacion = await _context.Identificacion
                .FirstOrDefaultAsync(i => i.Id == id);

                if (identificacion == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                identificacion.Estatus = EstatusIdentificacion.Inactivo;
                identificacion.ModifyAt = DateTime.Now;
                identificacion.ModifyBy = 0;

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
