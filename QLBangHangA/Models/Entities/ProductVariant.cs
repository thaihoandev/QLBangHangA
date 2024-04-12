using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLBangHangA.Models.Entities
{
    public partial class ProductVariant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int? SizeId { get; set; }
        public int? ColorId { get; set; }

        public int Quantity { get; set; }
           
        public virtual Product? Product { get; set; }
        
    }
}
