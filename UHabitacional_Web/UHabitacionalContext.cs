using Microsoft.EntityFrameworkCore;
using UHabitacional_Web.Models;

namespace UHabitacional_Web
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

            /*
             * *****************************
             * * SEED PARA TABLAS CATALOGO *
             * *****************************
             */

            // Seed para tipos de usuario
            modelBuilder.Entity<TipoUsuario>().HasData(
                new TipoUsuario { Id = 1, Descripcion = "Administrador" },
                new TipoUsuario { Id = 2, Descripcion = "Inquilino" },
                new TipoUsuario { Id = 3, Descripcion = "Vigilante" }
            );

            // Seed para identificaciones
            modelBuilder.Entity<Identificacion>().HasData(
                new Identificacion { Id = 1, Descripcion = "INE" },
                new Identificacion { Id = 2, Descripcion = "Pasaporte" },
                new Identificacion { Id = 3, Descripcion = "Licencia de conducir" }
            );

            // Seed para edificios
            modelBuilder.Entity<Edificio>().HasData(
                new Edificio { Id = "1-1", Calle = "Av. Chimalhuacán", TotalDeptos = 12, NumeroPisos = 6 },
                new Edificio { Id = "1-2", Calle = "Calle Pantitlán", TotalDeptos = 8, NumeroPisos = 4 },
                new Edificio { Id = "1-3", Calle = "Av. Bordo de Xochiaca", TotalDeptos = 15, NumeroPisos = 7 },
                new Edificio { Id = "1-4", Calle = "Calle Sor Juana Inés de la Cruz", TotalDeptos = 10, NumeroPisos = 5 },
                new Edificio { Id = "1-5", Calle = "Av. Adolfo López Mateos", TotalDeptos = 20, NumeroPisos = 10 }
            );

            // Seed para departamentos
            modelBuilder.Entity<Departamento>().HasData(
                new Departamento { Id = 1, NumeroInt = "101", Piso = 1, EdificioId = "1-1" },
                new Departamento { Id = 2, NumeroInt = "102", Piso = 1, EdificioId = "1-1" },
                new Departamento { Id = 3, NumeroInt = "201", Piso = 2, EdificioId = "1-2" }
            );
        }
    }
}
