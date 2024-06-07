﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GeneradorDeExamenes.Entidades;

public partial class ExamenIAContext : DbContext
{
    public ExamenIAContext()
    {
    }

    public ExamenIAContext(DbContextOptions<ExamenIAContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Examan> Examen { get; set; }

    public virtual DbSet<Preguntum> Pregunta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-1N3OSRL\\SQLEXPRESS;Database=ExamenIA;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Examan>(entity =>
        {
            entity.HasKey(e => e.IdExamen).HasName("PK__Examen__0E8DC9BEFC552D88");
        });

        modelBuilder.Entity<Preguntum>(entity =>
        {
            entity.HasKey(e => e.IdPregunta).HasName("PK__Pregunta__754EC09E3DB19625");

            entity.HasOne(d => d.IdExamenNavigation).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.IdExamen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pregunta__IdExam__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
