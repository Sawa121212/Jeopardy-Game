using GameSender.Domain;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameSender.Infrastructure;

public static class GameSenderButtons
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

    public static ReplyKeyboardMarkup MainButtons { get; } = new(new KeyboardButton(GameMessages.ConnectToRoom));

    public static ReplyKeyboardMarkup SendAnInvitationButtons { get; } = new(
        new KeyboardButton[]
        {
            new KeyboardButton(GameMessages.ConnectToRoom), new KeyboardButton(GameMessages.DenyConnect)
        }
    );

    public static ReplyKeyboardMarkup PlayerButtons { get; } = new(
        new KeyboardButton[]
        {
            new KeyboardButton(GameMessages.SetToHost), new KeyboardButton(GameMessages.LeaveTheRoom)
        }
    );

    public static ReplyKeyboardMarkup HostButtons { get; } = new(
        new KeyboardButton[]
        {
            new KeyboardButton(GameMessages.GoToPlayers), new KeyboardButton(GameMessages.LeaveTheRoom)
        }
    );

    public static ReplyKeyboardMarkup RedButton { get; } = new(new KeyboardButton(GameMessages.ToAnswer));
}