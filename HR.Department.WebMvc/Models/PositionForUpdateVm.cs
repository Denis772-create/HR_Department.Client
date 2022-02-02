using System;
using System.ComponentModel.DataAnnotations;

namespace HR.Department.WebMvc.Models
{
    public class PositionForUpdateVm
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(30)]
        public string Description { get; set; }

    }
}
