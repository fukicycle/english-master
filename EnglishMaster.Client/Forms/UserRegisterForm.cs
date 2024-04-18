using System.ComponentModel.DataAnnotations;

namespace EnglishMaster.Client.Forms
{
    public sealed class UserRegisterForm
    {
        [Required(ErrorMessage = "This field is required.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string LastName { get; set; } = null!;
        [MaxLength(15, ErrorMessage = "Nickname within 15 chars.")]
        public string? Nickname { get; set; }

    }
}
