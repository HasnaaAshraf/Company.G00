using System.ComponentModel.DataAnnotations;

namespace Company.G00.PL.Dtos
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage ="Code Is Required !!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name Is Required !!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date Is Required !!")]
        public DateTime CreateAt { get; set; }


    }
}
