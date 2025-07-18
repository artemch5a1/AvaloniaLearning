namespace AvaloniaApp.ServiceAbstractions
{
    public interface IViewModelBase
    {
        void Initialize<T>(T @params);
    }
}