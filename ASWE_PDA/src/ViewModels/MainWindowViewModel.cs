using System.Collections.ObjectModel;
using System.Reactive;
using ASWE_PDA.Models.ApplicationService;
using ASWE_PDA.Models.ApplicationService.DataModel;
using Avalonia.Media;
using Material.Icons;
using ReactiveUI;

namespace ASWE_PDA.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    #region Fields

    private static MaterialIconKind _speechButtonIcon;
    private static IBrush? _speechButtonBrush;

    #endregion
    
    #region Properties

    public MaterialIconKind SpeechButtonIcon
    {
        get => _speechButtonIcon;
        set => this.RaiseAndSetIfChanged(ref _speechButtonIcon, value);
    }
    
    public IBrush? SpeechButtonBrush
    {
        get => _speechButtonBrush;
        set => this.RaiseAndSetIfChanged(ref _speechButtonBrush, value);
    }
    
    public ObservableCollection<ChatMessage> Messages
    {
        get => ApplicationService.Messages;
        set => this.RaiseAndSetIfChanged(ref ApplicationService.Messages, value);
    }

    #endregion

    #region Constructors
    
    public MainWindowViewModel()
    {
        SpeechButtonIcon = MaterialIconKind.MicrophoneOff;
        SpeechButtonBrush = Brushes.Red;
        
        SpeechButtonClick = ReactiveCommand.Create(OnSpeechButtonClick);

        ApplicationService._mainWindowViewModel = this;
    }

    #endregion

    #region Commands
    
    public ReactiveCommand<Unit, Unit> SpeechButtonClick { get; }
    
    #endregion

    #region Public Methods
    
    public void OnSpeechButtonClick()
    {
        SpeechButtonIcon = SpeechButtonIcon == MaterialIconKind.Microphone ? MaterialIconKind.MicrophoneOff : MaterialIconKind.Microphone;
        SpeechButtonBrush = Equals(SpeechButtonBrush, Brushes.Green) ? Brushes.Red : Brushes.Green;

        ApplicationService.IsVoiceEnabled = !ApplicationService.IsVoiceEnabled;
    }

    #endregion
}