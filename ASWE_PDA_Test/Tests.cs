
using ASWE_PDA;
using ASWE_PDA.Models.ApplicationService;
using ASWE_PDA.Models.ApplicationService.DataModel;
using ASWE_PDA.ViewModels;
using ASWE_PDA.Views;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Threading;
using Moq;
using Pv;


namespace ASWE_PDA_Test;

public class AvaloniaTests
{
    [Fact]
    public void MessageAlignment_DefaultValue_IsRight()
    {
        // Arrange
        ChatMessage obj = new ChatMessage();

        // Act
        var messageAlignment = obj.MessageAlignment;

        // Assert
        Assert.Equal(HorizontalAlignment.Right, messageAlignment);
    }

    [Theory]
    [InlineData(HorizontalAlignment.Left)]
    [InlineData(HorizontalAlignment.Right)]
    [InlineData(HorizontalAlignment.Center)]
    [InlineData(HorizontalAlignment.Stretch)]
    public void MessageAlignment_SetValue_ReturnsSetValue(HorizontalAlignment alignmentToSet)
    {
        // Arrange
        ChatMessage obj = new ChatMessage();

        // Act
        obj.MessageAlignment = alignmentToSet;
        var messageAlignment = obj.MessageAlignment;

        // Assert
        Assert.Equal(alignmentToSet, messageAlignment);
    }
    
    [Fact]
    public void AddUserMessageTest()
    {
        // Arrange
        string sampleMessage = "Sample message";
        int minimumMessageCount = 1;

        // Act
        ApplicationService.AddUserMessage(sampleMessage);

        // Assert
        Assert.True(minimumMessageCount < ApplicationService.Messages.Count, "There was at least one more message, which has to be added by the user");
    }
    
    [Fact]
    public void Process_CallsCheetahProcess()
    {
        // Arrange
        var mockCheetah = new Mock<ApplicationService.ICheetah>();
        var sampleAudio = new short[] { 1, 2, 3, 4 };

        // Set up the Cheetah mock to return a predefined transcript and IsEndpoint value
        mockCheetah.Setup(c => c.Process(sampleAudio))
            .Returns(new CheetahTranscript("test transcript", true));

        // Act
        var result = mockCheetah.Object.Process(sampleAudio);

        // Assert
        // Verify that the Cheetah's Process method was called with the expected input
        mockCheetah.Verify(c => c.Process(sampleAudio), Times.Once());
        // Verify that the result is a CheetahTranscript object with the expected values
        Assert.Equal("test transcript", result.Transcript);
        Assert.True(result.IsEndpoint);
    }

    [Fact]
    public async Task HandleCheetahRecognitionAsync_RecognizedTextContainsHelix_AddsUserMessageAndReturns()
    {
        // Arrange
        string recognizedText = "he licks";
        ApplicationService.IsVoiceEnabled = true;

        // Act
        await ApplicationService.HandleCheetahRecognitionAsync(recognizedText);

        // Assert
        Assert.True(ApplicationService.Messages.Count > 0, "At least one message should be added");
    }
    

}

public class MainWindowTests
{
    [Fact]
    public void MainWindow_Constructor_InitializesComponent()
    {
        // Initialize the Avalonia platform
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .SetupWithoutStarting();

        // Arrange
        var mainWindow = new MainWindow();

        // Assert
        Assert.NotNull(mainWindow);
        Assert.NotNull(mainWindow.Content);
    }
    
}

public class MainWindowViewModelTests
{
    [Fact]
    public void OnSpeechButtonClick_TogglesIsVoiceEnabled()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var initialIsVoiceEnabled = ApplicationService.IsVoiceEnabled;

        // Act
        viewModel.OnSpeechButtonClick();
        var afterFirstClickIsVoiceEnabled = ApplicationService.IsVoiceEnabled;

        viewModel.OnSpeechButtonClick();
        var afterSecondClickIsVoiceEnabled = ApplicationService.IsVoiceEnabled;

