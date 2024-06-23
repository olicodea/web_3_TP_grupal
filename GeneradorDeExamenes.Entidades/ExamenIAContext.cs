using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GeneradorDeExamenes.Entidades;

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
    public virtual DbSet<Categoria> Categoria { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Examan>(entity =>
        {
            entity.HasKey(e => e.IdExamen).HasName("PK__Examen__0E8DC9BE4AB36D21");
            entity.HasOne(d => d.Categoria)
                      .WithMany(p => p.Examenes)
                      .HasForeignKey(d => d.IdCategoria)
                      .HasConstraintName("FK__Examen__IdCatego__5EBF139D");
        });

        modelBuilder.Entity<Preguntum>(entity =>
        {
            entity.HasKey(e => e.IdPregunta).HasName("PK__Pregunta__754EC09E6A00DE24");

            entity.HasOne(d => d.IdExamenNavigation).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.IdExamen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pregunta__IdExam__5FB337D6");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categoria__19093A0B4B7734FF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
