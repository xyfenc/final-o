using System.Text.Json;
using System.Text.RegularExpressions;

namespace CPMTools
{
    public partial class Form1 : Form
    {
        private readonly Cpm1Client _client = new();

        public Form1()
        {
            InitializeComponent();
            StyleFeatureButtons();

            btnLogin.Click += async (_, _) => await LoginAsync();
            btnLogout.Click += (_, _) => Logout();
            btnRefreshProfile.Click += async (_, _) => await FetchProfileAsync();

            btnMoney.Click += async (_, _) => await RunFeatureAsync(btnMoney, "Money", ApplyMoneyAsync);
            btnCoins.Click += async (_, _) => await RunFeatureAsync(btnCoins, "Coins", ApplyCoinsAsync);
            btnKingRank.Click += async (_, _) => await RunFeatureAsync(btnKingRank, "King Rank", ApplyKingRankAsync);
            btnChangeName.Click += async (_, _) => await RunFeatureAsync(btnChangeName, "Change Name", ApplyChangeNameAsync);
            btnChangeId.Click += async (_, _) => await RunFeatureAsync(btnChangeId, "Change ID", ApplyChangeIdAsync);
            btnFixAccount.Click += async (_, _) => await RunFeatureAsync(btnFixAccount, "Fix Account", ApplyFixAccountAsync);
            btnUnlockAll.Click += async (_, _) => await RunFeatureAsync(btnUnlockAll, "Unlock All", ApplyUnlockAllAsync);
            btnUnlockOne.Click += async (_, _) => await RunFeatureAsync(btnUnlockOne, "Unlock One", ApplyUnlockOneAsync);
            btnW16Engine.Click += async (_, _) => await RunFeatureAsync(btnW16Engine, "W16 Engine", ApplyW16EngineAsync);
            btnUnlimitedFuel.Click += async (_, _) => await RunFeatureAsync(btnUnlimitedFuel, "Unlimited Fuel", ApplyUnlimitedFuelAsync);
            btnArmor.Click += async (_, _) => await RunFeatureAsync(btnArmor, "Armor", ApplyArmorAsync);
            btnAllWheels.Click += async (_, _) => await RunFeatureAsync(btnAllWheels, "All Wheels", ApplyAllWheelsAsync);
            btnAllAnimations.Click += async (_, _) => await RunFeatureAsync(btnAllAnimations, "All Animations", ApplyAllAnimationsAsync);
            btnMaleClothes.Click += async (_, _) => await RunFeatureAsync(btnMaleClothes, "Male Clothes", ApplyMaleClothesAsync);
            btnFemaleClothes.Click += async (_, _) => await RunFeatureAsync(btnFemaleClothes, "Female Clothes", ApplyFemaleClothesAsync);

            _ = AutoLoginAsync();
        }

        private void StyleFeatureButtons()
        {
            var buttons = new[]
            {
                btnMoney, btnCoins, btnKingRank, btnChangeName, btnChangeId,
                btnFixAccount, btnUnlockAll, btnUnlockOne, btnW16Engine,
                btnUnlimitedFuel, btnArmor, btnAllWheels, btnAllAnimations,
                btnMaleClothes, btnFemaleClothes
            };

            foreach (var button in buttons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = Color.FromArgb(205, 153, 99);
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 187, 135);
                button.BackColor = Color.FromArgb(255, 247, 235);
                button.ForeColor = Color.FromArgb(58, 36, 24);
                button.Font = new Font("Segoe UI Semibold", 10F);
                button.Margin = new Padding(4);
                button.Size = new Size(140, 42);
            }

            btnUnlockAll.BackColor = Color.FromArgb(0, 122, 204);
            btnUnlockAll.ForeColor = Color.White;
            btnUnlockAll.FlatAppearance.BorderColor = Color.FromArgb(0, 150, 240);
            SetFeatureAvailability(false);
        }

        private void SetFeatureAvailability(bool enabled)
        {
            btnRefreshProfile.Enabled = enabled;
            btnMoney.Enabled = enabled;
            btnCoins.Enabled = enabled;
            btnKingRank.Enabled = enabled;
            btnChangeName.Enabled = enabled;
            btnChangeId.Enabled = enabled;
            btnFixAccount.Enabled = enabled;
            btnUnlockAll.Enabled = enabled;
            btnUnlockOne.Enabled = enabled;
            btnW16Engine.Enabled = enabled;
            btnUnlimitedFuel.Enabled = enabled;
            btnArmor.Enabled = enabled;
            btnAllWheels.Enabled = enabled;
            btnAllAnimations.Enabled = enabled;
            btnMaleClothes.Enabled = enabled;
            btnFemaleClothes.Enabled = enabled;
        }

