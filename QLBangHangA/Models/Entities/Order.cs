using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLBangHangA.Models.Entities
{
    public partial class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? ShipDate { get; set; }
        public string? ShippingAddress { get; set; }

        [ForeignKey("TransactStatus")]
        public int? TransactStatusId { get; set; }

        public bool? Deleted { get; set; }

        public bool? Paid { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int? PaymentId { get; set; }
        public decimal TotalPrice { get; set; }

        public string? Note { get; set; }

        public virtual TransactStatus? TransactStatus { get; set; }

        
        public ApplicationUser ApplicationUser { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
