using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace ASWE_PDA.Models.SpeechService;

public class SpeechServiceImpl : ISpeechService
{
    #region Fields

    private readonly SpeechSynthesizer _speechSynthesizer = new();

    #endregion
    
    #region Constructor

    public SpeechServiceImpl()
    {
        _speechSynthesizer.SelectVoiceByHints(VoiceGender.Female);
        _speechSynthesizer.Rate = 3;
    }

    #endregion

    #region MyRegion

    public void Speak(string text)
    {
        _speechSynthesizer.Speak(text);
    }

    #endregion
}