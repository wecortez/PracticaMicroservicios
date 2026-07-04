using ItemsTrabajo.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemsTrabajo.Api.Data;

public partial class BdItemsTrabajoContext : DbContext
{
    public BdItemsTrabajoContext()
    {
    }

    public BdItemsTrabajoContext(DbContextOptions<BdItemsTrabajoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ItemTrabajo> ItemsTrabajos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=BD_ItemsTrabajo;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemTrabajo>(entity =>
        {
            entity.HasKey(e => e.IdItemTrabajo).HasName("PK__ItemsTra__02499A4C6F4DDEB5");

            entity.ToTable("ItemsTrabajo");

            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaEntrega).HasColumnType("datetime");
            entity.Property(e => e.Titulo).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
