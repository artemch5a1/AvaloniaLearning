using MvvmNavigationKit.Abstractions.ViewModelBase;

namespace MvvmNavigationKit.Tests.TestHelper
{
    public class FakeViewModel : ViewModelTemplate
    {
        public (int, string) itemParam;

        public override void Dispose() { }

        public override void RefreshPage() { }

        protected override void InitializeParams<T>(T @params)
        {
            itemParam = GetAs<(int, string)>(@params);
        }
    }
}
