using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace LB_Command_Prompt
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_VSCROLL = 0x115;
        private const bool SB_LINEUP = false;
        private const bool SB_LINEDOWN = true;

        private const int WM_SETREDRAW = 11;

        Process cmdProcess;

        string command = "";
   
        //int lineOffset = 0;
        //int caretOffset = 0;

        public Form1()
        {
            InitializeComponent();

            cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmdProcess.StartInfo.RedirectStandardInput = true;
            cmdProcess.StartInfo.RedirectStandardOutput = true;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.OutputDataReceived += CmdProcess_OutputDataReceived;

            cmdProcess.Start();
            cmdProcess.BeginOutputReadLine();

            //cmdProcess.StandardInput.WriteLine("echo Hello World!");   // Example command
            //cmdProcess.StandardInput.WriteLine("exit"); // Sends the exit command to the cmd
            //cmdProcess.WaitForExit();

            Console.Read();

            cmdProcess.StandardInput.WriteLine("");
        }

        private void CmdProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            cmdPrintString(e.Data);
        }

        public void PrintChar(char c)
        {
            SendMessage(richTextBox1.Handle, WM_SETREDRAW, false, 0);

            richTextBox1.AppendText(c.ToString());

            if (c.Equals('\n'))
            {
                cmdProcess.StandardInput.WriteLine(command);
                cmdProcess.StandardInput.WriteLine("");

                command = "";
            }
            else command = command + c;

            richTextBox1.Select(richTextBox1.Text.Length, 0);
            richTextBox1.ScrollToCaret();
            SendMessage(richTextBox1.Handle, WM_VSCROLL, SB_LINEDOWN, 0);

            SendMessage(richTextBox1.Handle, WM_SETREDRAW, true, 0);
            richTextBox1.Refresh();
        }

        public void PrintString(string s)
        {
            SendMessage(richTextBox1.Handle, WM_SETREDRAW, false, 0);
            
            string[] lines = richTextBox1.Text.Split('\n');

            richTextBox1.AppendText(s);

            richTextBox1.Select(richTextBox1.Text.Length, 0);
            richTextBox1.ScrollToCaret();
            SendMessage(richTextBox1.Handle, WM_VSCROLL, SB_LINEDOWN, 0);

            SendMessage(richTextBox1.Handle, WM_SETREDRAW, true, 0);
            richTextBox1.Refresh();
        }

        public void cmdPrintString(string s)
        {
            try
            {
                if (s.EndsWith(">")) richTextBox1.Invoke(new Action<string>(PrintString), new object[] { s });
                else richTextBox1.Invoke(new Action<string>(PrintString), new object[] { s + '\n' });
            }
            catch (NullReferenceException) { }
        }

        /*public string[] cmdGetLines()
        {
            return richTextBox1.Text.Split('\n');
        }*/

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

            char c = e.KeyChar;

            /*if (caretOffset == 0)*/ PrintChar(c);
            /*else richTextBox1.Text = richTextBox1.Text.Insert(richTextBox1.Text.Length - caretOffset, c.ToString());*/
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //int caretShift = 0;
            string[] lines = richTextBox1.Text.Split('\n');

            switch (e.KeyCode)
            {
                case Keys.Home:
                    PrintString("\n");
                    cmdProcess.StandardInput.WriteLine("");
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Back:
                    SendMessage(richTextBox1.Handle, WM_SETREDRAW, false, 0);
                    if (richTextBox1.Text.Length >= 1) richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.Text.Length - 1, 1);
                    if (command.Length >= 1) command = command.Remove(command.Length, 1);
                    richTextBox1.Select(richTextBox1.Text.Length - 1, 0);
                    richTextBox1.ScrollToCaret();
                    SendMessage(richTextBox1.Handle, WM_VSCROLL, SB_LINEDOWN, 0);
                    SendMessage(richTextBox1.Handle, WM_SETREDRAW, true, 0);
                    richTextBox1.Refresh();
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Enter:
                    PrintChar('\n');
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Left:
                    //if (caretOffset < richTextBox1.Text.Length) caretOffset += 1;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Right:
                    //if (caretOffset > 0) caretOffset -= 1;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Up:
                    /*if (lineOffset < richTextBox1.Text.Split('\n').Count() - 1) lineOffset += 1;
                    caretShift = lines[lines.Count() - 1 - lineOffset].Length;
                    if (caretOffset + caretShift <= richTextBox1.Text.Length) caretOffset += caretShift;*/
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Down:
                    /*if (lineOffset > 0) lineOffset -= 1;
                    caretShift = lines[lines.Count() - 1 - lineOffset].Length;
                    if (caretOffset - caretShift >= 0) caretOffset -= caretShift;
                    else
                    {
                        caretOffset -= caretShift;

                        for (int i = caretShift - caretOffset; i < 0; i++)
                        {
                            caretOffset++;
                        }
                    }*/
                    e.SuppressKeyPress = true;
                    break;
            }
        }
    }
}
