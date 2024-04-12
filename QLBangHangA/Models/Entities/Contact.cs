using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLBangHangA.Models.Entities
{
    public partial class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }

        public string ContactName { get; set; }

        public string Phone { get; set; }
        public string Mail { get; set; }

        public string Message { get; set; }
        public DateTime? DateCreated { get; set; }



    }
}
