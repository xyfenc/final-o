namespace CPMTools;

public partial class MainForm
{
    private void InitializeComponent()
    {
        var tabMain = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F) };
        var tabCpm1 = new TabPage("CPM1");
        var tabLog = new TabPage("Log");

        txtCpm1Email = new TextBox
        {
            PlaceholderText = "Email",
            Width = 200,
            Height = 28,
            Location = new Point(10, 10),
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };

        txtCpm1Pass = new TextBox
        {
            PlaceholderText = "Password",
            Width = 160,
            Height = 28,
            Location = new Point(220, 10),
            Anchor = AnchorStyles.Top | AnchorStyles.Left,
            PasswordChar = '•'
        };

        btnCpm1Login = new Button
        {
            Text = "Login",
            Width = 90,
            Height = 30,
            Location = new Point(390, 8),
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };

        btnCpm1Logout = new Button
        {
            Text = "Logout",
            Width = 90,
            Height = 30,
            Location = new Point(490, 8),
            Anchor = AnchorStyles.Top | AnchorStyles.Left,
            Enabled = false
        };

        btnCpm1Login.Click += btnCpm1Login_Click;
        btnCpm1Logout.Click += btnCpm1Logout_Click;

        var loginBar1 = new Panel { Dock = DockStyle.Top, Height = 52, Padding = new Padding(6) };
        loginBar1.Controls.AddRange(new Control[] { txtCpm1Email, txtCpm1Pass, btnCpm1Login, btnCpm1Logout });

        btnC1Money = new Button { Text = "💰 Money", Size = new Size(155, 40) };
        btnC1Coin = new Button { Text = "🪙 Coins", Size = new Size(155, 40) };
        btnC1Rank = new Button { Text = "👑 Rank", Size = new Size(155, 40) };
        btnC1Name = new Button { Text = "✏️ Change Name", Size = new Size(155, 40) };
        btnC1Fix = new Button { Text = "🔧 Fix Account", Size = new Size(155, 40) };
        btnC1UnlockAll = new Button { Text = "🚗 Unlock All", Size = new Size(155, 40) };
        btnC1UnlockOne = new Button { Text = "🚙 Unlock One", Size = new Size(155, 40) };
        btnC1Anim = new Button { Text = "🎞️ Animations", Size = new Size(155, 40) };
        btnC1Male = new Button { Text = "👕 Male Clothes", Size = new Size(155, 40) };
        btnC1Female = new Button { Text = "👗 Female Clothes", Size = new Size(155, 40) };

        btnC1Money.Click += btnC1Money_Click;
        btnC1Coin.Click += btnC1Coin_Click;
        btnC1Rank.Click += btnC1Rank_Click;
        btnC1Name.Click += btnC1Name_Click;
        btnC1Fix.Click += btnC1Fix_Click;
        btnC1UnlockAll.Click += btnC1UnlockAll_Click;
        btnC1UnlockOne.Click += btnC1UnlockOne_Click;
        btnC1Anim.Click += btnC1Anim_Click;
        btnC1Male.Click += btnC1Male_Click;
        btnC1Female.Click += btnC1Female_Click;

        var flowCpm1 = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Padding = new Padding(10),
            BorderStyle = BorderStyle.None,
            BackColor = Color.FromArgb(245, 245, 245)
        };
        flowCpm1.Controls.AddRange(new Control[] { btnC1Money, btnC1Coin, btnC1Rank, btnC1Name, btnC1Fix, btnC1UnlockAll, btnC1UnlockOne, btnC1Anim, btnC1Male, btnC1Female });

        tabCpm1.Controls.Add(loginBar1);
        tabCpm1.Controls.Add(flowCpm1);
        tabCpm1.Padding = new Padding(0);

        rtbLog = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            Font = new Font("Consolas", 10F),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };
        tabLog.Controls.Add(rtbLog);

        tabMain.TabPages.Add(tabCpm1);
        tabMain.TabPages.Add(tabLog);

        tabMain.SelectedIndex = 0;

        Controls.Add(tabMain);
        ClientSize = new Size(900, 600);
        MinimumSize = new Size(780, 520);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "CPM1 Tools — Desktop";

        ResumeLayout(false);
    }
}