        // Assert
        Assert.NotEqual(initialIsVoiceEnabled, afterFirstClickIsVoiceEnabled);
        Assert.NotEqual(afterFirstClickIsVoiceEnabled, afterSecondClickIsVoiceEnabled);
        Assert.Equal(initialIsVoiceEnabled, afterSecondClickIsVoiceEnabled);
    }

    [Fact]
    public void Messages_Property_ReturnsSameReferenceAsApplicationService()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();

        // Act
        var viewModelMessages = viewModel.Messages;
        var applicationServiceMessages = ApplicationService.Messages;

        // Assert
        Assert.Same(viewModelMessages, applicationServiceMessages);
    }

    [Fact]
    public void SpeechButtonClick_Command_TriggersOnSpeechButtonClick()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var initialSpeechButtonIcon = viewModel.SpeechButtonIcon;
        var initialSpeechButtonBrush = viewModel.SpeechButtonBrush;

        // Act
        viewModel.SpeechButtonClick.Execute().Subscribe();
        var afterFirstClickSpeechButtonIcon = viewModel.SpeechButtonIcon;
        var afterFirstClickSpeechButtonBrush = viewModel.SpeechButtonBrush;

        viewModel.SpeechButtonClick.Execute().Subscribe();
        var afterSecondClickSpeechButtonIcon = viewModel.SpeechButtonIcon;
        var afterSecondClickSpeechButtonBrush = viewModel.SpeechButtonBrush;

        // Assert
        Assert.NotEqual(initialSpeechButtonIcon, afterFirstClickSpeechButtonIcon);
        Assert.NotEqual(afterFirstClickSpeechButtonIcon, afterSecondClickSpeechButtonIcon);
        Assert.Equal(initialSpeechButtonIcon, afterSecondClickSpeechButtonIcon);

        Assert.NotEqual(initialSpeechButtonBrush, afterFirstClickSpeechButtonBrush);
        Assert.NotEqual(afterFirstClickSpeechButtonBrush, afterSecondClickSpeechButtonBrush);
        Assert.Equal(initialSpeechButtonBrush, afterSecondClickSpeechButtonBrush);
    }
}

public class AppTests
{
    [Fact]
    public void ViewLocator_Match_ReturnsTrueForViewModelBase()
    {
        // Arrange
        var viewLocator = new ViewLocator();
        var viewModelBase = new ViewModelBase();

        // Act
        var isMatch = viewLocator.Match(viewModelBase);

        // Assert
        Assert.True(isMatch);
    }
}

public class ViewLocatorTests
{
    [Fact]
    public void Build_ValidViewModel_ReturnsMatchingView()
    {
        // Arrange
        var viewLocator = new ViewLocator();
        var viewModel = new MySampleViewModel(); // Replace this with your actual ViewModel

        // Act
        var view = viewLocator.Build(viewModel);

        // Assert
        Assert.NotNull(view);
        Assert.IsType<Avalonia.Controls.TextBlock>(view); // Replace this with your actual View
    }

    [Fact]
    public void Build_InvalidViewModel_ReturnsNotFoundTextBlock()
    {
        // Arrange
        var viewLocator = new ViewLocator();
        var viewModel = new InvalidViewModel(); // Use a non-existing ViewModel for testing

        // Act
        var view = viewLocator.Build(viewModel);

        // Assert
        Assert.NotNull(view);
        Assert.IsType<TextBlock>(view);
        Assert.Equal("Not Found: ASWE_PDA_Test.InvalidView", ((TextBlock)view).Text);
    }
}

// Mock ViewModels for testing purposes
public class MySampleViewModel : ViewModelBase { }
public class InvalidViewModel : ViewModelBase { }

public class ProgramTests : IDisposable
{
    private IDisposable _appLifetime;

    public void Dispose()
    {
        _appLifetime?.Dispose();
    }

    [Fact]
    public void BuildAvaloniaApp_CreatesAppBuilder()
    {
        // Act
        var appBuilder = Program.BuildAvaloniaApp();

        // Assert
        Assert.NotNull(appBuilder);
        Assert.IsType<AppBuilder>(appBuilder);
    }
}
