using Microsoft.EntityFrameworkCore;
using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Infrastructure.Contexts
{
    public class UHabitacionalContext : DbContext
    {
        public UHabitacionalContext(DbContextOptions options) : base(options) { }

        // DBSets = Tablas en la Base de Datos
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<Inquilino> Inquilino { get; set; }
        public DbSet<Edificio> Edificio { get; set; }
        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<Identificacion> Identificacion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*
             * ******************************
             * * CONFIGURACION DE ENTIDADES *
             * ******************************
             */

            // TABLA TIPO_USUARIO
            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Descripcion).IsRequired().HasMaxLength(50);

                // Campos auditoria
                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("SYSDATETIME()")
                    .ValueGeneratedOnAdd();
                entity.Property(u => u.CreatedBy)
                    .HasDefaultValue(0)
                    .ValueGeneratedOnAdd();
            });

            // TABLA USUARIO
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(150);
                entity.Property(u => u.ApellidoPaterno).IsRequired().HasMaxLength(150);
                entity.Property(u => u.ApellidoMaterno).HasMaxLength(150);
                entity.Property(u => u.Correo).HasMaxLength(150);
                entity.Property(u => u.Estatus).IsRequired();
                entity.Property(u => u.TipoUsuarioId).IsRequired();

                // Campos auditoria
                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("SYSDATETIME()")
                    .ValueGeneratedOnAdd();
                entity.Property(u => u.CreatedBy)
                    .HasDefaultValue(0)
                    .ValueGeneratedOnAdd();

                // Relaciones
                entity
                    .HasOne(u => u.TipoUsuario)
                    .WithMany(t => t.Usuarios)
                    .HasForeignKey(u => u.TipoUsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(u => u.Inquilino)
                    .WithOne(i => i.Usuario)
                    .HasForeignKey<Inquilino>(i => i.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TABLA IDENTIFICACION
            modelBuilder.Entity<Identificacion>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Descripcion).IsRequired().HasMaxLength(150);
                entity.Property(u => u.Estatus).IsRequired().HasDefaultValue(EstatusIdentificacion.Activo);

                // Campos auditoria
                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("SYSDATETIME()")
                    .ValueGeneratedOnAdd();
                entity.Property(u => u.CreatedBy)
                    .HasDefaultValue(0)
                    .ValueGeneratedOnAdd();
            });

            // TABLA EDIFICIO
            modelBuilder.Entity<Edificio>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(e => e.Calle).HasMaxLength(150);
                entity.Property(e => e.TotalDeptos).IsRequired();
                entity.Property(e => e.NumeroPisos).IsRequired();

                entity.Property(e => e.Estatus)
                    .HasConversion<int>()
                    .IsRequired()
                    .HasDefaultValue(EstatusEdificio.Activo);

                // Campos auditoria
                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("SYSDATETIME()")
                    .ValueGeneratedOnAdd();
                entity.Property(u => u.CreatedBy)
                    .HasDefaultValue(0)
                    .ValueGeneratedOnAdd();
            });

            // TABLA DEPARTAMENTO
            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.NumeroInt).HasMaxLength(20);

                // Campos auditoria
                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("SYSDATETIME()")
                    .ValueGeneratedOnAdd();
                entity.Property(u => u.CreatedBy)
                    .HasDefaultValue(0)
                    .ValueGeneratedOnAdd();

                // Relaciones
                entity
                    .HasOne(d => d.Edificio)
                    .WithMany(e => e.Departamentos)
                    .HasForeignKey(d => d.EdificioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasMany(d => d.Inquilinos)
                    .WithOne(i => i.Departamento)
                    .HasForeignKey(i => i.DepartamentoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasMany(d => d.BitacoraVisitantes)
                    .WithOne(b => b.Departamento)
                    .HasForeignKey(b => b.DepartamentoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TABLA BITACORA_VISITANTE
            modelBuilder.Entity<BitacoraVisitante>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(150);
                entity.Property(u => u.ApellidoPaterno).IsRequired().HasMaxLength(150);
                entity.Property(u => u.ApellidoMaterno).HasMaxLength(150);
                entity.Property(u => u.Correo).HasMaxLength(150);
                entity.Property(u => u.Estatus).IsRequired();
                entity.Property(u => u.codigoVisita).IsRequired();

                // Campos auditoria
                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("SYSDATETIME()")
                    .ValueGeneratedOnAdd();
                entity.Property(u => u.CreatedBy)
                    .HasDefaultValue(0)
                    .ValueGeneratedOnAdd();

                // Relaciones
                entity
                    .HasOne(d => d.Identificacion)
                    .WithMany(e => e.BitacoraVisitantes)
                    .HasForeignKey(d => d.IdentificacionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TABLA BITACORA_VIGILANTE
            modelBuilder.Entity<BitacoraVigilante>(entity =>
            {
                entity.HasKey(u => u.Id);

                // Campos auditoria
                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("SYSDATETIME()")
                    .ValueGeneratedOnAdd();
                entity.Property(u => u.CreatedBy)
                    .HasDefaultValue(0)
                    .ValueGeneratedOnAdd();

                // Relaciones
                entity
                    .HasOne(d => d.Usuario)
                    .WithMany(e => e.BitacoraVigilantes)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            // TABLA INQUILINO
            modelBuilder.Entity<Inquilino>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FechaInicio).IsRequired();

                // Campos auditoria
                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("SYSDATETIME()")
                    .ValueGeneratedOnAdd();
                entity.Property(u => u.CreatedBy)
                    .HasDefaultValue(0)
                    .ValueGeneratedOnAdd();
            });
        }
    }
}
