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

            // gọi đến hàm này để cập nhật card_reader đến phần mềm điều khiển khi khởi tạo socket connection
            using (WebSocket ws = new WebSocket("ws://127.0.0.1:7890/GetMKReaderID"))
            {
                ws.OnMessage += Ws_OnMessage;
                ws.Connect();
                DataTemplate data_send = new DataTemplate();
                data_send.mk_reader_id = "mk_Reader_sample";
                ws.Send(JsonConvert.SerializeObject(data_send));
                // Console.ReadKey();
            }

            // gọi dến hàm này khi có người dùng quẹt thẻ
            using (WebSocket ws = new WebSocket("ws://127.0.0.1:7890/GetMKLogs"))
            {
                ws.OnMessage += Ws_OnMessage;
                ws.Connect();
                DataTemplate data_send = new DataTemplate( "mkid_demo2", "uid2", "2021-08-03T09:20:16.3126479Z", "session_id sample2");
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
        public string mk_reader_id;

        // thông tin của user quét được qua MK
        public string uid;

        // thời điểm vào ra ex: 2021-08-03T09:20:16.3126479Z
        // DateTime.UtcNow.ToString("o");
        public string datetime;

        public string session_id;
        public DataTemplate() { }
        public DataTemplate(string mk_reader_id, string uid, string datetime, string session_id)
        {
            this.mk_reader_id = mk_reader_id;
            this.uid = uid;
            this.datetime = datetime;
            this.session_id = session_id;
        }
    }
}
