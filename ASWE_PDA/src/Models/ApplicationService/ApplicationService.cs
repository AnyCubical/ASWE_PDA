using System.Collections.ObjectModel;
using ASWE_PDA.Models.ApplicationService.DataModel;
using ASWE_PDA.Models.SpeechService;
using Avalonia.Layout;
using Avalonia.Media;

namespace ASWE_PDA.Models.ApplicationService;

public static class ApplicationService
{
    #region Fields
    
    public static ObservableCollection<ChatMessage> Messages = new();

    public static ISpeechService SpeechService;

    #endregion
    
    #region Constructors

    static ApplicationService()
    {
        SpeechService = new SpeechServiceImpl();
    }

    #endregion

    #region Public Methods

    public static void OnStartUp()
    {
        AddBotMessage("Hey, how can I help you?");
    }

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
        
        SpeechService.Speak(message);
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