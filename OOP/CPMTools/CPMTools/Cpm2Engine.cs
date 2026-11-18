using System.Buffers.Binary;
using System.Text;

namespace CPMTools;

public static class Cpm2Engine
{
    public static byte[] MakeXorKey(string lid)
    {
        var chars = new List<char>(lid ?? string.Empty);
        if (chars.Count >= 7)
        {
            (chars[6], chars[4]) = (chars[4], chars[6]);
        }

        if (chars.Count >= 9)
        {
            chars.RemoveAt(8);
        }

        if (chars.Count > 0)
        {
            chars.Add(chars[0]);
        }

        return Encoding.UTF8.GetBytes(new string(chars.ToArray()));
    }

    public static string AesEncrypt(string plain, string uid)
    {
        var key = Encoding.UTF8.GetBytes((uid[..Math.Min(8, uid.Length)] + "12345678")[..16]);
        var iv = Encoding.UTF8.GetBytes((uid[..Math.Min(8, uid.Length)] + "01234567")[..16]);
        return Convert.ToBase64String(Crypto.AesEncrypt(Encoding.UTF8.GetBytes(plain), key, iv));
    }

    public static byte[] MpI32(int value)
    {
        var buffer = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(buffer, value);
        return buffer;
    }

    public static byte[] MpU16(ushort value)
    {
        var buffer = new byte[2];
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, value);
        return buffer;
    }

    public static byte[] MpI64(long value)
    {
        var buffer = new byte[8];
        BinaryPrimitives.WriteInt64LittleEndian(buffer, value);
        return buffer;
    }

    public static byte[] MpStr(string? value)
    {
        if (value is null)
        {
            return new byte[] { 255, 255, 255, 255 };
        }

        if (value == string.Empty)
        {
            return MpI32(0);
        }

        var encoded = Encoding.UTF8.GetBytes(value);
        var buffer = new byte[8 + encoded.Length];
        BinaryPrimitives.WriteInt32LittleEndian(buffer.AsSpan(0), ~encoded.Length);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.AsSpan(4), value.Length);
        encoded.CopyTo(buffer, 8);
        return buffer;
    }

    public static string MpEncode(byte[] data, string uid)
    {
        var xorKey = MakeXorKey(uid);
        if (xorKey.Length == 0)
        {
            throw new InvalidOperationException("No key");
        }

        var compressed = Crypto.BrCompress(data);
        return Convert.ToBase64String(Crypto.Xor(compressed, xorKey));
    }
}
