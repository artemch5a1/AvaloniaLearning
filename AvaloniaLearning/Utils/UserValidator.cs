using AvaloniaApp.Models;

namespace AvaloniaApp.Utils
{
    public static class UserValidator
    {
        public static (bool, string?) ValidateUser(User user)
        {
            if (
                user.Name == string.Empty
                || user.Email == string.Empty
                || user.Surname == string.Empty
            )
            {
                return (false, "Есть не заполненные поля");
            }

            if (!EmailValidator.isEmailValid(user.Email))
                return (false, "Неверный формат почты");

            return (true, null);
        }
    }
}
