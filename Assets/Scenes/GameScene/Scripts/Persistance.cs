using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;

public class Persistance
{
    private static readonly string encryptionKey = "muzica-encryption-key";
    private static readonly byte[] salt = Encoding.UTF8.GetBytes("muzica-salt");
    public static void Save<T>(string fileName, T data)
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(path))
            {
                File.Delete(path); // 既存のファイルを削除
            }

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            byte[] encryptedData = Encrypt(json);
            File.WriteAllBytes(path, encryptedData);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save data to {fileName}: {ex.Message}");
        }
    }

    public static T Load<T>(string fileName)
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(path))
            {
                byte[] encryptedData = File.ReadAllBytes(path);
                string json = Decrypt(encryptedData);
                T data = JsonConvert.DeserializeObject<T>(json);
                return data;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load data from {fileName}: {ex.Message}");
        }

        return default;
    }

    private static byte[] Encrypt(string jsonData)
    {
        using (Aes aes = Aes.Create())
        {
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(encryptionKey, salt);
            aes.Key = key.GetBytes(32);
            aes.IV = key.GetBytes(16);

            ICryptoTransform encryptor = aes.CreateEncryptor();

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(jsonData);
                    }
                }
                return ms.ToArray();
            }
        }
    }

    private static string Decrypt(byte[] encryptedData)
    {
        using (Aes aes = Aes.Create())
        {
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(encryptionKey, salt);
            aes.Key = key.GetBytes(32);
            aes.IV = key.GetBytes(16);

            ICryptoTransform decryptor = aes.CreateDecryptor();

            using (MemoryStream ms = new MemoryStream(encryptedData))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}