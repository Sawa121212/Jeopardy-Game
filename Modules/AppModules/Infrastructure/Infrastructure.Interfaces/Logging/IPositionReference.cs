namespace Infrastructure.Interfaces.Logging
{
    public interface IPositionReference
    {
        // TODO: Выкинуть?
        
        /// <summary>
        /// Ссылка на позицию в строке.
        /// </summary>
        int X { get; }

        /// <summary>
        /// Ссылка на строку.
        /// </summary>
        int Y { get; }
    }
}