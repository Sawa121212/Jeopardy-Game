namespace Common.Ui.Parameters
{
    /// <summary>
    /// Параметры пользовательского интерфейса.
    /// </summary>
    public interface IPresentationParameters
    {
        /// <summary>
        /// Коэффициент масштабирования.
        /// </summary>
        double ScaleFactor { get; set; }
    }
}
