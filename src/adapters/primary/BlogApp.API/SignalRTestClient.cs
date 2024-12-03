using Microsoft.AspNetCore.SignalR.Client;

namespace BlogApp.API;

public class SignalRTestClient
{
    private static HubConnection? _connection;
    private static IHttpContextAccessor _httpContextAccessor;

    public static async Task StartClient()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7070/notificationHub")
            .Build();

        _connection.On<string, string>("ReceiveNotification", (postTitle, postContent) =>
        {
            Console.WriteLine("[Cliente] - Nova postagem recebida:");
            Console.WriteLine($"Título: {postTitle}");
            Console.WriteLine($"Conteúdo: {postContent}");
        });

        _connection.Closed += async (exception) =>
        {
            Console.WriteLine("Conexão fechada. Tentando reconectar...");
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await StartClient();
        };

        try
        {
            await _connection.StartAsync();
            Console.WriteLine("[Cliente] - Conectado ao servidor SignalR!");

            if (_connection.State == HubConnectionState.Connected)
            {
                Console.WriteLine("[Cliente] - Conexão estabelecida com o SignalR Hub.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao conectar ao servidor SignalR: {ex.Message}");
            return;
        }

        Console.WriteLine("Pressione qualquer tecla para desconectar...");
        Console.ReadKey();

        await _connection.StopAsync();
        Console.WriteLine("[Cliente] Conexão encerrada.");
    }
}