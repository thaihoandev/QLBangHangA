﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLBangHangA.Models.Entities
{
    public partial class OrderDetail
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public int? OrderNumber { get; set; }

        public int? Quantity { get; set; }

        public int? Discount { get; set; }

        public decimal? Price { get; set; }

        public DateTime? ShipDate { get; set; }

        public virtual Order? Order { get; set; }

        public virtual Product? Product { get; set; }
    }
}
