using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteClient
{
    public partial class Form1 : Form
    {
        Client client;
        public Form1()
        {
            InitializeComponent();
            comboBoxGrabType.SelectedItem = "Screen";
            comboBoxQuality.SelectedItem = "Middle";
        }

        //Ввод данных для подключения к серверу
        private void btnConnect_Click(object sender, EventArgs e)
        {
            client = new Client(txtIP.Text, int.Parse(txtPort.Text));
            if(client.ClientStart())
            {
                btnConnect.Text = "Connected";
                MessageBox.Show("Connected");
                btnConnect.Enabled = false;
                btnShare.Enabled = true;
            }
            else
            {
                MessageBox.Show("Failed to connect");
                btnConnect.Text = "Not Conencted";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnShare.Enabled = false;
        }

        //Таймер для отправки изображения по тику таймера
        private void btnShare_Click(object sender, EventArgs e)
        {
            if (btnShare.Text.StartsWith("Share"))
            {
                timer1.Start();
                btnShare.Text = "Stop Sharing";
            }
            else
            {
                timer1.Stop();
                btnShare.Text = "Share My Screen";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            client.SendDektopImage(comboBoxGrabType.SelectedItem.ToString(), comboBoxQuality.SelectedItem.ToString());
        }

  
    }
}
