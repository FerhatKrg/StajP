using Microsoft.AspNetCore.Identity;

namespace ProjeStaj.Models.IdentityModels
{
    public class CustomIdentityValidatior : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = $"Şifreniz en az {length} karakter içermelildir"
            };

        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresLower",
                Description = "Şifreniz en az 1 tane küçük harf içermelidir"
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresUpper",
                Description= "Şifreniz en az 1 tane Büyük harf içermelidir"
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresDigit",
                Description = "Şifreniz en az 1 tane rakam içermelidir"
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description= "Şifreniz en az 1 tane rakam içermelidir"
            };

        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description =$"{userName} adlı kullanıcı adı sistemde mevcut başka bir kullanıcı adı deneyiniz !"
            };

        }

        
    }
}
