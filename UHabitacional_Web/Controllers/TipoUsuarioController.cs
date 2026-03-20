using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UHabitacional_Web.Models;

namespace UHabitacional_Web.Controllers
{
    public class TipoUsuarioController : Controller
    {
        private readonly UHabitacionalContext _context;
        public TipoUsuarioController(UHabitacionalContext context)
        {
            _context = context;
        }

        // GET: TipoUsuarioController
        public async Task<IActionResult> Index()
        {
            List<TipoUsuario> tipoUsuarios = await _context.TiposUsuario.ToListAsync();

            return View(tipoUsuarios);
        }

        // GET: TipoUsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TipoUsuarioController/Create
        public ActionResult Create()
        {
            return PartialView("_Create", new TipoUsuario());
        }

        // POST: TipoUsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoUsuario tipoUsuario)
        {
            try
            {
                tipoUsuario.Estatus = EstatusTipoUsuario.Activo;
                tipoUsuario.CreatedAt = DateTime.Now;
                tipoUsuario.CreatedBy = 0;

                _context.TiposUsuario.Add(tipoUsuario);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: TipoUsuarioController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            TipoUsuario? tipoUsuario = await _context.TiposUsuario
                .FirstOrDefaultAsync(t => t.Id == id);

            return PartialView("_Edit", tipoUsuario);
        }

        // POST: TipoUsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoUsuario tipoUsuario)
        {
            try
            {
                TipoUsuario? tipoUsuarioDto = await _context.TiposUsuario
                .FirstOrDefaultAsync(t => t.Id == id);

                if (tipoUsuarioDto == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                tipoUsuarioDto.Descripcion = tipoUsuario.Descripcion;
                tipoUsuarioDto.ModifyAt = DateTime.Now;
                tipoUsuarioDto.ModifyBy = 0;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: TipoUsuarioController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            TipoUsuario? tipoUsuarioDto = await _context.TiposUsuario
                .FirstOrDefaultAsync(t => t.Id == id);

            return PartialView("_Delete", tipoUsuarioDto);
        }

        // POST: TipoUsuarioController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                TipoUsuario? tipoUsuarioDto = await _context.TiposUsuario
                .FirstOrDefaultAsync(t => t.Id == id);

                if (tipoUsuarioDto == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                tipoUsuarioDto.Estatus = EstatusTipoUsuario.Inactivo;
                tipoUsuarioDto.ModifyAt = DateTime.Now;
                tipoUsuarioDto.ModifyBy = 0;

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
