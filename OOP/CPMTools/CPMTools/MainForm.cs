using System.Text.Json;

namespace CPMTools;

public partial class MainForm : Form
{
    private readonly Cpm1Client _cpm1 = new();

    private TextBox txtCpm1Email = null!;
    private TextBox txtCpm1Pass = null!;
    private Button btnCpm1Login = null!;
    private Button btnCpm1Logout = null!;
    private Button btnC1Money = null!;
    private Button btnC1Coin = null!;
    private Button btnC1Rank = null!;
    private Button btnC1Name = null!;
    private Button btnC1Fix = null!;
    private Button btnC1UnlockAll = null!;
    private Button btnC1UnlockOne = null!;
    private Button btnC1Anim = null!;
    private Button btnC1Male = null!;
    private Button btnC1Female = null!;
    private RichTextBox rtbLog = null!;

    public MainForm()
    {
        InitializeComponent();
        AutoLogin();
    }

    private async void AutoLogin()
    {
        var saved1 = LocalDb.Load(1);
        if (saved1 is not null)
        {
            txtCpm1Email.Text = saved1.Value.email;
            txtCpm1Pass.Text = saved1.Value.password;
            await DoC1Login();
        }
    }

    private async Task DoC1Login()
    {
        Log("[CPM1] Logging in...");
        var result = await _cpm1.LoginAsync(txtCpm1Email.Text, txtCpm1Pass.Text);
        if (result.ok)
        {
            LocalDb.Save(1, txtCpm1Email.Text, txtCpm1Pass.Text, _cpm1.Auth, _cpm1.Uid);
            Log("[CPM1] Login Success");
            btnCpm1Login.Enabled = false;
            btnCpm1Logout.Enabled = true;
        }
        else
        {
            Log($"[CPM1] Failed: {result.msg}");
        }
    }

    private async void btnCpm1Login_Click(object? sender, EventArgs e) => await DoC1Login();

    private void btnCpm1Logout_Click(object? sender, EventArgs e)
    {
        LocalDb.Clear(1);
        txtCpm1Email.Clear();
        txtCpm1Pass.Clear();
        btnCpm1Login.Enabled = true;
        btnCpm1Logout.Enabled = false;
        Log("[CPM1] Logged out");
    }

    private async void btnC1Money_Click(object? sender, EventArgs e)
    {
        var amount = InputDialog.Ask("Enter Money (Max 50M):", "50000000");
        if (!int.TryParse(amount, out var value))
        {
            return;
        }

        Log($"[CPM1] Setting Money to {value:N0}...");
        var (ok, rec, err) = await _cpm1.GetPlayerAsync();
        if (!ok)
        {
            Log($"[CPM1] Load Failed: {err}");
            return;
        }

        rec.Money = Math.Clamp(value, 0, Env.MAX_MONEY);
        var saveResult = await _cpm1.SavePlayerAsync(rec, new HashSet<string> { "Money" });
        Log(saveResult.ok ? "[CPM1] Money Saved!" : $"[CPM1] Failed: {saveResult.msg}");
    }

    private async void btnC1Coin_Click(object? sender, EventArgs e)
    {
        var amount = InputDialog.Ask("Enter Coins (Max 500k):", "500000");
        if (!int.TryParse(amount, out var value))
        {
            return;
        }

        Log($"[CPM1] Setting Coins to {value:N0}...");
        var (ok, rec, err) = await _cpm1.GetPlayerAsync();
        if (!ok)
        {
            Log($"[CPM1] Load Failed: {err}");
            return;
        }

        rec.Coin = Math.Clamp(value, 0, Env.MAX_COIN);
        var saveResult = await _cpm1.SavePlayerAsync(rec, new HashSet<string> { "Coin" });
        Log(saveResult.ok ? "[CPM1] Coins Saved!" : $"[CPM1] Failed: {saveResult.msg}");
    }

    private async void btnC1Rank_Click(object? sender, EventArgs e)
    {
        Log("[CPM1] Applying Rank...");
        var payload = new { RatingData = new { time = 1e22, cars = 1e16, levels = 10000990000, race_win = 3e20 } };
        var raw = await Api.PostRawAsync(Env.CPM1_RANK_URL, new { data = JsonSerializer.Serialize(payload) }, new Dictionary<string, string> { ["Authorization"] = "demo-token" });
        Log(raw.Contains("result") ? "[CPM1] Rank Applied" : "[CPM1] Rank Failed");
    }

