using System.Buffers.Binary;
using System.Text;

namespace CPMTools;

public class PlayerRecord
{
    public string Name = "", LocalId = "", AllData = "";
    public int Money, Coin;
    public List<float> Floats = new(), LevelsDoneTime = new();
    public List<int> Integers = new(), Fcar = new(), BoughtFsos = new(), PoliceLights = new(), Sirens = new(), EmojiPacks = new(), Animations = new(), Wheels = new();
    public Dictionary<string, object>? MaleEquip, FemaleEquip;
    public List<object> Friends = new(), Plates = new(), CarStatus = new();
    public Dictionary<int, int> Flags = new();
}

public static class Cpm1Engine
{
    public static byte[] MakeXorKey(string uid)
    {
        var c = new List<char>(uid ?? "");
        if (c.Count >= 9) (c[1], c[8]) = (c[8], c[1]);
        if (c.Count >= 3) c.RemoveAt(2);
        if (c.Count >= 5) c.Add(c[4]);
        var s = new string(c.ToArray());
        return s.Length == 0 ? Encoding.UTF8.GetBytes("0") : Encoding.UTF8.GetBytes(s);
    }

    public static byte[] MpStr(string? s)
    {
        if (s is null)
        {
            var nullBuf = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(nullBuf, -1);
            return nullBuf;
        }

        if (s == string.Empty)
        {
            var emptyBuf = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(emptyBuf, 0);
            return emptyBuf;
        }

        var e = Encoding.UTF8.GetBytes(s);
        var b = new byte[8 + e.Length];
        BinaryPrimitives.WriteInt32LittleEndian(b.AsSpan(0), -(e.Length) - 1);
        BinaryPrimitives.WriteInt32LittleEndian(b.AsSpan(4), e.Length);
        e.CopyTo(b.AsSpan(8));
        return b;
    }

    public static PlayerRecord? ParsePlayer(byte[] buf)
    {
        var r = new Reader(buf);
        if (r.Byte() == 0) return null;

        var p = new PlayerRecord
        {
            Name = r.String(),
            Money = r.Int(),
            Coin = r.Int(),
            LocalId = r.String(),
            BoughtFsos = r.List(r.Int)
        };

        r.List(() =>
        {
            r.Byte();
            r.String();
            r.String();
            r.String();
            return 0;
        });

        p.LevelsDoneTime = r.List(r.Float);
        p.Floats = r.List(r.Float);
        p.Integers = r.List(r.Int);
        p.Fcar = r.List(r.Int);
        r.List(r.Int);
        r.List(r.Int);
        r.List(r.Int);
        r.List(r.Int);
        p.MaleEquip = r.ParseEquip();
        p.FemaleEquip = r.ParseEquip();

        if (r.Byte() != 0)
        {
            p.Plates = r.List<object>(() =>
            {
                r.Byte();
                r.Int();
                r.Int();
                r.Int();
                r.List(() =>
                {
                    r.Byte();
                    r.List(() =>
                    {
                        r.Float();
                        r.Float();
                        r.Float();
                        return 0f;
                    });
                    r.List(r.String);
                    r.List(r.Float);
                    r.String();
                    return 0;
                });
                return 0;
            });
        }

        if (r.Byte() != 0) p.CarStatus = r.List<object>(() => 0);

        p.AllData = r.String();
        p.Flags = r.Dict();
        p.Animations = r.List(r.Int);
        p.EmojiPacks = r.List(r.Int);
        p.Wheels = r.List(r.Int);
        p.PoliceLights = r.List(r.Int);
        p.Sirens = r.List(r.Int);
        return p;
    }

    public static string BuildPayload(PlayerRecord rec, string uid, HashSet<string>? force = null)
    {
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);
        var fields = new List<(int fieldId, byte[] data)>();
        force ??= new HashSet<string>();

        void AddField(int fieldId, byte[] data) => fields.Add((fieldId, data));

        if (rec.Money != 0 || force.Contains("Money"))
        {
            var b = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(b, rec.Money);
            AddField(2, b);
        }
        if (rec.Coin != 0 || force.Contains("Coin"))
        {
            var b = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(b, rec.Coin);
            AddField(4, b);
        }
        if (!string.IsNullOrEmpty(rec.Name) || force.Contains("Name"))
        {
            using var ms2 = new MemoryStream();
            ms2.Write(MpStr(rec.Name));
            AddField(3, ms2.ToArray());
        }
        if (!string.IsNullOrEmpty(rec.LocalId) || force.Contains("LocalId"))
        {
            using var ms2 = new MemoryStream();
            ms2.Write(MpStr(rec.LocalId));
            AddField(1, ms2.ToArray());
        }

