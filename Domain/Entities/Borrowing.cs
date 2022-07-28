using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class Borrowing: BaseEntity
    {
        [Required]
        public DateTime BorrowedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }

        [Required]
        public DateTime MustReturnAt { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Required]
        public decimal TotalRentPrice { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? LateCharge { get; set; }

        public Guid BorrowerID { get; set; }
        public string BorrowerName { get; set; }

        public BookVariant BookItem { get; set; }
    }
}

