using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PuntoDeVentaAutoCobroFinal.Models
{
    public class Product
    {
        [Key] 
        public int Id { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        public required string Marca { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public required string Categoria { get; set; }

        [Required(ErrorMessage = "El código de barras es obligatorio.")]
        [Display(Name = "Código de Barras")]
        public long CodigoDeBarras { get; set; }
    }
}