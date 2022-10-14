﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EduPlay.DAL.Entities
{
    public partial class EduPlayContext : DbContext
    {
        public EduPlayContext()
        {
        }

        public EduPlayContext(DbContextOptions<EduPlayContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Difficulties> Difficulties { get; set; }
        public virtual DbSet<Games> Games { get; set; }
        public virtual DbSet<Themes> Themes { get; set; }
        public virtual DbSet<UserGameRecords> UserGameRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=EduPlay;Username=postgres;Password=159hawk");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Difficulties>(entity =>
            {
                entity.ToTable("difficulties");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<Games>(entity =>
            {
                entity.ToTable("games");

                entity.HasIndex(e => e.DifficultyId);

                entity.HasIndex(e => e.ThemeId);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DifficultyId).HasColumnName("difficulty_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(70);

                entity.Property(e => e.ThemeId).HasColumnName("theme_id");

                entity.HasOne(d => d.Difficulty)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.DifficultyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_gamedifficulty");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.ThemeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_gametheme");
            });

            modelBuilder.Entity<Themes>(entity =>
            {
                entity.ToTable("themes");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<UserGameRecords>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.UserGameRecords)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_GameRecord");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGameRecords)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserRecord");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}