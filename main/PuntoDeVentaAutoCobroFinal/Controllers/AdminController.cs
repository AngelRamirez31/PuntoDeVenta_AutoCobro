using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PuntoDeVentaAutoCobroFinal.Data;
using PuntoDeVentaAutoCobroFinal.Models;

namespace PuntoDeVentaAutoCobroFinal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Products()
        {
            var products = await _context.Productos.OrderBy(p => p.Nombre).ToListAsync();
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Productos.AnyAsync(p => p.CodigoDeBarras == product.CodigoDeBarras))
                {
                    TempData["ErrorMessage"] = "Ya existe un producto con ese código de barras.";
                }
                else
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Producto agregado exitosamente.";
                }
                return RedirectToAction(nameof(Products));
            }
            var products = await _context.Productos.OrderBy(p => p.Nombre).ToListAsync();
            return View("Products", products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Productos.FindAsync(id);
            if (product != null)
            {
                _context.Productos.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Producto eliminado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se encontró el producto a eliminar.";
            }
            return RedirectToAction(nameof(Products));
        }

        // --- BÚSQUEDA DE TICKETS ---
        public async Task<IActionResult> SearchTicket(int? id)
        {
            if (id == null)
            {
                return View("SearchTicket", null);
            }

            var ticket = await _context.Tickets
                .Include(t => t.Items)
                .ThenInclude(ti => ti.Product)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                TempData["ErrorMessage"] = $"No se encontró el ticket con ID: {id}";
                return View("SearchTicket", null);
            }
            return View("SearchTicket", ticket);
        }

        // --- REPORTES ---
        public async Task<IActionResult> Reports(string timeFrame = "daily")
        {
            DateTime startDate;
            DateTime endDate = DateTime.Now;

            if (timeFrame == "monthly")
            {
                startDate = new DateTime(endDate.Year, endDate.Month, 1);
                ViewData["ReportTitle"] = "Reporte Mensual";
            }
            else
            {
                startDate = DateTime.Today;
                ViewData["ReportTitle"] = "Reporte Diario";
            }

            var ticketsInPeriod = _context.Tickets.Where(t => t.Fecha >= startDate && t.Fecha <= endDate);

            ViewBag.TotalSales = await ticketsInPeriod.SumAsync(t => t.Total);
            ViewBag.SalesByPaymentMethod = await ticketsInPeriod
                .GroupBy(t => t.MetodoPago)
                .Select(g => new { Method = g.Key, Total = g.Sum(t => t.Total) })
                .ToListAsync();
            ViewBag.TopProducts = await _context.TicketItems
                .Where(ti => ti.Ticket.Fecha >= startDate && ti.Ticket.Fecha <= endDate)
                .GroupBy(ti => ti.Product.Nombre)
                .Select(g => new { ProductName = g.Key, TotalSold = g.Sum(ti => ti.Cantidad) })
                .OrderByDescending(x => x.TotalSold)
                .Take(10)
                .ToListAsync();
            ViewBag.TopCategories = await _context.TicketItems
                .Where(ti => ti.Ticket.Fecha >= startDate && ti.Ticket.Fecha <= endDate)
                .GroupBy(ti => ti.Product.Categoria)
                .Select(g => new { CategoryName = g.Key, TotalSold = g.Sum(ti => ti.Cantidad) })
                .OrderByDescending(x => x.TotalSold)
                .Take(3)
                .ToListAsync();

            return View();
        }
    }
}