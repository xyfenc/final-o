using System.Text;
using System.Text.Json;

namespace CPMTools;

public static class Api
{
    public static readonly HttpClient Http = new(new SocketsHttpHandler
    {
        SslOptions = { RemoteCertificateValidationCallback = delegate { return true; } }
    });

    public static async Task<JsonElement> PostJsonAsync(string url, object body, Dictionary<string, string>? headers = null)
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            if (headers != null)
            {
                foreach (var (k, v) in headers)
                {
                    req.Headers.TryAddWithoutValidation(k, v);
                }
            }

            using var res = await Http.SendAsync(req);
            var text = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<JsonElement>(text);
        }
        catch
        {
            return JsonDocument.Parse("{}").RootElement.Clone();
        }
    }

    public static async Task<string> PostRawAsync(string url, object body, Dictionary<string, string>? headers = null)
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            if (headers != null)
            {
                foreach (var (k, v) in headers)
                {
                    req.Headers.TryAddWithoutValidation(k, v);
                }
            }

            using var res = await Http.SendAsync(req);
            return await res.Content.ReadAsStringAsync();
        }
        catch
        {
            return "{}";
        }
    }
}

public class Cpm1Client
{
    public string Auth = "", Uid = "", Email = "";

    private Dictionary<string, string> GetHeaders()
    {
        return new Dictionary<string, string>
        {
            ["Authorization"] = $"Bearer {Auth}",
            ["Content-Type"] = "application/json",
            ["Accept"] = "*/*",
            ["Accept-Encoding"] = "gzip",
            ["User-Agent"] = "UnityPlayer/2022.3.62f2 (UnityWebRequest/1.0, libcurl/8.10.1-DEV)",
            ["X-Unity-Version"] = "2022.3.62f2"
        };
    }

    public async Task<(bool ok, string msg)> LoginAsync(string email, string pw)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pw))
            return (false, "Email and password are required.");

        var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={Env.CPM1_API_KEY}";
        var raw = await Api.PostRawAsync(url, new { email, password = pw, returnSecureToken = true });
        using var doc = JsonDocument.Parse(raw);
        if (doc.RootElement.TryGetProperty("idToken", out var idToken))
        {
            Auth = idToken.GetString() ?? string.Empty;
            Uid = doc.RootElement.GetProperty("localId").GetString() ?? string.Empty;
            Email = email;
            return (true, string.Empty);
        }

        return (false, doc.RootElement.GetProperty("error").GetProperty("message").GetString() ?? "Authentication failed.");
    }

    public async Task<(bool ok, PlayerRecord rec, string err)> GetPlayerAsync()
    {
        var raw = await Api.PostRawAsync(Env.CPM1_LOAD_URL, new { data = (object?)null }, GetHeaders());
        System.Diagnostics.Debug.WriteLine($"[CPM1 Load Raw]: {raw}");

        using var doc = JsonDocument.Parse(raw);
        if (doc.RootElement.TryGetProperty("result", out var r) && r.ValueKind == JsonValueKind.String)
        {
            var b64 = r.GetString();
            if (string.IsNullOrEmpty(b64))
                return (false, new PlayerRecord(), raw);

            var buf = Convert.FromBase64String(b64);

            var decRaw = Crypto.BrDecompress(buf);
            if (decRaw != null)
            {
                var parsedRaw = Cpm1Engine.ParsePlayer(decRaw);
                if (parsedRaw != null) return (true, parsedRaw, "");
            }

            var p = Cpm1Engine.ParsePlayer(buf);
            if (p != null) return (true, p, "");

            var xor = Crypto.Xor(buf, Cpm1Engine.MakeXorKey(Uid));
            var dec = Crypto.BrDecompress(xor);
            if (dec != null)
            {
                var parsed = Cpm1Engine.ParsePlayer(dec);
                if (parsed != null) return (true, parsed, "");
            }
        }

        return (false, new PlayerRecord(), raw);
    }

    public async Task<(bool ok, string msg)> SavePlayerAsync(PlayerRecord rec, HashSet<string>? force = null)
    {
        var payload = Cpm1Engine.BuildPayload(rec, Uid, force);
        var raw = await Api.PostRawAsync(Env.CPM1_SAVE_URL, new { data = new { data = payload, deviceId = Uid[..Math.Min(8, Uid.Length)] } }, GetHeaders());
        return raw.Contains("result") ? (true, "Saved") : (false, raw);
    }

    public async Task<(bool ok, List<Dictionary<string, object>> cars, string msg)> FetchCarsAsync()
    {
        try
        {
            var raw = await Api.PostRawAsync(Env.CPM1_CARS_FETCH, new { data = "" }, GetHeaders());
            using var doc = JsonDocument.Parse(raw);
            if (doc.RootElement.TryGetProperty("result", out var r))
            {
                var json = r.ValueKind == JsonValueKind.String ? r.GetString() : r.GetRawText();
                var cars = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json ?? "[]") ?? new();
                return (true, cars, "");
            }

            return (true, new(), "");
        }
        catch (Exception ex)
        {
            return (false, new(), ex.Message);
        }
    }

    public async Task<bool> SaveCarAsync(Dictionary<string, object> car)
    {
        try
        {
            var enc = Cpm1Engine.EncryptCar(car, Uid);
            using var req = new HttpRequestMessage(HttpMethod.Post, Env.CPM1_CARS_SAVE);
            req.Content = new StringContent(JsonSerializer.Serialize(new { data = enc }), Encoding.UTF8, "application/json");
            foreach (var (k, v) in GetHeaders()) req.Headers.TryAddWithoutValidation(k, v);
            using var res = await Api.Http.SendAsync(req);
            return (int)res.StatusCode < 300;
        }
        catch
        {
            return false;
        }
    }
}

