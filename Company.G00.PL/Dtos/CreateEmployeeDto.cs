using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Company.G00.DAL.Models;

namespace Company.G00.PL.Dtos
{
    public class CreateEmployeeDto
    {

        [Required (ErrorMessage= "Name Is Required")] 
        public string Name { get; set; }

        [Range(22,66 , ErrorMessage ="Age Must Be Between 22 and 66")]
        public int? Age { get; set; }


        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
                            ,ErrorMessage ="Address Must Be Like 123-street-city-country")]
        public string Address { get; set; }


        [DataType(DataType.EmailAddress, ErrorMessage = "Email is Not Valid")]
        public string Email { get; set; }


        [Phone]
        public string Phone { get; set; }


        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }


        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }


        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }


        [DisplayName("Date Of Birth")]
        public DateTime CreateAt { get; set; }

        public int? DepartmentId { get; set; }

        public string? DepartmentName { get; set; }

        public string? ImageName { get; set; }

        public Microsoft.AspNetCore.Http.IFormFile? Image { get; set; }

    }
}
