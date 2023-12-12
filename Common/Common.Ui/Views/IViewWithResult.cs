namespace Common.Ui.Views
{
    public interface IViewWithResult<out T>
    {
        T Result { get; }
    }
}