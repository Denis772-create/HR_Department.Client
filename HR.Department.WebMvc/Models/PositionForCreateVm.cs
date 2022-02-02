using System;
using System.ComponentModel.DataAnnotations;

namespace HR.Department.WebMvc.Models
{
    public class PositionForCreateVm
    {
        [Required]
        public string Name { get; set; }
        [MaxLength(30)]
        public string Description { get; set; }
        [Required]
        public Guid TypePositionId { get; set; }
    }
}
