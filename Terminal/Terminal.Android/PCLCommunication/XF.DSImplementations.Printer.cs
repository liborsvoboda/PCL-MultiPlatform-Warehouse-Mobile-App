using System.Collections.Generic;
using System.Net.Sockets;
using Xamarin.Forms;

[assembly: Dependency(typeof(Terminal.Droid.Printer))]
namespace Terminal.Droid
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

    //TODO PRINT FILE https://github.com/bushbert/XamarinPCLPrinting/blob/master/PCLPrintExample/PCLPrintExample/PCLPrintExample.Android/Print.cs

}