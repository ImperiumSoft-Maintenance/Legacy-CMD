using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LB_Command_Prompt
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 cmd = new Form1();
            Application.Run(cmd);
        }
    }
}
