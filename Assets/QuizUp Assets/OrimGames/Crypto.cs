using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Crypto : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public static string encryptData(string text,string key)
    {
        byte[] data = UTF8Encoding.UTF8.GetBytes(text);
        string finalHash = "";
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform transform = tripDes.CreateEncryptor();
                byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                finalHash = Convert.ToBase64String(results, 0, results.Length);
            }
        }
        return finalHash;
    }

    public static string decryptData(string text,string key)
    {
        byte[] data = Convert.FromBase64String(text); // decrypt the incrypted text
        string finalHash = "";
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform transform = tripDes.CreateDecryptor();
                byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                finalHash = UTF8Encoding.UTF8.GetString(results);
            }
        }
        return finalHash;
    }
}
