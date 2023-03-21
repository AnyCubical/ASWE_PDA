using System.Reflection;
using ASWE_PDA.Models.ApplicationService;

namespace ASWE_PDA_Test;

public class ApplicationTests
{
    [Fact]
    public async void FinanceReportTest()
    {
        var methodInfo = typeof(ApplicationService)
            .GetMethod("GetFinanceReportAsync", BindingFlags.NonPublic | BindingFlags.Static);

        var task = (Task<string>)methodInfo?.Invoke(null, null)!;
        var result = await task;
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public async void EntertainmentTest()
    {
        var methodInfo = typeof(ApplicationService)
            .GetMethod("GetEntertainmentAsync", BindingFlags.NonPublic | BindingFlags.Static);

        var task = (Task<string>)methodInfo?.Invoke(null, null)!;
        var result = await task;
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public async void SportsTest()
    {
        var methodInfo = typeof(ApplicationService)
            .GetMethod("GetSportsAsync", BindingFlags.NonPublic | BindingFlags.Static);

        var task = (Task<string>)methodInfo?.Invoke(null, null)!;
        var result = await task;
        
        Assert.NotNull(result);
    }
}