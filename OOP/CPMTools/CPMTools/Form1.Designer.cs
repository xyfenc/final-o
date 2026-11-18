#nullable enable

namespace CPMTools
{
    partial class Form1
    {
        private System.ComponentModel.IContainer? components = null;
        private Panel topPanel = null!;
        private FlowLayoutPanel topControls = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;
        private Button btnLogout = null!;
        private Button btnRefreshProfile = null!;
        private Panel profilePanel = null!;
        private Label lblProfileName = null!;
        private Label lblProfileID = null!;
        private Label lblProfileMoney = null!;
        private Label lblProfileCoins = null!;
        private Label lblProfileCars = null!;
        private FlowLayoutPanel flowFeatures = null!;
        private GroupBox economyGroup = null!;
        private GroupBox accountGroup = null!;
        private GroupBox upgradesGroup = null!;
        private GroupBox cosmeticsGroup = null!;
        private FlowLayoutPanel economyActions = null!;
        private FlowLayoutPanel accountActions = null!;
        private FlowLayoutPanel upgradesActions = null!;
        private FlowLayoutPanel cosmeticsActions = null!;
        private Panel bottomPanel = null!;
        private RichTextBox rtbLog = null!;
        private Button btnMoney = null!;
        private Button btnCoins = null!;
        private Button btnKingRank = null!;
        private Button btnChangeName = null!;
        private Button btnChangeId = null!;
        private Button btnFixAccount = null!;
        private Button btnUnlockAll = null!;
        private Button btnUnlockOne = null!;
        private Button btnW16Engine = null!;
        private Button btnUnlimitedFuel = null!;
        private Button btnArmor = null!;
        private Button btnAllWheels = null!;
        private Button btnAllAnimations = null!;
        private Button btnMaleClothes = null!;
        private Button btnFemaleClothes = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            topPanel = new Panel();
            topControls = new FlowLayoutPanel();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnLogout = new Button();
            btnRefreshProfile = new Button();
            profilePanel = new Panel();
            lblProfileCars = new Label();
            lblProfileCoins = new Label();
            lblProfileMoney = new Label();
            lblProfileID = new Label();
            lblProfileName = new Label();
            flowFeatures = new FlowLayoutPanel();
            economyGroup = new GroupBox();
            economyActions = new FlowLayoutPanel();
            btnMoney = new Button();
            btnCoins = new Button();
            accountGroup = new GroupBox();
            accountActions = new FlowLayoutPanel();
            btnKingRank = new Button();
            btnChangeName = new Button();
            btnChangeId = new Button();
            btnFixAccount = new Button();
            upgradesGroup = new GroupBox();
            upgradesActions = new FlowLayoutPanel();
            btnUnlockAll = new Button();
            btnUnlockOne = new Button();
            btnW16Engine = new Button();
            btnUnlimitedFuel = new Button();
            btnArmor = new Button();
            btnAllWheels = new Button();
            cosmeticsGroup = new GroupBox();
            cosmeticsActions = new FlowLayoutPanel();
            btnAllAnimations = new Button();
            btnMaleClothes = new Button();
            btnFemaleClothes = new Button();
            bottomPanel = new Panel();
            rtbLog = new RichTextBox();
            topPanel.SuspendLayout();
            topControls.SuspendLayout();
            profilePanel.SuspendLayout();
            flowFeatures.SuspendLayout();
            economyGroup.SuspendLayout();
            economyActions.SuspendLayout();
            accountGroup.SuspendLayout();
            accountActions.SuspendLayout();
            upgradesGroup.SuspendLayout();
            upgradesActions.SuspendLayout();
            cosmeticsGroup.SuspendLayout();
            cosmeticsActions.SuspendLayout();
            bottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(58, 36, 24);
            topPanel.Controls.Add(topControls);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Padding = new Padding(18, 16, 18, 12);
            topPanel.Size = new Size(1180, 66);
            topPanel.TabIndex = 3;
            // 
            // topControls
            // 
            topControls.Controls.Add(txtEmail);
            topControls.Controls.Add(txtPassword);
            topControls.Controls.Add(btnLogin);
            topControls.Controls.Add(btnLogout);
            topControls.Dock = DockStyle.Fill;
            topControls.Location = new Point(18, 16);
            topControls.Name = "topControls";
            topControls.Size = new Size(1144, 38);
            topControls.TabIndex = 0;
            topControls.WrapContents = false;
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.FromArgb(255, 247, 235);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.ForeColor = Color.FromArgb(58, 36, 24);
            txtEmail.Location = new Point(0, 3);
            txtEmail.Margin = new Padding(0, 3, 8, 3);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Email";
            txtEmail.Size = new Size(260, 25);
            txtEmail.TabIndex = 0;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.FromArgb(255, 247, 235);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.ForeColor = Color.FromArgb(58, 36, 24);
            txtPassword.Location = new Point(268, 3);
            txtPassword.Margin = new Padding(0, 3, 8, 3);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '•';
            txtPassword.PlaceholderText = "Password";
            txtPassword.Size = new Size(220, 25);
            txtPassword.TabIndex = 1;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(46, 160, 110);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(496, 2);
            btnLogin.Margin = new Padding(0, 2, 8, 2);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(100, 30);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.FromArgb(190, 70, 82);
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(604, 2);
            btnLogout.Margin = new Padding(0, 2, 8, 2);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(100, 30);
            btnLogout.TabIndex = 3;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = false;
            // 
            // btnRefreshProfile
            // 
            btnRefreshProfile.BackColor = Color.FromArgb(200, 135, 74);
            btnRefreshProfile.Dock = DockStyle.Top;
            btnRefreshProfile.FlatAppearance.BorderSize = 0;
            btnRefreshProfile.FlatStyle = FlatStyle.Flat;
            btnRefreshProfile.ForeColor = Color.White;
            btnRefreshProfile.Location = new Point(18, 18);
            btnRefreshProfile.Name = "btnRefreshProfile";
            btnRefreshProfile.Size = new Size(264, 38);
            btnRefreshProfile.TabIndex = 5;
            btnRefreshProfile.Text = "Refresh Profile";
            btnRefreshProfile.UseVisualStyleBackColor = false;
            // 
            // profilePanel
            // 
            profilePanel.BackColor = Color.FromArgb(246, 231, 210);
            profilePanel.Controls.Add(lblProfileCars);
            profilePanel.Controls.Add(lblProfileCoins);
            profilePanel.Controls.Add(lblProfileMoney);
            profilePanel.Controls.Add(lblProfileID);
            profilePanel.Controls.Add(lblProfileName);
            profilePanel.Controls.Add(btnRefreshProfile);
            profilePanel.Dock = DockStyle.Right;
            profilePanel.Location = new Point(880, 66);
            profilePanel.Name = "profilePanel";
            profilePanel.Padding = new Padding(18);
            profilePanel.Size = new Size(300, 494);
            profilePanel.TabIndex = 2;
            // 
            // lblProfileCars
            // 
            lblProfileCars.AutoSize = true;
            lblProfileCars.Dock = DockStyle.Top;
            lblProfileCars.ForeColor = Color.FromArgb(177, 108, 48);
            lblProfileCars.Location = new Point(18, 188);
            lblProfileCars.Name = "lblProfileCars";
            lblProfileCars.Padding = new Padding(0, 12, 0, 0);
            lblProfileCars.Size = new Size(97, 31);
            lblProfileCars.TabIndex = 0;
            lblProfileCars.Text = "Cars Owned: -";
            // 
            // lblProfileCoins
            // 
            lblProfileCoins.AutoSize = true;
            lblProfileCoins.Dock = DockStyle.Top;
            lblProfileCoins.ForeColor = Color.FromArgb(177, 108, 48);
            lblProfileCoins.Location = new Point(18, 157);
            lblProfileCoins.Name = "lblProfileCoins";
            lblProfileCoins.Padding = new Padding(0, 12, 0, 0);
            lblProfileCoins.Size = new Size(56, 31);
            lblProfileCoins.TabIndex = 1;
            lblProfileCoins.Text = "Coins: -";
            // 
            // lblProfileMoney
            // 
            lblProfileMoney.AutoSize = true;
            lblProfileMoney.Dock = DockStyle.Top;
            lblProfileMoney.ForeColor = Color.FromArgb(46, 160, 110);
            lblProfileMoney.Location = new Point(18, 120);
            lblProfileMoney.Name = "lblProfileMoney";
            lblProfileMoney.Padding = new Padding(0, 18, 0, 0);
            lblProfileMoney.Size = new Size(65, 37);
            lblProfileMoney.TabIndex = 2;
            lblProfileMoney.Text = "Money: -";
            // 
            // lblProfileID
            // 
            lblProfileID.AutoSize = true;
            lblProfileID.Dock = DockStyle.Top;
            lblProfileID.ForeColor = Color.FromArgb(110, 78, 55);
            lblProfileID.Location = new Point(18, 89);
            lblProfileID.Name = "lblProfileID";
            lblProfileID.Padding = new Padding(0, 12, 0, 0);
            lblProfileID.Size = new Size(36, 31);
            lblProfileID.TabIndex = 3;
            lblProfileID.Text = "ID: -";
            // 
            // lblProfileName
            // 
            lblProfileName.AutoSize = true;
            lblProfileName.Dock = DockStyle.Top;
            lblProfileName.ForeColor = Color.FromArgb(58, 36, 24);
            lblProfileName.Location = new Point(18, 56);
            lblProfileName.Name = "lblProfileName";
            lblProfileName.Padding = new Padding(0, 14, 0, 0);
            lblProfileName.Size = new Size(58, 33);
            lblProfileName.TabIndex = 4;
            lblProfileName.Text = "Name: -";
            // 
            // flowFeatures
            // 
            flowFeatures.AutoScroll = true;
            flowFeatures.BackColor = Color.FromArgb(255, 247, 235);
            flowFeatures.Controls.Add(economyGroup);
            flowFeatures.Controls.Add(accountGroup);
            flowFeatures.Controls.Add(upgradesGroup);
            flowFeatures.Controls.Add(cosmeticsGroup);
            flowFeatures.Dock = DockStyle.Fill;
            flowFeatures.Location = new Point(0, 66);
            flowFeatures.Name = "flowFeatures";
            flowFeatures.Padding = new Padding(20);
            flowFeatures.Size = new Size(880, 494);
            flowFeatures.TabIndex = 0;
            // 
            // economyGroup
            // 
            economyGroup.BackColor = Color.FromArgb(255, 239, 216);
            economyGroup.Controls.Add(economyActions);
            economyGroup.ForeColor = Color.FromArgb(58, 36, 24);
            economyGroup.Location = new Point(28, 28);
            economyGroup.Margin = new Padding(8);
            economyGroup.Name = "economyGroup";
            economyGroup.Size = new Size(360, 160);
            economyGroup.TabIndex = 0;
            economyGroup.TabStop = false;
            economyGroup.Text = "Economy & Currency";
            // 
            // economyActions
            // 
            economyActions.Controls.Add(btnMoney);
            economyActions.Controls.Add(btnCoins);
            economyActions.Dock = DockStyle.Fill;
            economyActions.Location = new Point(3, 21);
            economyActions.Name = "economyActions";
            economyActions.Padding = new Padding(8);
            economyActions.Size = new Size(354, 136);
            economyActions.TabIndex = 0;
            // 
            // btnMoney
            // 
            btnMoney.Location = new Point(11, 11);
            btnMoney.Name = "btnMoney";
            btnMoney.Size = new Size(140, 42);
            btnMoney.TabIndex = 0;
            btnMoney.Text = "💰 Money";
            // 
            // btnCoins
            // 
            btnCoins.Location = new Point(157, 11);
            btnCoins.Name = "btnCoins";
            btnCoins.Size = new Size(140, 42);
            btnCoins.TabIndex = 1;
            btnCoins.Text = "\U0001fa99 Coins";
            // 
            // accountGroup
            // 
            accountGroup.BackColor = Color.FromArgb(255, 239, 216);
            accountGroup.Controls.Add(accountActions);
            accountGroup.ForeColor = Color.FromArgb(58, 36, 24);
            accountGroup.Location = new Point(404, 28);
            accountGroup.Margin = new Padding(8);
            accountGroup.Name = "accountGroup";
            accountGroup.Size = new Size(360, 190);
            accountGroup.TabIndex = 1;
            accountGroup.TabStop = false;
            accountGroup.Text = "Account Management";
            // 
            // accountActions
            // 
            accountActions.Controls.Add(btnKingRank);
            accountActions.Controls.Add(btnChangeName);
            accountActions.Controls.Add(btnChangeId);
            accountActions.Controls.Add(btnFixAccount);
            accountActions.Dock = DockStyle.Fill;
            accountActions.Location = new Point(3, 21);
            accountActions.Name = "accountActions";
            accountActions.Padding = new Padding(8);
            accountActions.Size = new Size(354, 166);
            accountActions.TabIndex = 0;
            // 
            // btnKingRank
            // 
            btnKingRank.Location = new Point(11, 11);
            btnKingRank.Name = "btnKingRank";
            btnKingRank.Size = new Size(140, 42);
            btnKingRank.TabIndex = 0;
            btnKingRank.Text = "👑 King Rank";
            // 
            // btnChangeName
            // 
            btnChangeName.Location = new Point(157, 11);
            btnChangeName.Name = "btnChangeName";
            btnChangeName.Size = new Size(140, 42);
            btnChangeName.TabIndex = 1;
            btnChangeName.Text = "✏️ Change Name";
            // 
            // btnChangeId
            // 
            btnChangeId.Location = new Point(11, 59);
            btnChangeId.Name = "btnChangeId";
            btnChangeId.Size = new Size(140, 42);
            btnChangeId.TabIndex = 2;
            btnChangeId.Text = "🆔 Change ID";
            // 
            // btnFixAccount
            // 
            btnFixAccount.Location = new Point(157, 59);
            btnFixAccount.Name = "btnFixAccount";
            btnFixAccount.Size = new Size(140, 42);
            btnFixAccount.TabIndex = 3;
            btnFixAccount.Text = "🔧 Fix Account";
            // 
            // upgradesGroup
            // 
            upgradesGroup.BackColor = Color.FromArgb(255, 239, 216);
            upgradesGroup.Controls.Add(upgradesActions);
            upgradesGroup.ForeColor = Color.FromArgb(58, 36, 24);
            upgradesGroup.Location = new Point(28, 234);
            upgradesGroup.Margin = new Padding(8);
            upgradesGroup.Name = "upgradesGroup";
            upgradesGroup.Size = new Size(440, 220);
            upgradesGroup.TabIndex = 2;
            upgradesGroup.TabStop = false;
            upgradesGroup.Text = "Unlocks & Upgrades";
            // 
            // upgradesActions
            // 
            upgradesActions.Controls.Add(btnUnlockAll);
            upgradesActions.Controls.Add(btnUnlockOne);
            upgradesActions.Controls.Add(btnW16Engine);
            upgradesActions.Controls.Add(btnUnlimitedFuel);
            upgradesActions.Controls.Add(btnArmor);
            upgradesActions.Controls.Add(btnAllWheels);
            upgradesActions.Dock = DockStyle.Fill;
            upgradesActions.Location = new Point(3, 21);
            upgradesActions.Name = "upgradesActions";
            upgradesActions.Padding = new Padding(8);
            upgradesActions.Size = new Size(434, 196);
            upgradesActions.TabIndex = 0;
            // 
            // btnUnlockAll
            // 
            btnUnlockAll.Location = new Point(11, 11);
            btnUnlockAll.Name = "btnUnlockAll";
            btnUnlockAll.Size = new Size(140, 42);
            btnUnlockAll.TabIndex = 0;
            btnUnlockAll.Text = "🚗 Unlock All";
            // 
            // btnUnlockOne
            // 
            btnUnlockOne.Location = new Point(157, 11);
            btnUnlockOne.Name = "btnUnlockOne";
            btnUnlockOne.Size = new Size(140, 42);
            btnUnlockOne.TabIndex = 1;
            btnUnlockOne.Text = "🚙 Unlock One";
            // 
            // btnW16Engine
            // 
            btnW16Engine.Location = new Point(11, 59);
            btnW16Engine.Name = "btnW16Engine";
            btnW16Engine.Size = new Size(140, 42);
            btnW16Engine.TabIndex = 2;
            btnW16Engine.Text = "⚡ W16 Engine";
            // 
            // btnUnlimitedFuel
            // 
            btnUnlimitedFuel.Location = new Point(157, 59);
            btnUnlimitedFuel.Name = "btnUnlimitedFuel";
            btnUnlimitedFuel.Size = new Size(140, 42);
            btnUnlimitedFuel.TabIndex = 3;
            btnUnlimitedFuel.Text = "⛽ Unlimited Fuel";
            // 
            // btnArmor
            // 
            btnArmor.Location = new Point(11, 107);
            btnArmor.Name = "btnArmor";
            btnArmor.Size = new Size(140, 42);
            btnArmor.TabIndex = 4;
            btnArmor.Text = "🛡️ Armor";
            // 
            // btnAllWheels
            // 
            btnAllWheels.Location = new Point(157, 107);
            btnAllWheels.Name = "btnAllWheels";
            btnAllWheels.Size = new Size(140, 42);
            btnAllWheels.TabIndex = 5;
            btnAllWheels.Text = "\U0001f6de All Wheels";
            // 
            // cosmeticsGroup
            // 
            cosmeticsGroup.BackColor = Color.FromArgb(255, 239, 216);
            cosmeticsGroup.Controls.Add(cosmeticsActions);
            cosmeticsGroup.ForeColor = Color.FromArgb(58, 36, 24);
            cosmeticsGroup.Location = new Point(484, 234);
            cosmeticsGroup.Margin = new Padding(8);
            cosmeticsGroup.Name = "cosmeticsGroup";
            cosmeticsGroup.Size = new Size(360, 190);
            cosmeticsGroup.TabIndex = 3;
            cosmeticsGroup.TabStop = false;
            cosmeticsGroup.Text = "Cosmetics";
            // 
            // cosmeticsActions
            // 
            cosmeticsActions.Controls.Add(btnAllAnimations);
            cosmeticsActions.Controls.Add(btnMaleClothes);
            cosmeticsActions.Controls.Add(btnFemaleClothes);
            cosmeticsActions.Dock = DockStyle.Fill;
            cosmeticsActions.Location = new Point(3, 21);
            cosmeticsActions.Name = "cosmeticsActions";
            cosmeticsActions.Padding = new Padding(8);
            cosmeticsActions.Size = new Size(354, 166);
            cosmeticsActions.TabIndex = 0;
            // 
            // btnAllAnimations
            // 
            btnAllAnimations.Location = new Point(11, 11);
            btnAllAnimations.Name = "btnAllAnimations";
            btnAllAnimations.Size = new Size(140, 42);
            btnAllAnimations.TabIndex = 0;
            btnAllAnimations.Text = "💃 All Animations";
            // 
            // btnMaleClothes
            // 
            btnMaleClothes.Location = new Point(157, 11);
            btnMaleClothes.Name = "btnMaleClothes";
            btnMaleClothes.Size = new Size(140, 42);
            btnMaleClothes.TabIndex = 1;
            btnMaleClothes.Text = "👕 Male Clothes";
            // 
            // btnFemaleClothes
            // 
            btnFemaleClothes.Location = new Point(11, 59);
            btnFemaleClothes.Name = "btnFemaleClothes";
            btnFemaleClothes.Size = new Size(140, 42);
            btnFemaleClothes.TabIndex = 2;
            btnFemaleClothes.Text = "👗 Female Clothes";
            // 
            // bottomPanel
            // 
            bottomPanel.BackColor = Color.FromArgb(58, 36, 24);
            bottomPanel.Controls.Add(rtbLog);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 560);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Padding = new Padding(8);
            bottomPanel.Size = new Size(1180, 200);
            bottomPanel.TabIndex = 1;
            // 
            // rtbLog
            // 
            rtbLog.BackColor = Color.FromArgb(45, 28, 20);
            rtbLog.BorderStyle = BorderStyle.None;
            rtbLog.DetectUrls = false;
            rtbLog.Dock = DockStyle.Fill;
            rtbLog.Font = new Font("Consolas", 10F);
            rtbLog.ForeColor = Color.FromArgb(255, 239, 216);
            rtbLog.Location = new Point(8, 8);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Size = new Size(1164, 184);
            rtbLog.TabIndex = 0;
            rtbLog.Text = "";
            // 
            // Form1
            // 
            BackColor = Color.FromArgb(255, 247, 235);
            ClientSize = new Size(1180, 760);
            Controls.Add(flowFeatures);
            Controls.Add(profilePanel);
            Controls.Add(topPanel);
            Controls.Add(bottomPanel);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(900, 600);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CPM1 Tools | Account Dashboard";
            topPanel.ResumeLayout(false);
            topControls.ResumeLayout(false);
            topControls.PerformLayout();
            profilePanel.ResumeLayout(false);
            profilePanel.PerformLayout();
            flowFeatures.ResumeLayout(false);
            economyGroup.ResumeLayout(false);
            economyActions.ResumeLayout(false);
            accountGroup.ResumeLayout(false);
            accountActions.ResumeLayout(false);
            upgradesGroup.ResumeLayout(false);
            upgradesActions.ResumeLayout(false);
            cosmeticsGroup.ResumeLayout(false);
            cosmeticsActions.ResumeLayout(false);
            bottomPanel.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}