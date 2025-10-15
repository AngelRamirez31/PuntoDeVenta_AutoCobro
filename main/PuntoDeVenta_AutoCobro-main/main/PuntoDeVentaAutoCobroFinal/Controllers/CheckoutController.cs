using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PuntoDeVentaAutoCobroFinal.Data;
using PuntoDeVentaAutoCobroFinal.Helpers;
using PuntoDeVentaAutoCobroFinal.Models;
using PuntoDeVentaAutoCobroFinal.ViewModels;

namespace PuntoDeVentaAutoCobroFinal.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "ShoppingCart";

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<ShoppingCartViewModel>(HttpContext.Session, CartSessionKey) ?? new ShoppingCartViewModel();
            return View(cart);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddItem(long barcode)
        {
            var product = await _context.Productos.FirstOrDefaultAsync(p => p.CodigoDeBarras == barcode);
            var cart = SessionHelper.GetObjectFromJson<ShoppingCartViewModel>(HttpContext.Session, CartSessionKey) ?? new ShoppingCartViewModel();

            if (product != null)
            {
                var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);
                if (cartItem != null)
                {
                    cartItem.Cantidad++;
                }
                else
                {
                    cart.Items.Add(new CartItem
                    {
                        ProductId = product.Id,
                        Nombre = product.Nombre,
                        Precio = product.Precio,
                        Cantidad = 1,
                        CodigoDeBarras = product.CodigoDeBarras
                    });
                }
                TempData["SuccessMessage"] = $"'{product.Nombre}' agregado al carrito.";
            }
            else
            {
                TempData["ErrorMessage"] = "Producto no encontrado.";
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, CartSessionKey, cart);
            return RedirectToAction("Index");
        }

        
        public IActionResult RemoveItem(int productId)
        {
            var cart = SessionHelper.GetObjectFromJson<ShoppingCartViewModel>(HttpContext.Session, CartSessionKey);
            if (cart != null)
            {
                var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (itemToRemove != null)
                {
                    cart.Items.Remove(itemToRemove);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, CartSessionKey, cart);
                    TempData["SuccessMessage"] = "Producto eliminado del carrito.";
                }
            }
            return RedirectToAction("Index");
        }

        
        public IActionResult Cancel()
        {
            HttpContext.Session.Remove(CartSessionKey);
            TempData["SuccessMessage"] = "Compra cancelada.";
            return RedirectToAction("Index", "Home");
        }

        
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(string paymentMethod, decimal amountReceived = 0)
        {
            var cart = SessionHelper.GetObjectFromJson<ShoppingCartViewModel>(HttpContext.Session, CartSessionKey);
            if (cart == null || !cart.Items.Any())
            {
                TempData["ErrorMessage"] = "El carrito está vacío.";
                return RedirectToAction("Index");
            }

            var ticket = new Ticket
            {
                Fecha = DateTime.Now,
                Total = cart.Total,
                MetodoPago = paymentMethod
            };

            if (paymentMethod == "Efectivo")
            {
                if (amountReceived < ticket.Total)
                {
                    TempData["ErrorMessage"] = "El monto recibido es menor que el total a pagar.";
                    return RedirectToAction("Index");
                }
                ticket.MontoRecibido = amountReceived;
                ticket.Cambio = amountReceived - ticket.Total;
            }

            foreach (var item in cart.Items)
            {
                ticket.Items.Add(item: new TicketItem
                {
                    ProductId = item.ProductId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.Precio
                });
            }

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            
            HttpContext.Session.Remove(CartSessionKey);

            return RedirectToAction("Receipt", new { ticketId = ticket.Id });
        }

        
        public async Task<IActionResult> Receipt(int ticketId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Items)
                .ThenInclude(ti => ti.Product)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket != null) return View(ticket);

            return NotFound();
        }
    }
}