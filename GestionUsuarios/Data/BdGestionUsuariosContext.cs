using System;
using System.Collections.Generic;
using GestionUsuarios.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionUsuarios.Api.Data;

public partial class BdGestionUsuariosContext : DbContext
{
    // Inicializa una instancia vacía del contexto de gestión de usuarios.
    public BdGestionUsuariosContext()
    {
    }

    // Inicializa el contexto de gestión de usuarios con opciones de configuración externas.
    public BdGestionUsuariosContext(DbContextOptions<BdGestionUsuariosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    // Configura la conexión a la base de datos cuando no se recibe desde la inyección de dependencias.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=BD_GestionUsuarios;Trusted_Connection=True;TrustServerCertificate=True;");

    // Configura el mapeo de la entidad Usuario con la tabla y sus columnas.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF978E491B22");

            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.EstaActivo).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    // Permite extender la configuración del modelo desde otra definición parcial.
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
