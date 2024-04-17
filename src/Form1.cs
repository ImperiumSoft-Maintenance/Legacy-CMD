using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LB_Command_Prompt
{
    public partial class Form1 : Form
    {
        int lineOffset = 0;
        int caretOffset = 0;

        Graphics g;

        public Form1()
        {
            InitializeComponent();
            g = this.CreateGraphics();
        }

        public void PrintChar(char c)
        {
            richTextBox1.Text = richTextBox1.Text + c;
        }

        public void PrintString(string s)
        {
            richTextBox1.Text = richTextBox1.Text + s;
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

            char c = e.KeyChar;

            if (c.Equals('\b') && richTextBox1.Text.Length >= 1) richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.Text.Length - 1 - caretOffset, 1);

            if (!c.Equals('\b') && caretOffset == 0) PrintChar(c);
            else if (!c.Equals('\b')) richTextBox1.Text = richTextBox1.Text.Insert(richTextBox1.Text.Length - caretOffset, c.ToString());

            richTextBox1.Select(richTextBox1.Text.Length - caretOffset, 0);
            richTextBox1.ScrollToCaret();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            int caretShift = 0;
            string[] lines = richTextBox1.Text.Split('\n');

            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (caretOffset < richTextBox1.Text.Length) caretOffset += 1;
                    richTextBox1.Select(richTextBox1.Text.Length - caretOffset, 0);
                    richTextBox1.ScrollToCaret();
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Right:
                    if (caretOffset > 0) caretOffset -= 1;
                    richTextBox1.Select(richTextBox1.Text.Length - caretOffset, 0);
                    richTextBox1.ScrollToCaret();
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Up:
                    if (lineOffset < richTextBox1.Text.Split('\n').Count() - 1) lineOffset += 1;
                    caretShift = lines[lines.Count() - 1 - lineOffset].Length;
                    if (caretOffset + caretShift <= richTextBox1.Text.Length) caretOffset += caretShift;
                    richTextBox1.Select(richTextBox1.Text.Length - caretOffset, 0);
                    richTextBox1.ScrollToCaret();
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Down:
                    if (lineOffset > 0) lineOffset -= 1;
                    caretShift = lines[lines.Count() - 1 - lineOffset].Length;
                    if (caretOffset - caretShift >= 0) caretOffset -= caretShift;
                    else
                    {
                        caretOffset -= caretShift;

                        for (int i = caretShift - caretOffset; i < 0; i++)
                        {
                            caretOffset++;
                        }
                    }
                    richTextBox1.Select(richTextBox1.Text.Length - caretOffset, 0);
                    richTextBox1.ScrollToCaret();
                    e.SuppressKeyPress = true;
                    break;
            }
        }
    }
}
