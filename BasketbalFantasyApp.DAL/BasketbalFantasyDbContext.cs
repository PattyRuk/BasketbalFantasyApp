using BasketbalFantasyApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketbalFantasyApp.DAL
{
    internal class BasketbalFantasyDbContext : DbContext
    {
        public BasketbalFantasyDbContext(DbContextOptions<BasketbalFantasyDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStats> PlayerStats { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentPlayer> TournamentPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Primary Keys
            modelBuilder.Entity<Team>()
                .HasKey(t => t.TeamId);
            modelBuilder.Entity<Player>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<PlayerStats>()
                .HasKey(ps => ps.Id);
            modelBuilder.Entity<Tournament>()
                .HasKey(t => t.TournamentId);
            modelBuilder.Entity<TournamentPlayer>()
                .HasKey(tp => new { tp.TournamentId, tp.PlayerId });

            // 1.Team - constraints/limits
            modelBuilder.Entity<Team>().Property(t => t.TeamName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Team>().HasIndex(t => t.OwnerUserId).IsUnique();

            // 2.Player - constraints/limits

        }
    }
}