        private async Task AutoLoginAsync()
        {
            try
            {
                var saved = await Task.Run(() => LocalDb.Load(1));
                if (saved is null || IsDisposed)
                {
                    return;
                }

                txtEmail.Text = saved.Value.email;
                txtPassword.Text = saved.Value.password;
                await LoginAsync();
            }
            catch (Exception ex)
            {
                Log($"ERROR: Auto-login failed: {ex.Message}");
            }
        }

        private async Task LoginAsync()
        {
            var email = txtEmail.Text.Trim();
            var password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                Log("ERROR: Email and password are required.");
                return;
            }

            try
            {
                var (ok, msg) = await _client.LoginAsync(email, password);
                if (!ok)
                {
                    Log($"ERROR: {msg}");
                    return;
                }

                await Task.Run(() => LocalDb.Save(1, email, password, _client.Auth, _client.Uid));
                SetFeatureAvailability(true);
                Log("SUCCESS: Login successful!");
                await FetchProfileAsync();
            }
            catch (Exception ex)
            {
                Log($"ERROR: {ex.Message}");
            }
        }

        private void Logout()
        {
            LocalDb.Clear(1);
            _client.Auth = string.Empty;
            _client.Uid = string.Empty;
            _client.Email = string.Empty;
            SetFeatureAvailability(false);
            txtEmail.Clear();
            txtPassword.Clear();
            ClearProfile();
            Log("SUCCESS: Logged out.");
        }

        private async Task FetchProfileAsync()
        {
            btnRefreshProfile.Enabled = false;
            UseWaitCursor = true;
            try
            {
                if (string.IsNullOrWhiteSpace(_client.Auth) || string.IsNullOrWhiteSpace(_client.Uid))
                {
                    Log("[CPM1] Please log in before refreshing the profile.");
                    return;
                }

                Log("[CPM1] Fetching live profile...");
                var (ok, rec, err) = await _client.GetPlayerAsync();
                if (!ok)
                {
                    Log($"[CPM1] ERROR: Failed to load profile. {err}");
                    return;
                }

                var (carsOk, cars, carsErr) = await _client.FetchCarsAsync();
                if (!carsOk)
                    Log($"[CPM1] Cars fetch failed: {carsErr}");

                lblProfileName.Text = $"Name: {rec.Name}";
                lblProfileID.Text = $"ID: {rec.LocalId}";
                lblProfileMoney.Text = $"Money: {rec.Money:N0}";
                lblProfileCoins.Text = $"Coins: {rec.Coin:N0}";
                lblProfileCars.Text = $"Cars Owned: {(carsOk ? cars.Count : 0)}";
                Log($"[CPM1] Live Stats -> Money: {rec.Money:N0} | Coins: {rec.Coin:N0} | Cars: {(carsOk ? cars.Count : 0)}");
            }
            catch (Exception ex)
            {
                Log($"[CPM1] ERROR: {ex.Message}");
            }
            finally
            {
                if (!btnRefreshProfile.IsDisposed)
                    btnRefreshProfile.Enabled = true;
                UseWaitCursor = false;
                SetFeatureAvailability(!string.IsNullOrWhiteSpace(_client.Auth) && !string.IsNullOrWhiteSpace(_client.Uid));
            }
        }

        private void ClearProfile()
        {
            lblProfileName.Text = "Name: -";
            lblProfileID.Text = "ID: -";
            lblProfileMoney.Text = "Money: -";
            lblProfileCoins.Text = "Coins: -";
            lblProfileCars.Text = "Cars Owned: -";
        }

        private async Task RunFeatureAsync(Button button, string featureName, Func<Task<bool>> action)
        {
            button.Enabled = false;
            UseWaitCursor = true;
            Log($"[CPM1] Processing {featureName}...");

            try
            {
                var success = await action();
                if (success)
                {
                    Log($"SUCCESS: {featureName} applied successfully!");
                }
            }
            catch (Exception ex)
            {
                Log($"ERROR: {ex.Message}");
            }
            finally
            {
                if (!button.IsDisposed)
                {
                    button.Enabled = true;
                }

                UseWaitCursor = false;
                SetFeatureAvailability(!string.IsNullOrWhiteSpace(_client.Auth) && !string.IsNullOrWhiteSpace(_client.Uid));
            }
        }

        private async Task<bool> EnsureLoggedInAsync()
        {
            if (!string.IsNullOrWhiteSpace(_client.Auth) && !string.IsNullOrWhiteSpace(_client.Uid))
            {
                return true;
            }

            var email = txtEmail.Text.Trim();
            var password = txtPassword.Text.Trim();
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                Log("ERROR: Please log in before using a feature.");
                return false;
            }

            var (ok, msg) = await _client.LoginAsync(email, password);
            if (!ok)
            {
                Log($"ERROR: {msg}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyMoneyAsync()
        {
            var raw = InputDialog.Ask("Enter amount:", "50000000");
            if (string.IsNullOrWhiteSpace(raw))
            {
                Log("ERROR: Invalid input. Please enter a number.");
                return false;
            }

            var sanitized = raw.Replace(",", string.Empty).Trim();
            if (!int.TryParse(sanitized, out var value))
            {
                Log("ERROR: Invalid input. Please enter a number.");
                return false;
            }

            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (ok, rec, err) = await _client.GetPlayerAsync();
            if (!ok)
            {
                Log($"ERROR: Unable to load the current player record. Server said: {err}");
                return false;
            }

            rec.Money = Math.Clamp(value, 0, Env.MAX_MONEY);
            var save = await _client.SavePlayerAsync(rec, new HashSet<string> { "Money" });
            if (!save.ok)
            {
                Log($"ERROR: {save.msg}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyCoinsAsync()
        {
            var raw = InputDialog.Ask("Enter coins:", "500000");
            if (string.IsNullOrWhiteSpace(raw))
            {
                Log("ERROR: Invalid input. Please enter a number.");
                return false;
            }

            var sanitized = raw.Replace(",", string.Empty).Trim();
            if (!int.TryParse(sanitized, out var value))
            {
                Log("ERROR: Invalid input. Please enter a number.");
                return false;
            }

            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (ok, rec, err) = await _client.GetPlayerAsync();
            if (!ok)
            {
                Log($"ERROR: Unable to load the current player record. Server said: {err}");
                return false;
            }

            rec.Coin = Math.Clamp(value, 0, Env.MAX_COIN);
            var save = await _client.SavePlayerAsync(rec, new HashSet<string> { "Coin" });
            if (!save.ok)
            {
                Log($"ERROR: {save.msg}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyKingRankAsync()
        {
            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var payload = new { RatingData = new { time = 1e22, cars = 1e16, levels = 10000990000, race_win = 3e20 } };
            var raw = await Api.PostRawAsync(
                Env.CPM1_RANK_URL,
                new { data = JsonSerializer.Serialize(payload) },
                new Dictionary<string, string> { ["Authorization"] = "demo-token" });

            if (!raw.Contains("result", StringComparison.OrdinalIgnoreCase))
            {
                Log($"ERROR: {raw}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyChangeNameAsync()
        {
            var newName = InputDialog.Ask("Enter new name:");
            if (string.IsNullOrWhiteSpace(newName))
            {
                Log("ERROR: Name cannot be empty.");
                return false;
            }

            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (ok, rec, err) = await _client.GetPlayerAsync();
            if (!ok)
            {
                Log($"ERROR: Unable to load the current player record. Server said: {err}");
                return false;
            }

            rec.Name = newName.Trim();
            var save = await _client.SavePlayerAsync(rec, new HashSet<string> { "Name" });
            if (!save.ok)
            {
                Log($"ERROR: {save.msg}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyChangeIdAsync()
        {
            var newId = InputDialog.Ask("Enter new ID (8 chars):");
            if (string.IsNullOrWhiteSpace(newId))
            {
                Log("ERROR: Invalid ID. Please enter exactly 8 characters.");
                return false;
            }

            newId = newId.Trim().ToUpperInvariant();
            if (!Regex.IsMatch(newId, "^[A-Z0-9]{8}$"))
            {
                Log("ERROR: Invalid ID. Please enter exactly 8 characters.");
                return false;
            }

            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (ok, rec, err) = await _client.GetPlayerAsync();
            if (!ok)
            {
                Log($"ERROR: Unable to load the current player record. Server said: {err}");
                return false;
            }

            rec.LocalId = newId;
            var save = await _client.SavePlayerAsync(rec, new HashSet<string> { "LocalId" });
            if (!save.ok)
            {
                Log($"ERROR: {save.msg}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyFixAccountAsync()
        {
            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (ok, rec, err) = await _client.GetPlayerAsync();
            if (!ok)
            {
                Log($"ERROR: Unable to load the current player record. Server said: {err}");
                return false;
            }

            rec.Floats = Enumerable.Range(0, 80).Select(_ => 0f).ToList();
            rec.Integers = Enumerable.Range(0, 80).Select(_ => 0).ToList();
            var save = await _client.SavePlayerAsync(rec, new HashSet<string> { "Floats", "Integers" });
            if (!save.ok)
            {
                Log($"ERROR: {save.msg}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyUnlockAllAsync()
        {
            if (string.IsNullOrWhiteSpace(Env.CPM1_SOURCE_EMAIL) || string.IsNullOrWhiteSpace(Env.CPM1_SOURCE_PASSWORD))
            {
                Log("ERROR: Source account credentials are not configured.");
                return false;
            }

            var sourceClient = new Cpm1Client();
            var login = await sourceClient.LoginAsync(Env.CPM1_SOURCE_EMAIL, Env.CPM1_SOURCE_PASSWORD);
            if (!login.ok)
            {
                Log($"ERROR: Source login failed: {login.msg}");
                return false;
            }

            var (srcOk, srcCars, srcErr) = await sourceClient.FetchCarsAsync();
            if (!srcOk)
            {
                Log($"ERROR: {srcErr}");
                return false;
            }

            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (_, targetCars, _) = await _client.FetchCarsAsync();
            var existing = new HashSet<int>();
            foreach (var car in targetCars)
            {
                if (car.TryGetValue("CarID", out var idObj) && int.TryParse(idObj?.ToString(), out var id))
                {
                    existing.Add(id);
                }
            }

            var toSave = srcCars
                .Where(car => car.TryGetValue("CarID", out var idObj) && int.TryParse(idObj?.ToString(), out var id) && !existing.Contains(id) && id < Env.CAR_IDS_RANGE)
                .ToList();

            if (toSave.Count == 0)
            {
                Log("ERROR: No new cars are available to unlock from the source account.");
                return false;
            }

            var successCount = 0;
            foreach (var car in toSave)
            {
                if (await _client.SaveCarAsync(car))
                {
                    successCount++;
                }
            }

            if (successCount == 0)
            {
                Log("ERROR: Failed to save any cars from the source account.");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyUnlockOneAsync()
        {
            var raw = InputDialog.Ask("Enter car IDs (comma separated, e.g. 10,11,12):");
            if (string.IsNullOrWhiteSpace(raw))
            {
                Log("ERROR: Invalid input. Please enter a comma-separated list of car IDs.");
                return false;
            }

            var ids = raw
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(part => int.TryParse(part, out var id) ? id : -1)
                .Where(id => id >= 0)
                .Distinct()
                .ToList();

            if (ids.Count == 0)
            {
                Log("ERROR: Invalid input. Please enter a comma-separated list of car IDs.");
                return false;
            }

            var sourceClient = new Cpm1Client();
            var login = await sourceClient.LoginAsync(Env.CPM1_SOURCE_EMAIL, Env.CPM1_SOURCE_PASSWORD);
            if (!login.ok)
            {
                Log($"ERROR: Source login failed: {login.msg}");
                return false;
            }

            var (srcOk, srcCars, srcErr) = await sourceClient.FetchCarsAsync();
            if (!srcOk)
            {
                Log($"ERROR: {srcErr}");
                return false;
            }

            var toSave = srcCars
                .Where(car => car.TryGetValue("CarID", out var idObj) && int.TryParse(idObj?.ToString(), out var id) && ids.Contains(id))
                .ToList();

            if (toSave.Count == 0)
            {
                Log("ERROR: No matching cars were found for the requested IDs.");
                return false;
            }

            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var saved = 0;
            foreach (var car in toSave)
            {
                if (await _client.SaveCarAsync(car))
                {
                    saved++;
                }
            }

            if (saved == 0)
            {
                Log("ERROR: Failed to save the selected cars to the target account.");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyW16EngineAsync()
        {
            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (ok, rec, err) = await _client.GetPlayerAsync();
            if (!ok)
            {
                Log($"ERROR: Unable to load the current player record. Server said: {err}");
                return false;
            }

            while (rec.Floats.Count <= 32)
            {
                rec.Floats.Add(0f);
            }

            rec.Floats[32] = 1.0f;
            var save = await _client.SavePlayerAsync(rec, new HashSet<string> { "Floats" });
            if (!save.ok)
            {
                Log($"ERROR: {save.msg}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyUnlimitedFuelAsync()
        {
            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (ok, rec, err) = await _client.GetPlayerAsync();
            if (!ok)
            {
                Log($"ERROR: Unable to load the current player record. Server said: {err}");
                return false;
            }

            while (rec.Floats.Count <= 3)
            {
                rec.Floats.Add(0f);
            }

            rec.Floats[3] = 1.0f;
            var save = await _client.SavePlayerAsync(rec, new HashSet<string> { "Floats" });
            if (!save.ok)
            {
                Log($"ERROR: {save.msg}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyArmorAsync()
        {
            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (ok, rec, err) = await _client.GetPlayerAsync();
            if (!ok)
            {
                Log($"ERROR: Unable to load the current player record. Server said: {err}");
                return false;
            }

            while (rec.Floats.Count <= 34)
            {
                rec.Floats.Add(0f);
            }

            rec.Floats[34] = 1.0f;
            var save = await _client.SavePlayerAsync(rec, new HashSet<string> { "Floats" });
            if (!save.ok)
            {
                Log($"ERROR: {save.msg}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyAllWheelsAsync()
        {
            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (ok, rec, err) = await _client.GetPlayerAsync();
            if (!ok)
            {
                Log($"ERROR: Unable to load the current player record. Server said: {err}");
                return false;
            }

            rec.Wheels = Enumerable.Range(0, 120).ToList();
            for (var i = 0; i < rec.Integers.Count; i++)
            {
                rec.Integers[i] = 1;
            }

            if (rec.Integers.Count < 20)
            {
                rec.Integers.AddRange(Enumerable.Repeat(1, 20 - rec.Integers.Count));
            }

            var save = await _client.SavePlayerAsync(rec, new HashSet<string> { "Wheels", "Integers" });
            if (!save.ok)
            {
                Log($"ERROR: {save.msg}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyAllAnimationsAsync()
        {
            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (ok, rec, err) = await _client.GetPlayerAsync();
            if (!ok)
            {
                Log($"ERROR: Unable to load the current player record. Server said: {err}");
                return false;
            }

            var anims = new HashSet<int>(rec.Animations);
            for (var i = 0; i < 301; i++)
            {
                anims.Add(i);
            }

            rec.Animations = anims.OrderBy(x => x).ToList();
            var save = await _client.SavePlayerAsync(rec, new HashSet<string> { "Animations" });
            if (!save.ok)
            {
                Log($"ERROR: {save.msg}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyMaleClothesAsync()
        {
            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (ok, rec, err) = await _client.GetPlayerAsync();
            if (!ok)
            {
                Log($"ERROR: Unable to load the current player record. Server said: {err}");
                return false;
            }

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

            var save = await _client.SavePlayerAsync(rec, new HashSet<string> { "MaleEquip" });
            if (!save.ok)
            {
                Log($"ERROR: {save.msg}");
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyFemaleClothesAsync()
        {
            if (!await EnsureLoggedInAsync())
            {
                return false;
            }

            var (ok, rec, err) = await _client.GetPlayerAsync();
            if (!ok)
            {
                Log($"ERROR: Unable to load the current player record. Server said: {err}");
                return false;
            }

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

            var save = await _client.SavePlayerAsync(rec, new HashSet<string> { "FemaleEquip" });
            if (!save.ok)
            {
                Log($"ERROR: {save.msg}");
                return false;
            }

            return true;
        }

        private void Log(string msg)
        {
            if (rtbLog.IsDisposed)
            {
                return;
            }

            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action<string>(Log), msg);
                return;
            }

            rtbLog.AppendText($"{DateTime.Now:HH:mm:ss} {msg}{Environment.NewLine}");
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.ScrollToCaret();
        }
    }
}
