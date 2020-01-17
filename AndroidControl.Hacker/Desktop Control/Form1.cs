using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Control
{
    public partial class Form1 : Form
    {
        private ClientWebSocket _client;
        private CancellationToken _cancel;

        public Form1()
        {
            InitializeComponent();
        }

        private async Task ConectarAoServer(string ip, string porta)
        {
            _client = new ClientWebSocket();
            await _client.ConnectAsync(new System.Uri($"ws://{ip}:{porta}"), new CancellationToken());
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private async void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                await ConectarAoServer(textBox1.Text, textBox2.Text);
            }
            catch
            {
                MessageBox.Show("Socket indisponível");
            }
        }

        private async void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "fechou", new CancellationToken());
            }
            catch
            {
                MessageBox.Show("Erro");
            }
            button2.Enabled = false;
            button1.Enabled = true;
        }

        private async void button3_Click(object sender, System.EventArgs e)
        {
            await SendMessage("hide");
        }

        private async Task SendMessage(string message)
        {
            if (_client.State == WebSocketState.Open)
            {
                var segment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                await _client.SendAsync(segment, WebSocketMessageType.Text, true, _cancel);
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            await SendMessage("swap");
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await SendMessage("movecursor");
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            await SendMessage("opencddriver");
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            await SendMessage("lock");
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            await SendMessage("wallpaper");
        }

        private async void button9_Click(object sender, EventArgs e)
        {
            await SendMessage("grita");
        }
    }
}