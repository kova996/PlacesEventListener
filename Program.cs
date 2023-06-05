using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug)
                .AddConsole();
        });
        ILogger logger = loggerFactory.CreateLogger<Program>();

        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/eventsHub")
            .Build();

        connection.On<Object>("ReceiveMessage", message =>
        {
            logger.LogInformation(" [{TimeReceived:MMMM dd, yyyy}] Event received: {Message}", DateTimeOffset.UtcNow, message);
            //Console.WriteLine("Event received: " + message);
        });

        await connection.StartAsync();

        logger.LogInformation("Connected to SignalR hub");

        Console.ReadLine();

        await connection.StopAsync();
        logger.LogInformation("Connected to SignalR hub");
    }
}