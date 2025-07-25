
namespace MvvmNavigationKit.Tests.TestHelper
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
