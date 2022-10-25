using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace SeaOfShops.Models
{
    public class CustomUserValidator : IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (user.Email.ToLower().EndsWith("@mail.ru")) // ограничение по почте
            {
                errors.Add(new IdentityError
                {
                    Description = "Some spam"
                });
            }
            if (manager.FindByNameAsync(user.UserName) != null)
            {
                errors.Add(new IdentityError
                {
                    Description = "Пользователь с таким Email уже зарегистрирован"
                }); 
            }
            string pattern = "^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+$";
            if (!Regex.IsMatch(user.Email, pattern))
            {
                errors.Add(new IdentityError
                {
                    Description = "Неверный формат почты"
                });
            }
            if (user.UserName.Contains("Rostik"))
            {
                errors.Add(new IdentityError
                {
                    Description = "никаких ростиков"
                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
