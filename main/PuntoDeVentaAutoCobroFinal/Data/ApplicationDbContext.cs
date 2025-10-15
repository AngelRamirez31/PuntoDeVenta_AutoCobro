using Microsoft.EntityFrameworkCore;
using PuntoDeVentaAutoCobroFinal.Models;
using System.Collections.Generic;

namespace PuntoDeVentaAutoCobroFinal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Productos { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketItem> TicketItems { get; set; }
    }
}