    private async void btnC1Name_Click(object? sender, EventArgs e)
    {
        var name = InputDialog.Ask("Enter New Name:");
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        Log($"[CPM1] Changing Name to {name}...");
        var (ok, rec, err) = await _cpm1.GetPlayerAsync();
        if (!ok)
        {
            Log($"[CPM1] Load Failed: {err}");
            return;
        }

        rec.Name = name;
        var saveResult = await _cpm1.SavePlayerAsync(rec, new HashSet<string> { "Name" });
        Log(saveResult.ok ? "[CPM1] Name Saved!" : $"[CPM1] Failed: {saveResult.msg}");
    }

    private async void btnC1Fix_Click(object? sender, EventArgs e)
    {
        Log("[CPM1] Fixing Account...");
        var (ok, rec, err) = await _cpm1.GetPlayerAsync();
        if (!ok)
        {
            Log($"[CPM1] Load Failed: {err}");
            return;
        }

        var saveResult = await _cpm1.SavePlayerAsync(rec, new HashSet<string> { "Floats", "Integers" });
        Log(saveResult.ok ? "[CPM1] Account Fixed!" : $"[CPM1] Failed: {saveResult.msg}");
    }

    private async void btnC1UnlockAll_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Env.CPM1_SOURCE_EMAIL))
        {
            Log("[CPM1] Error: You must set CPM1_SOURCE_EMAIL in Env.cs first.");
            return;
        }

        Log("[CPM1] Logging into source account...");
        var srcClient = new Cpm1Client();
        var login = await srcClient.LoginAsync(Env.CPM1_SOURCE_EMAIL, Env.CPM1_SOURCE_PASSWORD);
        if (!login.ok)
        {
            Log($"[CPM1] Source login failed: {login.msg}");
            return;
        }

        Log("[CPM1] Fetching source cars...");
        var (srcOk, srcCars, srcErr) = await srcClient.FetchCarsAsync();
        if (!srcOk)
        {
            Log($"[CPM1] Source fetch failed: {srcErr}");
            return;
        }

        Log("[CPM1] Fetching your cars...");
        var (tgtOk, tgtCars, _) = await _cpm1.FetchCarsAsync();

        var existingIds = new HashSet<int>();
        foreach (var car in tgtCars)
        {
            if (car.TryGetValue("CarID", out var idObj) && int.TryParse(idObj?.ToString(), out var cid))
            {
                existingIds.Add(cid);
            }
        }

        var toClone = srcCars.Where(c =>
            c.TryGetValue("CarID", out var idObj) &&
            int.TryParse(idObj?.ToString(), out var cid) &&
            !existingIds.Contains(cid) &&
            cid < Env.CAR_IDS_RANGE).ToList();

        if (toClone.Count == 0)
        {
            Log("[CPM1] You already own all source cars!");
            return;
        }

        Log($"[CPM1] Unlocking {toClone.Count} cars... (Please wait)");
        var saved = 0;
        var failed = 0;

        foreach (var car in toClone)
        {
            if (await _cpm1.SaveCarAsync(car))
            {
                saved++;
            }
            else
            {
                failed++;
            }
        }

        Log($"[CPM1] Unlock All Complete: {saved} saved, {failed} failed.");
    }

    private async void btnC1UnlockOne_Click(object? sender, EventArgs e)
    {
        var input = InputDialog.Ask("Enter Car IDs (comma separated, e.g. 10,11,12):", string.Empty);
        if (string.IsNullOrEmpty(input)) return;

        var ids = input.Split(',').Select(s => int.TryParse(s.Trim(), out var i) ? i : -1).Where(i => i != -1).ToHashSet();
        if (ids.Count == 0) return;

        var srcClient = new Cpm1Client();
        var login = await srcClient.LoginAsync(Env.CPM1_SOURCE_EMAIL, Env.CPM1_SOURCE_PASSWORD);
        if (!login.ok)
        {
            Log("[CPM1] Source login failed.");
            return;
        }

        var (srcOk, srcCars, _) = await srcClient.FetchCarsAsync();
        if (!srcOk) return;

        var toClone = srcCars.Where(c =>
            c.TryGetValue("CarID", out var idObj) &&
            int.TryParse(idObj?.ToString(), out var cid) &&
            ids.Contains(cid)).ToList();

        Log($"[CPM1] Unlocking {toClone.Count} specific cars...");
        var saved = 0;
        foreach (var car in toClone)
        {
            if (await _cpm1.SaveCarAsync(car)) saved++;
        }

        Log($"[CPM1] Unlock One Complete: {saved} saved.");
    }

    private async Task ApplyFeature(string label, string fieldName, Action<PlayerRecord> mutate)
    {
        Log($"[CPM1] {label}...");
        var (ok, rec, err) = await _cpm1.GetPlayerAsync();
        if (!ok)
        {
            Log($"[CPM1] Load Failed: {err}");
            return;
        }

        mutate(rec);
        var result = await _cpm1.SavePlayerAsync(rec, new HashSet<string> { fieldName });
        Log(result.ok ? $"[CPM1] {label} Saved!" : $"[CPM1] Failed: {result.msg}");
    }

    private async void btnC1Anim_Click(object? sender, EventArgs e)
    {
        await ApplyFeature("All Animations", "Animations", rec =>
        {
            var newAnims = new HashSet<int>(rec.Animations);
            for (var i = 0; i < 301; i++) newAnims.Add(i);
            rec.Animations = newAnims.OrderBy(x => x).ToList();
        });
    }

    private async void btnC1Male_Click(object? sender, EventArgs e)
    {
        await ApplyFeature("Male Clothes", "MaleEquip", rec =>
        {
            rec.MaleEquip = new Dictionary<string, object>
            {
                { "Gender", 0 },
                { "bag", Enumerable.Range(0, 101).ToList() },
                { "beard", Enumerable.Range(6, 15).Concat(new[] { 100 }).ToList() },
                { "cap", Enumerable.Range(3, 61).ToList() },
                { "face", new List<int> { 0, 1, 2, 100 } },
                { "glasses", Enumerable.Range(0, 10).Concat(new[] { 100 }).ToList() },
                { "gloves", Enumerable.Range(0, 6).Concat(new[] { 100 }).ToList() },
                { "hair", Enumerable.Range(3, 17).Concat(new[] { 100 }).ToList() },
                { "mask", Enumerable.Range(3, 6).Concat(new[] { 100 }).ToList() },
                { "pants", Enumerable.Range(0, 26).ToList() },
                { "shoes", Enumerable.Range(0, 31).ToList() },
                { "top", Enumerable.Range(2, 107).ToList() },
                { "SelectedEquipments", new List<int> { -1, 10, 19, 41, 100, 4, 20, 9, 22, 21, 74 } }
            };
        });
    }

    private async void btnC1Female_Click(object? sender, EventArgs e)
    {
        await ApplyFeature("Female Clothes", "FemaleEquip", rec =>
        {
            rec.FemaleEquip = new Dictionary<string, object>
            {
                { "Gender", 1 },
                { "bag", Enumerable.Range(0, 6).ToList() },
                { "beard", new List<int>() },
                { "cap", Enumerable.Range(3, 38).ToList() },
                { "face", new List<int> { 0 } },
                { "glasses", Enumerable.Range(0, 10).ToList() },
                { "gloves", new List<int> { 1 } },
                { "hair", new List<int> { 0, 7, 8, 9, 10 } },
                { "mask", Enumerable.Range(3, 5).ToList() },
                { "pants", Enumerable.Range(0, 12).ToList() },
                { "shoes", Enumerable.Range(3, 12).ToList() },
                { "top", Enumerable.Range(5, 75).ToList() },
                { "SelectedEquipments", new List<int> { 0, 0, -1, -1, -1, -1, -1, -1, 0, -1, -1 } }
            };
        });
    }

    private void Log(string msg)
    {
        if (rtbLog.InvokeRequired)
        {
            rtbLog.Invoke(() => Log(msg));
            return;
        }

        rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}{Environment.NewLine}");
        rtbLog.ScrollToCaret();
    }
}
