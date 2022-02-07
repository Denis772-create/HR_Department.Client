using System;
using System.ComponentModel.DataAnnotations;

namespace HR.Department.WebMvc.Models
{
    public class EmployeeForAddingToPositionVm
    {
        public Guid PositionId { get; set; }
        [Required(ErrorMessage = "The FirstName is required.")]
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [Required(ErrorMessage = "The Phone is required.")]
        [RegularExpression(@"^\+375\s\(\d{2}\)\s\d{2}\S\d{2}\S\d{3}$",
            ErrorMessage = "Required format +375 (00) 00-00-000")]
        public string Phone { get; set; }
        [Range(18, 60, ErrorMessage = "Minimum age allowed 18.")]
        public int Age { get; set; }
        [Required(ErrorMessage = "The RequiredSalary is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Salary cannot be less than 1$.")]
        public decimal RequiredSalary { get; set; }
    }
}
