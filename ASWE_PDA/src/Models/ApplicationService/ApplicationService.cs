using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using ASWE_PDA.Models.ApiServices.GoldApi;
using ASWE_PDA.Models.ApplicationService.DataModel;
using ASWE_PDA.ViewModels;
using Avalonia.Layout;
using Avalonia.Media;

namespace ASWE_PDA.Models.ApplicationService;

public static class ApplicationService
{
    #region Fields

    public static bool IsVoiceEnabled = false;
    
    public static ObservableCollection<ChatMessage> Messages = new();
    
    private static readonly SpeechSynthesizer SpeechSynthesizer = new();
    private static readonly SpeechRecognitionEngine SpeechRecognitionEngine = new();

    public static MainWindowViewModel? _mainWindowViewModel = null;
    
    #endregion
    
    #region Constructors

    static ApplicationService()
    {
        AddBotMessage("Hey, how can I help you?");
        
        //check if design time
        if ((LicenseManager.UsageMode == LicenseUsageMode.Designtime))
            return;
        
        Init();
    }

    #endregion
    
    #region Private Methods

    private static async void Init()
    {
        SpeechSynthesizer.SelectVoiceByHints(VoiceGender.Female);
        SpeechSynthesizer.Rate = 3;

        var vocabulary = new Choices();
        vocabulary.Add("helix", "stop", "hello");

        var grammarBuilder = new GrammarBuilder();
        grammarBuilder.Append(vocabulary);

        var grammar = new Grammar(grammarBuilder);
        
        SpeechRecognitionEngine.LoadGrammar(grammar);
        SpeechRecognitionEngine.SetInputToDefaultAudioDevice();
        SpeechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);

        SpeechRecognitionEngine.SpeechRecognized += OnSpeechRecognized;

        var api = GoldApi.GetInstance();
        var res = await api.GetGoldSliverPriceDollar();
    }

    private static void OnSpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
    {
        switch (e.Result.Text.ToLower())
        {
            case "helix":
                _mainWindowViewModel?.OnSpeechButtonClick();
                break;
            case "stop":
                _mainWindowViewModel?.OnSpeechButtonClick();
                break;
        }

        if(!IsVoiceEnabled)
            return;
        
        switch (e.Result.Text.ToLower())
        {
            case "hello":
                AddUserMessage("Hello!");
                AddBotMessage("Hello, what can I do for you?");
                break;
        }
    }
    
    /// <summary>
    /// Adds a message from the bot to the main chat.
    /// </summary>
    private static void AddBotMessage(string message)
    {
        Messages.Add(new ChatMessage()
        {
            MessageText = message,
            MessageAlignment = HorizontalAlignment.Left,
            MessageBackground = new SolidColorBrush(Color.FromRgb(123, 120, 121)),
            IsBotIconVisible = true
        });
        
        //SpeechSynthesizer.Speak(message);
    }
    
    /// <summary>
    /// Adds a message from the user to the main chat.
    /// </summary>
    private static void AddUserMessage(string message)
    {
        Messages.Add(new ChatMessage()
        {
            MessageText = message
        });
    }
    
    #endregion
}