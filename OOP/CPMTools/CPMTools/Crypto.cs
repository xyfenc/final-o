using System.IO.Compression;
using System.Security.Cryptography;

namespace CPMTools;

public static class Crypto
{
    public static byte[] Xor(byte[] data, byte[] key)
    {
        if (key.Length == 0)
        {
            return data;
        }

        var result = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
        {
            result[i] = (byte)(data[i] ^ key[i % key.Length]);
        }

        return result;
    }

    public static byte[] BrCompress(byte[] data)
    {
        using var ms = new MemoryStream();
        using (var brotli = new BrotliStream(ms, CompressionLevel.Optimal, leaveOpen: true))
        {
            brotli.Write(data, 0, data.Length);
        }

        return ms.ToArray();
    }

    public static byte[]? BrDecompress(byte[] data)
    {
        try
        {
            using var ms = new MemoryStream(data);
            using var brotli = new BrotliStream(ms, CompressionMode.Decompress);
            using var output = new MemoryStream();
            brotli.CopyTo(output);
            return output.ToArray();
        }
        catch
        {
            return null;
        }
    }

    public static byte[] AesEncrypt(byte[] data, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key.Take(16).ToArray();
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        return encryptor.TransformFinalBlock(data, 0, data.Length);
    }

    public static byte[]? AesDecrypt(byte[] data, byte[] key, byte[] iv)
    {
        try
        {
            using var aes = Aes.Create();
            aes.Key = key.Take(16).ToArray();
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            return decryptor.TransformFinalBlock(data, 0, data.Length);
        }
        catch
        {
            return null;
        }
    }
}
