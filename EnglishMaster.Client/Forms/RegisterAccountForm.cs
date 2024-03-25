using System.ComponentModel.DataAnnotations;

namespace EnglishMaster.Client.Forms
{
    public sealed class RegisterAccountForm
    {
        [Required(ErrorMessage = "This field is required.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [Compare("Password", ErrorMessage = "Re-type password doesn't match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
