using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public enum BookVariantStatus
    {
        Available = 1,
        Borrowed = 2
    }

    public class BookVariant: BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public Book Book { get; set; }

        [Required]
        [Key]
        public string Code { get; set; }

        public BookVariantStatus Status { get; set; }

    }
}

