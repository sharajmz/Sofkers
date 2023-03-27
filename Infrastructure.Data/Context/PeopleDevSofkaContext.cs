using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Data.Context
{
    public partial class PeopleDevSofkaContext : DbContext
    {
        public PeopleDevSofkaContext()
        {
        }

        public PeopleDevSofkaContext(DbContextOptions<PeopleDevSofkaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<IdentificationType> IdentificationTypes { get; set; } = null!;
        public virtual DbSet<Sofker> Sofkers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=tcp:peopledevsofka-server.database.windows.net,1433;Initial Catalog=PeopleDevSofka;Persist Security Info=False;User ID=projectadmin;Password=vHuFu3lrOC57APCmMieV;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");

                entity.Property(e => e.ClientId).HasColumnName("clientId");

                entity.Property(e => e.ClientName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("clientName");
            });

            modelBuilder.Entity<IdentificationType>(entity =>
            {
                entity.HasKey(e => e.IdentificationId);

                entity.ToTable("IdentificationType");

                entity.Property(e => e.IdentificationId).HasColumnName("identificationId");

                entity.Property(e => e.Identification)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("identification");
            });

            modelBuilder.Entity<Sofker>(entity =>
            {
                entity.HasKey(e => new { e.SofkerTypeId, e.SofkerId });

                entity.ToTable("Sofker");

                entity.Property(e => e.SofkerTypeId).HasColumnName("sofkerTypeId");

                entity.Property(e => e.SofkerId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sofkerId");

                entity.Property(e => e.SofkerActive).HasColumnName("sofkerActive");

                entity.Property(e => e.SofkerAddress)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("sofkerAddress");

                entity.Property(e => e.SofkerClient).HasColumnName("sofkerClient");

                entity.Property(e => e.SofkerName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("sofkerName");

                entity.HasOne(d => d.SofkerClientNavigation)
                    .WithMany(p => p.Sofkers)
                    .HasForeignKey(d => d.SofkerClient)
                    .HasConstraintName("FK_SofkerClient");

                entity.HasOne(d => d.SofkerType)
                    .WithMany(p => p.Sofkers)
                    .HasForeignKey(d => d.SofkerTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SofkerTypeId");

                entity.Navigation(sofker => sofker.SofkerType).AutoInclude();

                entity.Navigation(sofker => sofker.SofkerClientNavigation).AutoInclude();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
