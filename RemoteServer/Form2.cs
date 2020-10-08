using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteServer
{
    public partial class Form2 : Form
    {
        Server server;
        int port;
        public Form2(int port)
        {
            this.port = port;
            InitializeComponent();
          
        }
        //Запуск сервера при загрузке формы
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
          

            //Запуск сервера
            server = new Server(pictureBox1, port);
            server.Listening.Start();
        }

        //Остановка прослушивания сервера при закрытии формы
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            server.StopListening();
            Program.f1.btnListen.Enabled = true;
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
