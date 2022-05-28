using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DBSnooker.Models
{
    public partial class SnookerDataBaseContext : DbContext
    {
        public SnookerDataBaseContext()
        {
        }

        public SnookerDataBaseContext(DbContextOptions<SnookerDataBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<GameResult> GameResults { get; set; } = null!;
        public virtual DbSet<Player> Players { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=C:\\Users\\User\\Desktop\\study\\visprog\\DBSnooker\\DBSnooker\\Assets\\SnookerDataBase.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.EventId)
                    .ValueGeneratedNever()
                    .HasColumnName("Event Id");

                entity.Property(e => e.EventCountry).HasColumnName("Event Country");

                entity.Property(e => e.EventName).HasColumnName("Event Name");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.GameId)
                    .ValueGeneratedNever()
                    .HasColumnName("Game Id");

                entity.Property(e => e.EventId).HasColumnName("Event Id");

                entity.Property(e => e.PlayerName).HasColumnName("Player Name");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.EventId);
            });

            modelBuilder.Entity<GameResult>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Game Result");

                entity.Property(e => e.GameId).HasColumnName("Game Id");

                entity.HasOne(d => d.Game)
                    .WithMany()
                    .HasForeignKey(d => d.GameId);
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.PlayerName });

                entity.ToTable("Player");

                entity.Property(e => e.PlayerId).HasColumnName("Player Id");

                entity.Property(e => e.PlayerName).HasColumnName("Player Name");

                entity.Property(e => e.PlayerCountry).HasColumnName("Player Country");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
