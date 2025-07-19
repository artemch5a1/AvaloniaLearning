using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApp.Tests.TestHelper
{
    public class Animal
    {
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public string? Breed { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Animal animal)
                return animal.Name == this.Name
                    && animal.Age == this.Age
                    && animal.Breed == this.Breed;
            return false;
        }
    }
}
