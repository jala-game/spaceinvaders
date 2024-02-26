using System;
using System.Net.WebSockets;

namespace spaceinvaders.services;

public class WebSocketManager
{
    private static WebSocketManager _instance;

    private readonly ClientWebSocket _webSocketClient = new();

    public static WebSocketManager GetInstance()
    {
        _instance ??= new WebSocketManager();
        return _instance;
    }

    public async void Connect()
    {
        await _webSocketClient.ConnectAsync(new Uri("ws://localhost:8080"), default);
    }
}