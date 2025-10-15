using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PuntoDeVentaAutoCobroFinal.Models
{
    public class TicketItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }
        
        public virtual Ticket? Ticket { get; set; }

        [Required]
        public int ProductId { get; set; }
        
        public virtual Product? Product { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }
    }
}