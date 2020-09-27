using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace RemoteServer
{
    class Server
    {
        private readonly int port; // порт для прослушивания подключений
        private TcpClient client;
        private TcpListener server;
        private NetworkStream mainStream;

        public readonly Thread Listening;
        public readonly Thread GetImage;
        public readonly Thread GetText;
        public PictureBox picture;
        private TextBox textBox;
 

        int bytesReceived, totalReceived = 0;
        byte[] receivedData = new byte[1024 * 600];
        byte[] receivedDataText = new byte[1024 * 600];
        public Server(PictureBox pctr, TextBox tBox, int port = 8888)
        {

            this.port = port;
            this.picture = pctr;
            this.textBox = tBox;
            client = new TcpClient();
            server = new TcpListener(IPAddress.Any, port);
            Listening = new Thread(ServerStart);
            GetImage = new Thread(ReceiveImage);
            GetText = new Thread(ReceiveText);
     
        }
        //Запуск сервера на прослушивание
        public void ServerStart()
        {
            try
            {
              
                while (!client.Connected)
                {
                    // Запуск слушателя
                    server.Start();
                    Console.WriteLine("Ожидание подключений... ");

                    // Приниаем входящее подключение
                    client = server.AcceptTcpClient();
                }

                Console.WriteLine("Подключен клиент. Выполнение запроса...");

                GetImage.Start();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
        }
        //Остановка прослушивания 
        public void StopListening()
        {
            //
            server.Stop();
            client = null;

            if (Listening.IsAlive || GetImage.IsAlive || GetText.IsAlive)
            {    
                Listening.Abort();
            }
        }
        //Получение изображения с клиента
        public void ReceiveImage()
        {
           
            while (client.Connected)
            {
                //Считываем данные из потока
                mainStream = client.GetStream();
                do
                {
                    bytesReceived = mainStream.Read(receivedData, 0, receivedData.Length);
                    try
                    {
                        var image = Image.FromStream(new MemoryStream(receivedData));
                        picture.Image = image;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }

                    totalReceived += bytesReceived;

                    Console.WriteLine(DateTime.Now + " Bytes received:" + bytesReceived.ToString());
                }
                while (bytesReceived != 0);

                Console.WriteLine("Total bytes read:" + totalReceived.ToString());
            }
        }
        //Получение текста с клиента
        public void ReceiveText() 
        {
           /*
            Реализация получения данных о игре
                    */

        }

    }
}
