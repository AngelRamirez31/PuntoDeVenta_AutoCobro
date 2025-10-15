namespace PuntoDeVentaAutoCobroFinal.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal Total => Items.Sum(i => i.Precio * i.Cantidad);
    }
}