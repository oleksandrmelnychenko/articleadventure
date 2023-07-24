using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace domain.ArticleAdventure.IdentityEntities
{
    public sealed class User : IdentityUser
    {
        public Guid NetId { get; set; }

        [MaxLength(250)]
        public string DisplayName { get; set; }

        public Guid? ResetPasswordToken { get; set; }
    }
}
