﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace QLBangHangA.Models.Entities
{
    public partial class News
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        public string? Title { get; set; }

        public string? Scontents { get; set; }

        public string? Contents { get; set; }

        public string? Thumb { get; set; }

        public bool Published { get; set; }

        public string? Alias { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? Author { get; set; }

        public int? AccountId { get; set; }

        public string? Tags { get; set; }

        public int? CatId { get; set; }

        public bool IsHot { get; set; }

        public bool IsNewfeed { get; set; }

        public string? MetaDesc { get; set; }
        public string? MetaKey { get; set; }

        public int? Views { get; set; }

        public virtual Category? Cat { get; set; }
        
    }
}
