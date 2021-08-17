using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using WebSocketSharp.Server;

namespace csharp_server
{
    public class GetMKReaderID : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Received message from GetMKReaderID client: " + e.Data);
            Send(e.Data);
        }
    }

    public class GetMKLogs : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Received message from GetMKLogs client: " + e.Data);
            Sessions.Broadcast(e.Data);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            WebSocketServer wssv = new WebSocketServer("ws://127.0.0.1:7890");

            wssv.AddWebSocketService<GetMKReaderID>("/GetMKReaderID");
            wssv.AddWebSocketService<GetMKLogs>("/GetMKLogs");

            wssv.Start();
            Console.WriteLine("WS server started on ws://127.0.0.1:7890/GetMKReaderID");
            Console.WriteLine("WS server started on ws://127.0.0.1:7890/GetMKLogs");

            Console.ReadKey();
            wssv.Stop();
        }
    }
}
