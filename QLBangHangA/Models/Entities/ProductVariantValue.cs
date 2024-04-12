using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLBangHangA.Models.Entities
{
    public partial class ProductVariantValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Value { get; set; }

        [ForeignKey("ProductAttribute")]
        public int ProductAttributeId { get; set; }

        public string Description { get; set; }

        public ProductAttribute? ProductAttribute { get; set; }

    }
}
