using Fleck;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace AndroidControl.Hacker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var meuIp = GetLocalIPAddress();
            Console.WriteLine("My IP: " + meuIp);

            string porta = "15345";

            if (args != null && args.Length > 0)
                porta = args[0];

            var _servidor = new WebSocketServer($"ws://{meuIp}:{porta}");
            var _conexoes = new List<IWebSocketConnection>();
            var swap = false;
            var cursor = false;
            var grita = false;
            var barra = 1;
            _servidor.Start((conexao) =>
            {
                conexao.OnOpen = () =>
                {
                    _conexoes.Add(conexao);
                    Console.WriteLine("User connected: " + conexao.ConnectionInfo.ClientIpAddress);
                };

                conexao.OnClose = () =>
                {
                    _conexoes.Remove(conexao);
                    Console.WriteLine("User disconnected: " + conexao.ConnectionInfo.ClientIpAddress);
                };

                conexao.OnMessage = (mensagem) =>
                {
                    mensagem = mensagem.ToLower();

                    switch (mensagem)
                    {
                        case "hide":
                            {
                                barra = barra == 0 ? 1 : 0;
                                Helper.TaskBar(barra);
                                break;
                            }
                        case "swap":
                            {
                                swap = !swap;
                                Helper.SwapMouse(swap);
                                break;
                            }
                        case "movecursor":
                            {
                                Helper.MouseCursor();
                                break;
                            }
                        case "opencddriver":
                            {
                                Helper.OpenCdDriver();
                                break;
                            }
                        case "lock":
                            {
                                Helper.Lock();
                                break;
                            }
                        case "grita":
                            {
                                grita = !grita;

                                if (grita)
                                    Helper.Grita();
                                else
                                    Helper.ParaDeGritar();

                                break;
                            }
                        case "wallpaper":
                            {
                                var papelDeParede = Path.Combine(Directory.GetCurrentDirectory(), _wallpapers[countWallPaper]);

                                Helper.SetWallpaper(papelDeParede);
                                countWallPaper++;

                                if (countWallPaper > 1)
                                    countWallPaper = 0;
                                break;
                            }
                        case "wind":
                            {
                                Helper.WinD();
                                break;
                            }
                        case "rotate180":
                            {
                                Helper.Rotate180();
                                break;
                            }
                        case "resetrotate":
                            {
                                Helper.ResetRotate();
                                break;
                            }
                    }
                };
            });

            while (true) { }
        }

        private static int countWallPaper = 0;
        private static readonly string[] _wallpapers = new string[] { @"anonymous.jpg", @"anonymous-2.jpg" };

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}