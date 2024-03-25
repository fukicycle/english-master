using System.ComponentModel.DataAnnotations;

namespace EnglishMaster.Client.Forms
{
    public sealed class RegisterAccountForm
    {
        [Required(ErrorMessage = "This field is required.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string LastName { get; set; } = null!;

    }
}
