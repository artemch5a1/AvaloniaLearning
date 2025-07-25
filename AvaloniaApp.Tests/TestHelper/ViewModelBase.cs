using MvvmNavigationKit.Abstractions.ViewModelBase;

namespace MvvmNavigationKit.Tests.TestHelper
{
    public class ViewModelBase : ViewModelTemplate
    {
        protected bool IsDisposed { get; set; } = false;

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }

        public override void RefreshPage() { }

        protected override void InitializeParams<T>(T @params) { }
    }
}
