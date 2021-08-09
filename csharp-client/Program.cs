using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using Newtonsoft.Json;

namespace csharp_client
{
    class Vector
    {
        public double x, y;
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now);
            // Create a scoped instance of a WS client that will be properly disposed
            using (WebSocket ws = new WebSocket("ws://127.0.0.1:7890/EchoAll"))
            {
                ws.OnMessage += Ws_OnMessage;
                ws.Connect();
                DataTemplate data_send = new DataTemplate( "mkid_demo", "user_info_demo", "in", "2021-08-03T09:20:16.3126479Z");
                ws.Send(JsonConvert.SerializeObject(data_send));
                Console.ReadKey();
            }
        }

        private static void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("Received from the server: " + e.Data);

            try
            {
                Vector pos = JsonConvert.DeserializeObject<Vector>(e.Data);
                //Console.WriteLine("Created a vector: " + pos.x + "," + pos.y);
                DrawDot(pos.x, pos.y, 50, 15, 1);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                Console.WriteLine("I don't know what to do with \"" + e.Data + "\"");
            }

        }

        static void DrawDot(double xpos, double ypos, int width, int height, int borderWidth)
        {
             int x = (int) Math.Round(xpos * width);
            int y = (int)Math.Round(ypos * height);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (i == x && j == y)
                    {
                        Console.Write("O");
                    }
                    else if (j < borderWidth || j > height - 1 - borderWidth
                        || i < borderWidth || i > width - 1 - borderWidth)
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
    class DataTemplate
    {
        // id của MK hoặc info của MK
        public string mk_id;

        // thông tin của user quét được qua MK
        public string uid;

        // thời điểm vào ra ex: 2021-08-03T09:20:16.3126479Z
        // DateTime.UtcNow.ToString("o");
        public string date;

        public DataTemplate(string Mk_id, string Uid, string Date)
        {
            mk_id = Mk_id;
            uid = Uid;
            date = Date;
        }
    }
}
