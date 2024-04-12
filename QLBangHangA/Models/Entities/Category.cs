using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLBangHangA.Models.Entities
{
    public partial class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CatId { get; set; }

        [Required]
        [DisplayName("Tên")]
        public string? CatName { get; set; }

        [DisplayName("Mô tả")]
        public string? CatDesc { get; set; }

        public int? ParentId { get; set; }
        public int? Levels { get; set; }

        public int? Ordering { get; set; }

        public bool Published { get; set; }

        [DisplayName("Ảnh hiển thị")]
        public string? Thumb { get; set; }

        [DisplayName("Tiêu đề")]
        public string? Title { get; set; }

        public string? Alias { get; set; }

        public string? MetaDesc { get; set; }

        public string? MetaKey { get; set; }

        public string? Cover { get; set; }

        public string? SchemaMarkup { get; set; }

        public virtual ICollection<News> News { get; set; } = new List<News>();

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
