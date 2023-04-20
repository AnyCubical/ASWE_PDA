using System.Drawing;
using Avalonia.Layout;
using Avalonia.Media;
using Color = Avalonia.Media.Color;

namespace ASWE_PDA.Models.ApplicationService.DataModel;

public class ChatMessage
{
    public string MessageText { get; set; } = "";
    public bool IsBotIconVisible { get; set; } = false;
    public HorizontalAlignment MessageAlignment { get; set; } = HorizontalAlignment.Right;
}