using System;
using System.ComponentModel.DataAnnotations;

namespace HttpConsumer.Domain.DTOs
{
    public class CreateCustomerDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid TenantId { get; set; }

        [Required]
        public bool Enabled { get; set; }
    }
}
