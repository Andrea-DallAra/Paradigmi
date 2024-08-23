using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Paradigmi.Classi;

namespace Paradigmi.Dati
{
    public class DC : DbContext
    {
        public DC(DbContextOptions<DC> options) : base(options)
        {
        }

        public DbSet<Utente> utente { get; set; }
        public DbSet<Risorsa> risorse { get; set; }
        public DbSet<Booking> bookings { get; set; }

        public  bool TestConnection()
        {
            try
            {
                return this.Database.CanConnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                return false;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Utente>().ToTable("utente", "dbo");
            modelBuilder.Entity<Utente>().HasIndex(u => u.email).IsUnique();

           
            modelBuilder.Entity<Utente>()
                .HasMany(u => u.bookings)
                 .WithOne(b => b.utente)
                 .HasForeignKey(b => b.idUtente);

           
            modelBuilder.Entity<Risorsa>()
                .HasMany(r => r.booking)
              .WithOne(b => b.risorsa)
               .HasForeignKey(b => b.idRisorsa);


            base.OnModelCreating(modelBuilder);
        }
    }
}
