using GameSender.Domain;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameSender.Infrastructure;

public partial class GameSenderService
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
    private ReplyKeyboardRemove EmptyButtons { get; } = new()
    {
        Selective = true
    };

    private ReplyKeyboardMarkup SendAnInvitationButton { get; } = new(new KeyboardButton(GameMessages.ConnectToRoom));

    private ReplyKeyboardMarkup BaseRoomButtons { get; } = new(
        new KeyboardButton[]
        {
            new KeyboardButton(GameMessages.SetToHost), new KeyboardButton(GameMessages.LeaveTheRoom)
        }
    );
}