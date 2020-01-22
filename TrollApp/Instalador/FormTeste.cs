using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Instalador
{
    public partial class FormTeste : Form
    {
        private object syncGate = new object();
        private Process process;
        private StringBuilder output = new StringBuilder();
        private bool outputChanged;

        public FormTeste()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lock (syncGate)
            {
                if (process != null) return;
            }

            output.Clear();
            outputChanged = false;
            textBox1.Text = "";

            process = new Process();
            process.StartInfo.FileName = @"systeminfo";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += OnOutputDataReceived;
            process.Exited += OnProcessExited;
            process.Start();
            process.BeginOutputReadLine();
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (syncGate)
            {
                if (sender != process)
                    return;
                output.AppendLine(e.Data);
                if (outputChanged)
                    return;
                outputChanged = true;
                BeginInvoke(new Action(OnOutputChanged));
            }
        }

        private void OnOutputChanged()
        {
            lock (syncGate)
            {
                textBox1.Text = output.ToString();
                outputChanged = false;
            }
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            lock (syncGate)
            {
                if (sender != process) return;
                process.Dispose();
                process = null;
            }
        }
    }
}