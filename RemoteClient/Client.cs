using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.IO;
using Encoder = System.Drawing.Imaging.Encoder;
using System.Runtime.InteropServices;

namespace RemoteClient
{
    class Client
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }


        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public byte[] CaptureDesktop()
        {
            return GrabDesktop(GetDesktopWindow());
        }

        public byte[] CaptureActiveWindow()
        {
            return GrabDesktop(GetForegroundWindow());
        }

        private readonly int port;
        private readonly string server;
        private readonly TcpClient client = new TcpClient();
        private NetworkStream mainStream;
        private int framesSent = 0;
        private static byte[] bytes;
        private long imageQuality;

        public Client(string server, int port)
        {
            this.server = server;
            this.port = port;           
        }
        //Запуск клиента для подключения к серверу
        public bool ClientStart()
        {
            try
            {
                IPAddress ipAddress = Dns.GetHostEntry(server).AddressList[0];
                client.Connect(server, port);


                return true;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                return false;
            }

        }

        //Создание скриншота и преобразование его в байты
        private byte[] GrabDesktop(IntPtr handle)
        {
            //Rectangle bound = Screen.PrimaryScreen.Bounds;
            //Bitmap screenshot = new Bitmap(bound.Width, bound.Height, PixelFormat.Format32bppArgb);
            //Graphics graphics = Graphics.FromImage(screenshot);
            //graphics.CopyFromScreen(bound.X, bound.Y, 0, 0, bound.Size, CopyPixelOperation.SourceCopy);
            try
            {
                Rect rect = new Rect();
                GetWindowRect(handle, ref rect);
                Rectangle bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height);
                Graphics graphics = Graphics.FromImage(screenshot);
                graphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
                var mss = new MemoryStream();

                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, imageQuality);
                screenshot.Save(mss, GetEncoder(ImageFormat.Jpeg), encoderParameters);

                bytes = mss.ToArray();
                screenshot.Dispose();
                mss.Close();

                return bytes;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.Single(codec => codec.FormatID == format.Guid);
        }

        //Отправка изображения на сервер
        public void SendDektopImage(string grbType, string imgQuality)
        {
            string grabType = grbType;
            switch (imgQuality)
            {
                case "Low":
                    imageQuality = 24L;
                    break;
                case "Middle":
                    imageQuality = 48L;
                    break;
                case "High":
                    imageQuality = 64L;
                    break;
                default:
                    imageQuality = 48L;
                    break;
            }
            try
            {
                //Отправка байтов изображения на сервер
                mainStream = client.GetStream();
                framesSent += 1;
                int lenght = 0;
                if (grabType == "Active Window")
                {
                    lenght = CaptureActiveWindow().Length;
                    mainStream.Write(CaptureActiveWindow(), 0, lenght);
                }
                else if(grabType == "Screen")
                {
                    lenght = CaptureDesktop().Length;
                    mainStream.Write(CaptureDesktop(), 0, lenght);
                }
                Console.WriteLine("Client. Bytes sent:" + lenght.ToString());
                mainStream.Flush();
    
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
           
        }
        public void SendTextFile()
        {



        }

    }
}
        
    

