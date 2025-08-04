using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface IWebSocketManager
    {
        void AddSocket(WebSocket socket);
        Task RemoveSocket(WebSocket socket);
        Task BroadcastMessageAsync(string message);
    }
}