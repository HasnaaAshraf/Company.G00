using System.ComponentModel.DataAnnotations;

namespace Company.G00.PL.Dtos
{
    public class ResetPasswordDto
    {

        [Required(ErrorMessage = "New Password Is Required ")]
        [DataType(DataType.Password)]  // ******
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password Is Required ")]
        [DataType(DataType.Password)]  // ******
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password Does Not Match Password")]
        public string ConfirmPassword { get; set; }


    }
}
