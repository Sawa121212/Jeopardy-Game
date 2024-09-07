using GameSender.Domain;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameSender.Infrastructure;

internal static class GameSenderButtons
{
    // var rkm = new ReplyKeyboardMarkup();  
    //    rkm.Keyboard =  
    // new KeyboardButton[][]  
    // {  
    //  new KeyboardButton[]  
    //  {  
    //      new KeyboardButton("item"),  
    //      new KeyboardButton("item")  
    //  },  
    //    new KeyboardButton[]  
    //  {  
    //      new KeyboardButton("item")  
    //  }  
    // }; 

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ReplyKeyboardRemove EmptyButtons { get; } = new()
    {
        Selective = true
    };

    public static ReplyKeyboardMarkup SendAnInvitationButton { get; } = new(new KeyboardButton(GameMessages.ConnectToRoom));

    public static ReplyKeyboardMarkup BaseRoomButtons { get; } = new(
        new KeyboardButton[]
        {
            new KeyboardButton(GameMessages.SetToHost), new KeyboardButton(GameMessages.LeaveTheRoom)
        }
    );
}