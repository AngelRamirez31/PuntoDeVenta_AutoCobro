using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PuntoDeVentaAutoCobroFinal.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        [Display(Name = "Método de Pago")]
        public required string MetodoPago { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Monto Recibido")]
        public decimal? MontoRecibido { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Cambio { get; set; }

        
        public virtual ICollection<TicketItem> Items { get; set; } = new List<TicketItem>();
    }
}