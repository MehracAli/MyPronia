using Microsoft.AspNetCore.Identity;

namespace Pronia.Entities.UserModels
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
    }
}
