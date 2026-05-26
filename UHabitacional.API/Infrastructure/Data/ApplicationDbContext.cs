using Microsoft.EntityFrameworkCore;
using UHabitacional.API.Domain.Entities;

namespace UHabitacional.API.Infrastructure.Data;

/// <summary>
/// Contexto de Entity Framework para el sistema UHabitacional.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Identificacion> Identificaciones => Set<Identificacion>();
    public DbSet<TipoUsuario> TiposUsuario => Set<TipoUsuario>();
    public DbSet<Edificio> Edificios => Set<Edificio>();
    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Inquilino> Inquilinos => Set<Inquilino>();
    public DbSet<BitacoraVigilante> BitacorasVigilante => Set<BitacoraVigilante>();
    public DbSet<BitacoraVisitante> BitacorasVisitante => Set<BitacoraVisitante>();
    public DbSet<Sesion> Sesiones => Set<Sesion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------- Identificacion ----------
        modelBuilder.Entity<Identificacion>(entity =>
        {
            entity.HasIndex(e => e.Nombre).IsUnique();
            entity.HasQueryFilter(e => e.Activo);
        });

        // ---------- TipoUsuario ----------
        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasIndex(e => e.Nombre).IsUnique();
            entity.HasQueryFilter(e => e.Activo);
        });

        // ---------- Edificio ----------
        modelBuilder.Entity<Edificio>(entity =>
        {
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        // ---------- Departamento ----------
        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasOne(d => d.Edificio)
                  .WithMany(e => e.Departamentos)
                  .HasForeignKey(d => d.IdEdificio)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(d => new { d.IdEdificio, d.NumeroDepartamento }).IsUnique();
        });

        // ---------- Usuario ----------
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasOne(u => u.TipoUsuario)
                  .WithMany(t => t.Usuarios)
                  .HasForeignKey(u => u.IdTipoUsuario)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Identificacion)
                  .WithMany(i => i.Usuarios)
                  .HasForeignKey(u => u.IdIdentificacion)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => new { u.IdIdentificacion, u.NumeroIdentificacion }).IsUnique();

            entity.HasQueryFilter(u => u.Activo);
        });

        // ---------- Inquilino ----------
        modelBuilder.Entity<Inquilino>(entity =>
        {
            entity.HasOne(i => i.Usuario)
                  .WithOne(u => u.Inquilino)
                  .HasForeignKey<Inquilino>(i => i.IdUsuario)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.Departamento)
                  .WithMany(d => d.Inquilinos)
                  .HasForeignKey(i => i.IdDepartamento)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(i => i.IdUsuario).IsUnique();

            entity.HasQueryFilter(i => i.Activo);
        });

        // ---------- BitacoraVigilante ----------
        modelBuilder.Entity<BitacoraVigilante>(entity =>
        {
            entity.HasOne(b => b.Usuario)
                  .WithMany(u => u.BitacorasVigilante)
                  .HasForeignKey(b => b.IdUsuario)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- BitacoraVisitante ----------
        modelBuilder.Entity<BitacoraVisitante>(entity =>
        {
            entity.HasOne(b => b.Inquilino)
                  .WithMany(i => i.BitacorasVisitante)
                  .HasForeignKey(b => b.IdInquilino)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Identificacion)
                  .WithMany(i => i.BitacorasVisitante)
                  .HasForeignKey(b => b.IdIdentificacion)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.VigilanteEntrada)
                  .WithMany()
                  .HasForeignKey(b => b.IdVigilanteEntrada)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.VigilanteSalida)
                  .WithMany()
                  .HasForeignKey(b => b.IdVigilanteSalida)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(b => b.CodigoVisita);

            entity.HasQueryFilter(b => b.Activo);
        });

        // ---------- Sesion ----------
        modelBuilder.Entity<Sesion>(entity =>
        {
            entity.HasOne(s => s.Usuario)
                  .WithMany(u => u.Sesiones)
                  .HasForeignKey(s => s.IdUsuario)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(s => s.Jti).IsUnique();
            entity.HasIndex(s => new { s.IdUsuario, s.Activa });
        });
    }
}