        if (force.Contains("Floats") && rec.Floats.Count > 0)
        {
            using var ms2 = new MemoryStream();
            using var w2 = new BinaryWriter(ms2);
            w2.Write(rec.Floats.Count);
            foreach (var f in rec.Floats) w2.Write(f);
            AddField(11, ms2.ToArray());
        }
        if (force.Contains("Integers") && rec.Integers.Count > 0)
        {
            using var ms2 = new MemoryStream();
            using var w2 = new BinaryWriter(ms2);
            w2.Write(rec.Integers.Count);
            foreach (var i in rec.Integers) w2.Write(i);
            AddField(12, ms2.ToArray());
        }
        if (force.Contains("LevelsDoneTime") && rec.LevelsDoneTime.Count > 0)
        {
            using var ms2 = new MemoryStream();
            using var w2 = new BinaryWriter(ms2);
            w2.Write(rec.LevelsDoneTime.Count);
            foreach (var f in rec.LevelsDoneTime) w2.Write(f);
            AddField(10, ms2.ToArray());
        }
        if (force.Contains("Animations") && rec.Animations.Count > 0)
        {
            using var ms2 = new MemoryStream();
            using var w2 = new BinaryWriter(ms2);
            w2.Write(rec.Animations.Count);
            foreach (var i in rec.Animations) w2.Write(i);
            AddField(46, ms2.ToArray());
        }
        if (force.Contains("Wheels") && rec.Wheels.Count > 0)
        {
            using var ms2 = new MemoryStream();
            using var w2 = new BinaryWriter(ms2);
            w2.Write(rec.Wheels.Count);
            foreach (var i in rec.Wheels) w2.Write(i);
            AddField(48, ms2.ToArray());
        }
        if (force.Contains("PoliceLights") && rec.PoliceLights.Count > 0)
        {
            using var ms2 = new MemoryStream();
            using var w2 = new BinaryWriter(ms2);
            w2.Write(rec.PoliceLights.Count);
            foreach (var i in rec.PoliceLights) w2.Write(i);
            AddField(7, ms2.ToArray());
        }
        if (force.Contains("Sirens") && rec.Sirens.Count > 0)
        {
            using var ms2 = new MemoryStream();
            using var w2 = new BinaryWriter(ms2);
            w2.Write(rec.Sirens.Count);
            foreach (var i in rec.Sirens) w2.Write(i);
            AddField(8, ms2.ToArray());
        }
        if (force.Contains("EmojiPacks") && rec.EmojiPacks.Count > 0)
        {
            using var ms2 = new MemoryStream();
            using var w2 = new BinaryWriter(ms2);
            w2.Write(rec.EmojiPacks.Count);
            foreach (var i in rec.EmojiPacks) w2.Write(i);
            AddField(18, ms2.ToArray());
        }

        if (force.Contains("MaleEquip") && rec.MaleEquip is not null)
        {
            using var ms2 = new MemoryStream();
            using var w2 = new BinaryWriter(ms2);
            WriteEquip(w2, rec.MaleEquip);
            AddField(41, ms2.ToArray());
        }
        if (force.Contains("FemaleEquip") && rec.FemaleEquip is not null)
        {
            using var ms2 = new MemoryStream();
            using var w2 = new BinaryWriter(ms2);
            WriteEquip(w2, rec.FemaleEquip);
            AddField(42, ms2.ToArray());
        }

        w.Write(fields.Count);
        foreach (var (fieldId, data) in fields)
        {
            w.Write((short)fieldId);
            w.Write(data.Length);
            w.Write(data);
        }

        var comp = Crypto.BrCompress(ms.ToArray());
        return Convert.ToBase64String(Crypto.Xor(comp, MakeXorKey(uid)));
    }

    private static void WriteEquip(BinaryWriter w, Dictionary<string, object> d)
    {
        if (d == null)
        {
            w.Write((byte)0);
            return;
        }

        w.Write((byte)13);
        string[] keys = { "hair", "face", "beard", "cap", "mask", "top", "gloves", "bag", "pants", "shoes", "glasses", "SelectedEquipments" };
        foreach (var key in keys)
        {
            var list = d.TryGetValue(key, out var value) && value is List<int> intList ? intList : new List<int>();
            w.Write(list.Count);
            foreach (var i in list) w.Write(i);
        }

        w.Write(Convert.ToInt32(d.GetValueOrDefault("Gender", 0)));
    }

    public static string EncryptCar(Dictionary<string, object> car, string uid)
    {
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);
        w.Write(car.Count);
        foreach (var kv in car)
        {
            w.Write((short)kv.Key.Length);
            w.Write(0);
        }

        var comp = Crypto.BrCompress(ms.ToArray());
        return "__ver1__" + Convert.ToBase64String(Crypto.Xor(comp, MakeXorKey(uid)));
    }
}

public sealed class Reader
{
    private readonly byte[] _buffer;
    public int Pos { get; private set; }

    public Reader(byte[] buffer)
    {
        _buffer = buffer;
    }

    public byte Byte() => Pos < _buffer.Length ? _buffer[Pos++] : (byte)0;

    public int Int()
    {
        if (Pos + 4 > _buffer.Length) return 0;
        var value = BinaryPrimitives.ReadInt32LittleEndian(_buffer.AsSpan(Pos));
        Pos += 4;
        return value;
    }

    public float Float()
    {
        if (Pos + 4 > _buffer.Length) return 0;
        var value = BinaryPrimitives.ReadSingleLittleEndian(_buffer.AsSpan(Pos));
        Pos += 4;
        return value;
    }

    public string String()
    {
        var m = Int();
        if (m == 0 || m == -1) return string.Empty;
        int l = m < -1 ? -m - 1 : m;
        if (m < -1) Int();
        if (Pos + l > _buffer.Length) return string.Empty;
        var s = Encoding.UTF8.GetString(_buffer, Pos, l);
        Pos += l;
        return s;
    }

    public List<T> List<T>(Func<T> fn)
    {
        int c = Int();
        var l = new List<T>();
        for (int i = 0; i < c && Pos < _buffer.Length; i++) l.Add(fn());
        return l;
    }

    public Dictionary<int, int> Dict()
    {
        int c = Int();
        var d = new Dictionary<int, int>();
        for (int i = 0; i < c; i++) d[Int()] = Int();
        return d;
    }

    public Dictionary<string, object>? ParseEquip()
    {
        if (Byte() == 0) return null;
        var d = new Dictionary<string, object>();
        string[] keys = { "hair", "face", "beard", "cap", "mask", "top", "gloves", "bag", "pants", "shoes", "glasses", "SelectedEquipments" };
        foreach (var key in keys)
        {
            var list = new List<int>();
            var count = Int();
            for (int i = 0; i < count; i++) list.Add(Int());
            d[key] = list;
        }

        d["Gender"] = Int();
        return d;
    }
}
