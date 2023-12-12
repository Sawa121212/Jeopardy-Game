using System;

namespace Confirmation.Module.Enums
{
    
    /// <summary>
    /// Состав кнопок и результат диалога сообщений.
    /// </summary>
    [Flags]
    public enum ConfirmationResultEnum
    {
        None = 0,
        Ok = 1,
        Yes = 2,
        No = 4,
        Cancel = 8,
    }
}