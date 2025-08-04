using Blog.Application.Interfaces;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Services
{
    public class WebSocketManager : IWebSocketManager
    {
        // ConcurrentBag é uma coleção thread-safe, ideal para um ambiente singleton
        // onde múltiplos threads (requisições) podem adicionar/remover sockets simultaneamente.
        private readonly ConcurrentBag<WebSocket> _sockets = new ConcurrentBag<WebSocket>();

        public void AddSocket(WebSocket socket)
        {
            _sockets.Add(socket);
        }

        public async Task RemoveSocket(WebSocket socket)
        {
            // Em um cenário mais complexo, gerenciaríamos os sockets por um ID único.
            // Para este projeto, o middleware é o principal responsável por detectar
            // o fechamento da conexão. Esta implementação é um exemplo de como
            // uma remoção poderia ser tratada.

            // A maneira mais eficaz de remover um item específico de um ConcurrentBag
            // é, na verdade, reconstruir a lista sem ele, mas para este caso,
            // o mais importante é que a lógica de broadcast verifica o estado do socket.
            if (!socket.CloseStatus.HasValue)
            {
                await socket.CloseAsync(
                   closeStatus: WebSocketCloseStatus.NormalClosure,
                   statusDescription: "Closed by the WebSocketManager",
                   cancellationToken: CancellationToken.None);
            }
        }

        public async Task BroadcastMessageAsync(string message)
        {
            var messageBuffer = Encoding.UTF8.GetBytes(message);

            // Percorre todos os sockets e envia a mensagem para aqueles que estão abertos.
            // Esta abordagem é segura mesmo que a coleção _sockets seja modificada
            // durante a iteração.
            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(
                        buffer: new ArraySegment<byte>(messageBuffer, 0, messageBuffer.Length),
                        messageType: WebSocketMessageType.Text,
                        endOfMessage: true,
                        cancellationToken: CancellationToken.None);
                }
            }
        }
    }
}