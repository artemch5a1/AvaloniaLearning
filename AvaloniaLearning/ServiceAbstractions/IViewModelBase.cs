namespace AvaloniaApp.ViewModel
{
    public interface IViewModelBase
    {
        void Initialize<T>(T @params);
    }
}