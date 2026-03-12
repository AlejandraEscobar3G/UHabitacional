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
        public async Task<IActionResult> Details(int id)
        {
            Inquilino? inquilino = await _context.Inquilino
                .Where(i => i.UsuarioId == id)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.TipoUsuario)
                .Include(i => i.Departamento)
                    .ThenInclude(d => d.Edificio)
                .FirstOrDefaultAsync();

            return PartialView("Details", inquilino);
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
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                inquilino.Usuario.Estatus = EstatusUsuario.Activo;
                inquilino.Usuario.CreatedAt = DateTime.Now;
                inquilino.Usuario.CreatedBy = 0;

                _context.Usuarios.Add(inquilino.Usuario);
                _context.SaveChanges();

                Inquilino inquilinoDto = new Inquilino()
                {
                    FechaInicio = inquilino.FechaInicio,
                    UsuarioId = inquilino.Usuario.Id,
                    DepartamentoId = inquilino.DepartamentoId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = 0
                };

                _context.Inquilino.Add(inquilinoDto);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: InquilinoController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Inquilino? inquilino = await _context.Inquilino
                .Where(i => i.UsuarioId == id)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.TipoUsuario)
                .Include(i => i.Departamento)
                    .ThenInclude(d => d.Edificio)
                .FirstOrDefaultAsync();

            if (inquilino == null)
            {
                return View(inquilino);
            }

            List<TipoUsuario> tipoUsuarios = _context.TiposUsuario.Where(u => u.Id == 2).ToList();
            List<Edificio> edificiosDisponibles = _context.Edificio
                .Where(e =>
                    e.Id == inquilino.Departamento.EdificioId
                    || e.Departamentos.Any(
                        d => !d.Inquilinos.Any(i => i.FechaFin == null))
                    )
                .ToList();
            var departamentos = _context.Departamento
                .Where(d =>
                    d.EdificioId == inquilino.Departamento.EdificioId
                    && (
                        d.Id == inquilino.DepartamentoId
                        || !d.Inquilinos.Any(i => i.FechaFin == null)
                    )
                )
                .Select(d => new { id = d.Id, numeroInt = d.NumeroInt })
                .ToList();

            ViewBag.TipoUsuarios = new SelectList(tipoUsuarios, "Id", "Descripcion", inquilino.Usuario.TipoUsuarioId);
            ViewBag.Edificios = new SelectList(edificiosDisponibles, "Id", "Id", inquilino.Departamento.EdificioId);
            ViewBag.Departamentos = new SelectList(departamentos, "id", "numeroInt", inquilino.DepartamentoId);

            return View(inquilino);
        }

        // POST: InquilinoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inquilino inquilino)
        {
            try
            {
                Inquilino? inquilinoDto = await _context.Inquilino
                .Where(i => i.UsuarioId == id)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.TipoUsuario)
                .Include(i => i.Departamento)
                    .ThenInclude(d => d.Edificio)
                .FirstOrDefaultAsync();

                if (inquilinoDto == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                // Actualiza tabla Usuario
                inquilinoDto.Usuario.Nombre = inquilino.Usuario.Nombre;
                inquilinoDto.Usuario.ApellidoPaterno = inquilino.Usuario.ApellidoPaterno;
                inquilinoDto.Usuario.ApellidoMaterno = inquilino.Usuario.ApellidoMaterno;
                inquilinoDto.Usuario.Correo = inquilino.Usuario.Correo;
                inquilinoDto.Usuario.TipoUsuarioId = inquilino.Usuario.TipoUsuarioId;
                inquilinoDto.Usuario.Estatus = inquilino.Usuario.Estatus;
                inquilinoDto.Usuario.ModifyAt = DateTime.Now;
                inquilinoDto.Usuario.ModifyBy = 0;

                // Actualiza tabla Inquilino
                inquilinoDto.FechaInicio = inquilino.FechaInicio;
                inquilinoDto.DepartamentoId = inquilino.DepartamentoId;
                inquilinoDto.ModifyAt = DateTime.Now;
                inquilinoDto.ModifyBy = 0;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InquilinoController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Inquilino? inquilino = await _context.Inquilino
                .Where(i => i.UsuarioId == id)
                .Include(i => i.Usuario)
                    .ThenInclude(t => t.TipoUsuario)
                .Include(i => i.Departamento)
                    .ThenInclude(d => d.Edificio)
                .FirstOrDefaultAsync();

            return PartialView("_Delete", inquilino);
        }

        // POST: InquilinoController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Inquilino? inquilino = await _context.Inquilino
                    .Where(i => i.UsuarioId == id)
                    .Include(i => i.Usuario)
                    .FirstOrDefaultAsync();

                if (inquilino == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                inquilino.Usuario.Estatus = EstatusUsuario.Inactivo;
                inquilino.FechaFin = DateTime.Now;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public JsonResult GetDepartamentosByEdificio(string edificioId)
        {
            var departamentos = _context.Departamento
                .Where(d =>
                    d.EdificioId == edificioId
                    && !d.Inquilinos.Any(i => i.FechaFin == null)
                )
                .Select(d => new { id = d.Id, numeroInt = d.NumeroInt })
                .ToList();

            return Json(departamentos);
        }
    }
}
