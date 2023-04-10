using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class RegisterVM
    {
        [StringLength(maximumLength:10)]
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool Terms { get; set; }

    }
}
