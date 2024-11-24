using Microsoft.AspNetCore.Identity;

namespace WebsiteGenZ.Models
{
    public class AppUser : IdentityUser
    {


            public string FirstName { get; set; }
            public string LastName { get; set; }

            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }

            public bool IsActive { get; set; }

            public string Token { get; set; }





    }
}
