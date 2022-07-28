using System;
using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class Author: BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}

