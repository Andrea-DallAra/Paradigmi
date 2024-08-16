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

        public DbSet<Utente> utenti { get; set; }
        public DbSet<Risorsa> risorse { get; set; }
        public DbSet<Booking> bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure email is unique
            modelBuilder.Entity<Utente>().HasIndex(u => u.getEmail()).IsUnique();

            // One-to-many relationship between User and Booking
            modelBuilder.Entity<Utente>()
                .HasMany(u => u.getBookings())
                 .WithOne(b => b.GetUtente())
                 .HasForeignKey(b => b.GetIdUtente());

            // One-to-many relationship between Resource and Booking
            modelBuilder.Entity<Risorsa>()
                .HasMany(r => r.GetBooking())
              .WithOne(b => b.GetRisorsa())
               .HasForeignKey(b => b.GetIdRisorsa());


            base.OnModelCreating(modelBuilder);
        }
    }
}
