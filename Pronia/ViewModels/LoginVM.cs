using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class LoginVM
    {
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
