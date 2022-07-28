using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required]
        [Key]
        public string Code { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }

        [Required]
        public int PublishYear { get; set; }

        public Category Category { get; set; }
        public Publisher Publisher { get; set; }
        public ICollection<BookAuthor> Authors { get; set; }
        public ICollection<BookVariant> Variants { get; set; }
    }
}

