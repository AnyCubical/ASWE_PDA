using System.Collections.ObjectModel;
using ASWE_PDA.Models.ApplicationService.DataModel;
using Avalonia.Layout;
using Avalonia.Media;

namespace ASWE_PDA.Models.ApplicationService;

public static class ApplicationService
{
    #region Fields
    
    public static ObservableCollection<ChatMessage> Messages = new();

    #endregion
    
    #region Constructors

    static ApplicationService()
    {
        AddBotMessage("ABCDE");
        AddUserMessage("BLABLA");
        AddBotMessage("ABCDE");
        AddUserMessage("BLABLA");
        AddBotMessage("ABCDE");
        AddUserMessage("BLABLA");
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Adds a message from the bot to the main chat.
    /// </summary>
    public static void AddBotMessage(string message)
    {
        Messages.Add(new ChatMessage()
        {
            MessageText = message,
            MessageAlignment = HorizontalAlignment.Left,
            MessageBackground = new SolidColorBrush(Color.FromRgb(123, 120, 121)),
            IsBotIconVisible = true
        });
    }
    
    /// <summary>
    /// Adds a message from the user to the main chat.
    /// </summary>
    public static void AddUserMessage(string message)
    {
        Messages.Add(new ChatMessage()
        {
            MessageText = message
        });
    }
    
    #endregion
}