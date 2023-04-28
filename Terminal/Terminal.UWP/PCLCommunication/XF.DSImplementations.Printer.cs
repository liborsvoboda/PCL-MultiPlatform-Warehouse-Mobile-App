using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Terminal.UWP.Printer))]
namespace Terminal.UWP
{
    public class Printer : Interfaces.IPclPrintService
    {
        public void Print(string ipAddress, int port, byte[] content)
        {
            Socket pSocket = new Socket(SocketType.Stream, ProtocolType.IP);
            pSocket.SendTimeout = 1500;
            pSocket.Connect(ipAddress, port);
            pSocket.Send(content);
            pSocket.Close();
        }
    }
}