using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GeneradorDeExamenes.Web;

public partial class ExamenIaContext : DbContext
{
    public ExamenIaContext()
    {
    }

    public ExamenIaContext(DbContextOptions<ExamenIaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Examan> Examen { get; set; }

    public virtual DbSet<Preguntum> Pregunta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-1N3OSRL\\SQLEXPRESS;Database=ExamenIA;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Examan>(entity =>
        {
            entity.HasKey(e => e.IdExamen).HasName("PK__Examen__0E8DC9BE4AB36D21");
        });

        modelBuilder.Entity<Preguntum>(entity =>
        {
            entity.HasKey(e => e.IdPregunta).HasName("PK__Pregunta__754EC09E6A00DE24");

            entity.HasOne(d => d.IdExamenNavigation).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.IdExamen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pregunta__IdExam__5FB337D6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
