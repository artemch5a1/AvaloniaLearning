using System;

namespace AvaloniaApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateAdding { get; set; }
        public DateTime? DateEdit { get; set; } = null;
    }
}
