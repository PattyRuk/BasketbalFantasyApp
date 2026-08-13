using BasketbalFantasyApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketbalFantasyApp.DAL
{
    public class BasketbalFantasyDbContext : IdentityDbContext<IdentityUser>
    {
        public BasketbalFantasyDbContext(DbContextOptions<BasketbalFantasyDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStats> PlayerStats { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentPlayer> TournamentPlayers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<TournamentTeam> TournamentTeams { get; set; }

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
            modelBuilder.Entity<Game>()
                .HasKey(g => g.GameId);
            modelBuilder.Entity<TournamentTeam>()
                .HasKey(tt => new { tt.TournamentId, tt.TeamId });

            // 1.Team - constraints/limits
            modelBuilder.Entity<Team>().Property(t => t.TeamName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Team>().HasIndex(t => t.OwnerUserId).IsUnique();

            // 2.Player - constraints/limits
            modelBuilder.Entity<Player>().Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Player>().Property(p => p.LastName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Player>().Property(p => p.Position).HasMaxLength(50);
            modelBuilder.Entity<Player>().Property(p => p.NbaTeam).HasMaxLength(100);
            modelBuilder.Entity<Player>() // One-to-Many connection with Players
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3.PlayerStats - One-to-Many connection with Players
            modelBuilder.Entity<PlayerStats>() 
                .HasOne(ps => ps.Player)
                .WithMany(p => p.Stats)
                .HasForeignKey(ps => ps.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // 4.Tournament - constraints/limits
            modelBuilder.Entity<Tournament>().Property(t => t.TournamentName).IsRequired().HasMaxLength(150);

            // 5.Tournament Players - Many-to-Many junction tables
            modelBuilder.Entity<TournamentPlayer>() // one tournment with many tournmentplayers(players)
                .HasOne(tp => tp.Tournament)
                .WithMany(t => t.TournamentPlayers)
                .HasForeignKey(tp => tp.TournamentId);

            modelBuilder.Entity<TournamentPlayer>() // one player belongs to many tournmentplayers(tourments)
                .HasOne(tp => tp.Player)
                .WithMany(p => p.TournamentPlayers)
                .HasForeignKey(tp => tp.PlayerId);
            // 6. Game - Many-to-many connections
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Tournament)
                .WithMany(t => t.Games)
                .HasForeignKey(g => g.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.TeamA)
                .WithMany()
                .HasForeignKey(g => g.TeamAId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.TeamB)
                .WithMany()
                .HasForeignKey(g => g.TeamBId)
                .OnDelete(DeleteBehavior.Restrict);
            // 7. PlayerStats - one-to-many connection
            modelBuilder.Entity<PlayerStats>()
                .HasOne(ps => ps.Game)
                .WithMany()
                .HasForeignKey(ps => ps.GameId)
                .OnDelete(DeleteBehavior.Restrict);
            // 8. TournamentTeam
            modelBuilder.Entity<TournamentTeam>()
                .HasMany(tt => tt.RegisteredPlayers)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "TournamentTeamRosterJunction",
                    j => j.HasOne<Player>().WithMany().HasForeignKey("PlayerId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<TournamentTeam>().WithMany().HasForeignKey("TournamentId", "TeamId").OnDelete(DeleteBehavior.Restrict));
        }
    }
}
