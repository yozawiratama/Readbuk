using System;
using Domain.Common;

namespace Domain.Entities
{
    public class Language: BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}

