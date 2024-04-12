using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLBangHangA.Models.Entities
{
    public partial class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public string? Slug { get; set; }

        public string? NameWithType { get; set; }

        public string? PathWithType { get; set; }

        public int? ParentCode { get; set; }

        public int? Levels { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
