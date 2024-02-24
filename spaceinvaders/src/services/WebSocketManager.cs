using System.Net.WebSockets;
using System.Threading;

public class WebSocketManager {
    private static WebSocketManager instance;
    public static WebSocketManager GetInstance() {
        instance ??= new WebSocketManager();
        return instance;
    }

    private readonly ClientWebSocket webSocketClient;

    public WebSocketManager() {
        webSocketClient = new ClientWebSocket();
    }

    public async void Connect() {
        await webSocketClient.ConnectAsync(new System.Uri("ws://localhost:8080"), default);
    }
}