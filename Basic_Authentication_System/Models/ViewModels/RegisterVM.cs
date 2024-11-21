using System.ComponentModel.DataAnnotations;

namespace Basic_Authentication_System.Models.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First Name is required.")]
        [MinLength(3, ErrorMessage ="First Name should contain at least 3 characters.")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required.")]
        [MinLength(2, ErrorMessage = "Last Name should contain at least 2 characters.")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter valid email address.")]
        public string Email { get; set; }

        //Validation used in view page
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords don't match, try another.")]
        public string ConfirmPassword { get; set; }
    }
}
