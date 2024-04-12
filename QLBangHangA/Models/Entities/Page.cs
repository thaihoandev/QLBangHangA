using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLBangHangA.Models.Entities
{
    public partial class Page
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PageId { get; set; }

        public string? PageName { get; set; }

        public string? Contents { get; set; }

        public string? Thumb { get; set; }

        public bool Published { get; set; }

        public string? Title { get; set; }

        public string? MetaDesc { get; set; }

        public string? MetaKey { get; set; }

        public string? Alias { get; set; }

        public DateTime? CreateAt { get; set; }

        public int? Ordering { get; set; }
    }
}
