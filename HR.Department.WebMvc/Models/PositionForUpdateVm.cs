using System;
using System.ComponentModel.DataAnnotations;

namespace HR.Department.WebMvc.Models
{
    public class PositionForUpdateVm
    {
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "The Name is required.")]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }

    }
}
