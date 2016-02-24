using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Ransomware;
using System.IO;
using System.Security.Cryptography;
namespace decryptoTool
{
    static class Program
    {
        private static void decryptAll(string dir, Byte[] aesKey)
        {
            var di = new DirectoryInfo(dir);
            try
            {
                foreach (FileInfo fi in di.GetFiles("*.*"))
                    ransomwareCryptoMod.decryptFile(fi.FullName, aesKey);
                foreach (DirectoryInfo d in di.GetDirectories())
                    decryptAll(d.FullName, aesKey);
            }
            catch (Exception)
            {
            }
        }

        [STAThread]
        static void Main(string[] arg)
        {
            if (!File.Exists("sendBack.txt"))
            {
                MessageBox.Show("You need RSA data for me.(sendBack.txt)");
                return;
            }
            RSACryptoServiceProvider RSAObj = new RSACryptoServiceProvider();
            RSAObj.FromXmlString(File.ReadAllText("sendBack.txt"));
            Byte[] myKey = RSAObj.Decrypt(File.ReadAllBytes("secretAES.txt"), false);
            decryptAll(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), myKey);
        }
    }
}
