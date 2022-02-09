using System;
using System.ComponentModel.DataAnnotations;

namespace HR.Department.WebMvc.ViewModels
{
    public class PositionForCreateVm
    {
        [Required(ErrorMessage = "The Name is required.")]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public Guid TypePositionId { get; set; }
    }
}
