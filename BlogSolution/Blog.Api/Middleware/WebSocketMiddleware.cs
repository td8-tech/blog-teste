using Blog.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Api.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketManager _webSocketManager;

        public WebSocketMiddleware(RequestDelegate next, IWebSocketManager webSocketManager)
        {
            _next = next;
            _webSocketManager = webSocketManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Se a requisição for para o nosso endpoint de WebSocket...
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    _webSocketManager.AddSocket(webSocket);

                    // Mantém a conexão aberta para ouvir mensagens do cliente (ex: disconnect)
                    await Receive(webSocket, async (result, buffer) =>
                    {
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await _webSocketManager.RemoveSocket(webSocket);
                            return;
                        }
                    });
                }
                else
                {
                    context.Response.StatusCode = 400; // Bad Request
                }
            }
            else
            {
                // Se não for uma requisição WebSocket, passa para o próximo middleware no pipeline.
                await _next(context);
            }
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                handleMessage(result, buffer);
            }
        }
    }
}