public class Cpm2Client
{
    public string Auth = "", Uid = "";

    public async Task<(bool ok, string msg)> LoginAsync(string email, string pw)
    {
        var raw = await Api.PostRawAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={Env.CPM2_API_KEY}", new { email, password = pw, returnSecureToken = true });
        using var doc = JsonDocument.Parse(raw);
        if (doc.RootElement.TryGetProperty("idToken", out var idToken))
        {
            Auth = idToken.GetString() ?? string.Empty;
            Uid = doc.RootElement.GetProperty("localId").GetString() ?? string.Empty;
            return (true, "");
        }

        return (false, doc.RootElement.GetProperty("error").GetProperty("message").GetString() ?? "Authentication failed.");
    }

    public async Task<(bool ok, string msg)> InjectMoneyAsync(long amount)
    {
        var wallet = Cpm2Engine.MpI32(1).Concat(Cpm2Engine.MpU16(0)).Concat(Cpm2Engine.MpI32(8)).Concat(Cpm2Engine.MpI64(amount)).ToArray();
        var enc = Cpm2Engine.MpEncode(wallet, Uid);
        var h = new Dictionary<string, string> { ["Authorization"] = $"{Auth}", ["Content-Type"] = "application/json" };
        var raw = await Api.PostRawAsync($"{Env.CPM2_API_BASE}/SaveWalletData23_1", new { data = enc }, h);
        return raw.Contains("code\":1") ? (true, "Injected") : (false, raw);
    }

    public async Task<(bool ok, string msg)> ApplyKingAsync()
    {
        try
        {
            var kingRating = new
            {
                cars = 100000,
                car_fix = 100000,
                car_collided = 100000,
                car_exchange = 100000,
                car_trade = 100000,
                car_wash = 100000,
                slicer_cut = 100000,
                drift_max = 100000,
                drift = 100000,
                cargo = 100000,
                delivery = 100000,
                taxi = 100000,
                levels = 100000,
                gifts = 100000,
                fuel = 100000,
                offroad = 100000,
                speed_banner = 100000,
                reactions = 100000,
                police = 100000,
                run = 100000,
                real_estate = 100000,
                t_distance = 100000,
                treasure = 100000,
                block_post = 100000,
                push_ups = 100000,
                burnt_tire = 100000,
                passanger_distance = 100000,
                time = 9999999999,
                race_win = 5000
            };

            var kingAll = new
            {
                general = kingRating,
                achievements = new Dictionary<string, int> { { "cars", 5 }, { "car_fix", 5 }, { "time", 5 }, { "race_win", 5 } },
                race_win = 5000,
                level = 120,
                score = 999.0,
                batches = new[] { 5, 15, 25, 45, 60, 120 }
            };

            var enc = Cpm2Engine.AesEncrypt(JsonSerializer.Serialize(kingAll), Uid);
            var h = new Dictionary<string, string> { ["Authorization"] = $"{Auth}", ["Content-Type"] = "application/json" };
            var raw = await Api.PostRawAsync($"{Env.CPM2_API_BASE}/SetUserRating22_1", new { data = enc }, h);
            return raw.Contains("code\":1") ? (true, "King Applied!") : (false, raw);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
