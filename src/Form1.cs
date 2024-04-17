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
        Graphics g;

        public Form1()
        {
            InitializeComponent();
            g = this.CreateGraphics();
        }

        public void DrawChar(char c, int x, int y)
        {

        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            
        }
    }
}
