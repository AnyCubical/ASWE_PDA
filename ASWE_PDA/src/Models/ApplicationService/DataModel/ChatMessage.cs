using Avalonia.Layout;
using Avalonia.Media;

namespace ASWE_PDA.Models.ApplicationService.DataModel;

public class ChatMessage
{
    public string MessageText { get; set; } = "";
    public bool IsBotIconVisible { get; set; } = false;
    //public IBrush MessageBackground { get; set; } = new SolidColorBrush(Color.FromRgb(59, 248, 221));
    public HorizontalAlignment MessageAlignment { get; set; } = HorizontalAlignment.Right;
}