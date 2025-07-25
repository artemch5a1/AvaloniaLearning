using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using AvaloniaApp.Models;
using AvaloniaApp.ServiceAbstractions;
using AvaloniaApp.Utils;

namespace AvaloniaApp.Services.DataServices
{
    public class UserService : IUserService
    {
        private readonly string _filePath;
        private const char Separator = ';';

        public UserService()
        {
            _filePath = "Users.csv";

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(
                    _filePath,
                    $"Id{Separator}Name{Separator}Surname{Separator}Email{Separator}DateAdding{Separator}DateEdit\n",
                    Encoding.UTF8
                );
            }
        }

        public List<User> GetAllUsers()
        {
            return File.ReadAllLines(_filePath, Encoding.UTF8)
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(ParseLine)
                .ToList();
        }

        public User? GetUserById(int id)
        {
            return GetAllUsers().FirstOrDefault(u => u.Id == id);
        }

        public void CreateUser(User user)
        {
            if (!UserValidator.ValidateUser(user).Item1)
                throw new ArgumentException("Неверный формат user");

            List<User> users = GetAllUsers();

            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;

            user.DateAdding = DateTime.Now;

            string line =
                $"{user.Id}{Separator}{user.Name}{Separator}{user.Surname}{Separator}{user.Email}{Separator}{user.DateAdding}{Separator}{user.DateEdit}";

            File.AppendAllLines(_filePath, new[] { line }, Encoding.UTF8);
        }

        public bool UpdateUser(User user)
        {
            if (!UserValidator.ValidateUser(user).Item1)
                return false;

            List<User> users = GetAllUsers();

            int index = users.FindIndex(u => u.Id == user.Id);

            if (index == -1)
                return false;

            user.DateEdit = DateTime.Now;
            user.DateAdding = users[index].DateAdding;

            users[index] = user;

            WriteAll(users);

            return true;
        }

        public bool DeleteUser(int id)
        {
            List<User> users = GetAllUsers();

            User? userToRemove = users.FirstOrDefault(u => u.Id == id);

            if (userToRemove == null)
                return false;

            users.Remove(userToRemove);

            WriteAll(users);

            return true;
        }

        private User ParseLine(string line)
        {
            string[] parts = line.Split(Separator);

            (DateTime, bool) edit;
            (DateTime, bool) create;

            edit.Item2 = DateTime.TryParse(parts[5], out edit.Item1);
            create.Item2 = DateTime.TryParse(parts[4], out create.Item1);

            return new User
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                Surname = parts[2],
                Email = parts[3],
                DateAdding = create.Item2 ? create.Item1 : edit.Item1,
                DateEdit = edit.Item2 ? edit.Item1 : null,
            };
        }

        private void WriteAll(List<User> users)
        {
            var lines = new List<string>
            {
                $"Id{Separator}Name{Separator}Surname{Separator}Email{Separator}DateAdding{Separator}DateEdit",
            };

            lines.AddRange(
                users.Select(u =>
                    $"{u.Id}{Separator}{u.Name}{Separator}{u.Surname}{Separator}{u.Email}{Separator}{u.DateAdding}{Separator}{u.DateEdit}"
                )
            );

            File.WriteAllLines(_filePath, lines, Encoding.UTF8);
        }
    }
}
