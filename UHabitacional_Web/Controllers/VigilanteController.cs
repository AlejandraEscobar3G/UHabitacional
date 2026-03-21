using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UHabitacional_Web.Models;

namespace UHabitacional_Web.Controllers
{
    public class VigilanteController : Controller
    {
        private readonly UHabitacionalContext _context;
        public VigilanteController(UHabitacionalContext context)
        {
            _context = context;
        }

        // GET: VigilanteController
        public async Task<IActionResult> Index()
        {
            List<Usuario> usuarios = await _context.Usuarios
                .Where(u => u.TipoUsuarioId == 3)
                .ToListAsync();

            return View(usuarios);
        }

        // GET: VigilanteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VigilanteController/Create
        public async Task<IActionResult> Create()
        {
            return PartialView("_Create", new Usuario());
        }

        // POST: VigilanteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            try
            {
                usuario.Estatus = EstatusUsuario.Activo;
                usuario.TipoUsuarioId = 3;
                usuario.CreatedAt = DateTime.Now;
                usuario.CreatedBy = 0;

                _context.Add(usuario);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: VigilanteController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Usuario? usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == id && u.TipoUsuarioId == 3);

            return PartialView("_Edit", usuario);
        }

        // POST: VigilanteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            try
            {
                Usuario? usuarioDto = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == id && u.TipoUsuarioId == 3);

                if (usuarioDto == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                usuarioDto.Nombre = usuario.Nombre;
                usuarioDto.ApellidoPaterno = usuario.ApellidoPaterno;
                usuarioDto.ApellidoMaterno = usuario.ApellidoMaterno;
                usuarioDto.Correo = usuario.Correo;
                usuarioDto.Estatus = usuario.Estatus;
                usuarioDto.ModifyAt = DateTime.Now;
                usuarioDto.ModifyBy = 0;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: VigilanteController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Usuario? usuarioDto = await _context.Usuarios
                    .Where(u => u.Id == id && u.TipoUsuarioId == 3)
                    .Include(u => u.TipoUsuario)
                    .FirstOrDefaultAsync();

            return PartialView("_Delete", usuarioDto);
        }

        // POST: VigilanteController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Usuario? usuarioDto = await _context.Usuarios
                    .Where(u => u.Id == id && u.TipoUsuarioId == 3)
                    .Include(u => u.TipoUsuario)
                    .FirstOrDefaultAsync();

                if (usuarioDto == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                usuarioDto.Estatus = EstatusUsuario.Inactivo;
                usuarioDto.ModifyAt = DateTime.Now;
                usuarioDto.ModifyBy = 0;

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
