using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using ASWE_PDA.Models.ApiServices.GoldApi;
using ASWE_PDA.Models.ApplicationService.DataModel;
using ASWE_PDA.ViewModels;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

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
        Init();
    }

    #endregion
    
    #region Private Methods
    
    /// <summary>
    /// Initializes Application
    /// </summary>
    private static void Init()
    {
        SpeechSynthesizer.SelectVoiceByHints(VoiceGender.Female);
        SpeechSynthesizer.Rate = 3;

        var vocabulary = new Choices();
        vocabulary.Add(
            "helix", "stop", "hello",
            "finances");

        var grammarBuilder = new GrammarBuilder();
        grammarBuilder.Append(vocabulary);

        var grammar = new Grammar(grammarBuilder);
        
        SpeechRecognitionEngine.LoadGrammar(grammar);
        SpeechRecognitionEngine.SetInputToDefaultAudioDevice();
        SpeechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);

        SpeechRecognitionEngine.SpeechRecognized += OnSpeechRecognized;
        
        AddBotMessage("Hey, how can I help you?");
    }

    /// <summary>
    /// Handles Speech Recognition
    /// </summary>
    private static async void OnSpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
    {
        // toggle activation
        switch (e.Result.Text.ToLower())
        {
            case "helix":
                _mainWindowViewModel?.OnSpeechButtonClick();
                break;
            case "stop":
                _mainWindowViewModel?.OnSpeechButtonClick();
                IsVoiceEnabled = false;
                break;
        }

        if(!IsVoiceEnabled)
            return;
        
        switch (e.Result.Text.ToLower())
        {
            case "hello":
                _mainWindowViewModel?.OnSpeechButtonClick();
                AddUserMessage("Hello!");
                AddBotMessage("Greetings, what can I do for you?");
                break;
            case "finance":
                AddUserMessage("Finance?");
                var res = await GetFinanceReportAsync();
                AddBotMessage(res);
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
        
        SpeechSynthesizer.SpeakAsync(message);
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

    #region Use Cases

    private static async Task<string> GetFinanceReportAsync()
    {
        var coinPaprika = CoinPaprikaApi.GetInstance();
        var goldApi = GoldApi.GetInstance();
        var exchangeRateApi = ExchangeRateApi.GetInstance();

        var bitcoinEthereum = await coinPaprika.GetBitcoinEthereumPriceDollar();
        var goldSilver = await goldApi.GetGoldSliverPriceDollar();
        var exchangeRate = await exchangeRateApi.GetUSDtoEUR();


        var bitcoin = (bitcoinEthereum?.Item1 * exchangeRate) ?? 0;
        var ethereum = (bitcoinEthereum?.Item2 * exchangeRate) ?? 0;
        var gold = (goldSilver?.Item1 * exchangeRate) ?? 0;
        var silver = (goldSilver?.Item2 * exchangeRate) ?? 0;
    
        return @$"Here are the current exchange rates. Bitcoin : {Math.Round(bitcoin, 2)}€ , Ethereum : {Math.Round(ethereum, 2)}€ , Gold : {Math.Round(gold, 2)}€ , Silver : {Math.Round(silver, 2)}€ ";
    }

    #endregion
}