
namespace MvvmNavigationKit.Tests.TestHelper
{
    public class FakeViewModel2 : ViewModelBase
    {
        public Animal animal { get; set; } = new Animal();

        protected override void InitializeParams<T>(T @params)
        {
            animal = GetAs<Animal>(@params);
        }

        public override void RefreshPage()
        {
            animal.Breed = "тест";
        }

        public override void Dispose() { }
    }
}
