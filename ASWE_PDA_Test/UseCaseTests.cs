using System.Reflection;
using ASWE_PDA.Models.ApplicationService;

namespace ASWE_PDA_Test;

public class UseCaseTests
{
    [Fact]
    public async void FinanceReportTest()
    {
        var methodInfo = typeof(ApplicationService)
            .GetMethod("GetFinanceReportAsync", BindingFlags.NonPublic | BindingFlags.Static);

        var task = (Task<string>)methodInfo?.Invoke(null, null)!;
        var result = await task;
        
        Assert.NotNull(result);
        Assert.IsType<string>(result);
    }
    
    [Fact]
    public async void EntertainmentTest()
    {
        var methodInfo = typeof(ApplicationService)
            .GetMethod("GetEntertainmentAsync", BindingFlags.NonPublic | BindingFlags.Static);

        var task = (Task<string>)methodInfo?.Invoke(null, null)!;
        var result = await task;
        
        Assert.NotNull(result);
        Assert.IsType<string>(result);
    }
    
    [Fact]
    public async void SportsTest()
    {
        var methodInfo = typeof(ApplicationService)
            .GetMethod("GetSportsAsync", BindingFlags.NonPublic | BindingFlags.Static);

        var task = (Task<string>)methodInfo?.Invoke(null, null)!;
        var result = await task;
        
        Assert.NotNull(result);
        Assert.IsType<string>(result);
    }
    
    [Fact]
    public async void GoodMorningTest()
    {
        var methodInfo = typeof(ApplicationService)
            .GetMethod("GetGoodMorningAsync", BindingFlags.NonPublic | BindingFlags.Static);

        var task = (Task<string>)methodInfo?.Invoke(null, null)!;
        var result = await task;
        
        Assert.NotNull(result);
        Assert.IsType<string>(result);
    }
    
    [Fact]
    public async Task AskForMoreInformationTest()
    {
        // Arrange
        var moreInformationPrompt = "Would you like to know more?";

        // Get the private AskForMoreInformation method using reflection
        var methodInfo = typeof(ApplicationService)
            .GetMethod("AskForMoreInformation", BindingFlags.NonPublic | BindingFlags.Static);

        // Act
        var task = (Task)methodInfo?.Invoke(null, null);
        await task;

        // Assert
        Assert.Equal(moreInformationPrompt, ApplicationService.LastAddedMessage.MessageText);
    }

    [Fact]
    public async void HandleHelloTest()
    {
        var expectedResponse = "Greetings, what can I do for you?";

        var methodInfo = typeof(ApplicationService)
            .GetMethod("HandleHello", BindingFlags.NonPublic | BindingFlags.Static);
        
        var task = (Task<string>)methodInfo?.Invoke(null, null)!;
        var result = await task;
        
        Assert.NotNull(result);
        Assert.Equal(expectedResponse, result);
    }
    
    [Fact]
    public async void HandleBundesLiga()
    {
        var expectedResponse = "Here is the Bundesliga table";

        var methodInfo = typeof(ApplicationService)
            .GetMethod("HandleBundesLiga", BindingFlags.NonPublic | BindingFlags.Static);
        
        var task = (Task<string>)methodInfo?.Invoke(null, null)!;
        var result = await task;
        
        Assert.NotNull(result);
        Assert.Equal(expectedResponse, result);
    }
    
    [Fact]
    public async void HandleF1()
    {
        var methodInfo = typeof(ApplicationService)
            .GetMethod("HandleF1", BindingFlags.NonPublic | BindingFlags.Static);
        
        var task = (Task<string>)methodInfo?.Invoke(null, null)!;
        var result = await task;
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task HandleCheetahRecognitionAsync_Hello()
    {
        // Arrange
        string recognizedText = "hello";
        
        // Act
        await ApplicationService.HandleCheetahRecognitionAsync(recognizedText);

        // Assert
        // Check if the bot added the correct message to Messages list
        // You may need to expose the Messages property or create a public getter for testing purposes
        Assert.NotNull(ApplicationService.Messages.Last().MessageText);
    }
    
    [Fact]
    public async Task HandleCheetahRecognitionAsync_Stop()
    {
        // Arrange
        string recognizedText = "stop";

        // Set IsVoiceEnabled to true for testing purposes
        ApplicationService.IsVoiceEnabled = true;

        // Act
        await ApplicationService.HandleCheetahRecognitionAsync(recognizedText);

        // Assert
        Assert.False(ApplicationService.IsVoiceEnabled);
    }

    [Fact]
    public async Task HandleCheetahRecognitionAsync_UnrecognizedInput()
    {
        // Arrange
        string recognizedText = "unrecognized input";
        string gptResponse = "This is a sample GPT-4 response.";

        // Replace GetGpt4ResponseAsync with a mock version that returns a fixed response
        ApplicationService.GetGpt4ResponseEvent += async (input) => gptResponse;

        // Act
        await ApplicationService.HandleCheetahRecognitionAsync(recognizedText);

        // Assert
        Assert.NotNull(ApplicationService.Messages.Last().MessageText);

        // Unsubscribe the mock event handler
        ApplicationService.GetGpt4ResponseEvent -= async (input) => gptResponse;
    }



}