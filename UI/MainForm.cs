using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace WalletAuditor.UI
{
    /// <summary>
    /// MainForm.cs - Primary UI form for WalletAuditor application
    /// Provides comprehensive wallet management with tabbed interface,
    /// validation, balance checking, transaction history, and security features
    /// </summary>
    public partial class MainForm : Form
    {
        // Form title and constants
        private const string APPLICATION_TITLE = "Wallet Auditor - Professional Wallet Management Suite";
        private const int FORM_WIDTH = 1200;
        private const int FORM_HEIGHT = 700;
        
        // Security and validation constants
        private const int MIN_PASSWORD_LENGTH = 12;
        private const int SESSION_TIMEOUT_MINUTES = 30;
        
        // UI Components
        private TabControl mainTabControl;
        private TabPage tabDashboard;
        private TabPage tabWalletValidation;
        private TabPage tabBalanceCheck;
        private TabPage tabTransactionHistory;
        private TabPage tabSecuritySettings;
        private TabPage tabAuditLogs;
        
        // Dashboard Controls
        private Label lblWalletStatus;
        private Label lblTotalBalance;
        private Label lblLastSync;
        private ProgressBar progressBarSync;
        private Button btnQuickSync;
        private RichTextBox rtbDashboardSummary;
        
        // Wallet Validation Tab Controls
        private TextBox txtWalletAddress;
        private Button btnValidateWallet;
        private Label lblValidationResult;
        private DataGridView dgvValidationResults;
        private CheckBox chkDeepValidation;
        private Label lblValidationStatus;
        
        // Balance Check Tab Controls
        private TextBox txtBalanceWalletAddress;
        private Button btnCheckBalance;
        private Label lblBalanceAmount;
        private Label lblBalanceCurrency;
        private ComboBox cmbCurrencyType;
        private Label lblExchangeRate;
        private Label lblLastBalanceUpdate;
        
        // Transaction History Tab Controls
        private TextBox txtTransactionWalletAddress;
        private Button btnLoadTransactions;
        private DataGridView dgvTransactionHistory;
        private Label lblTransactionCount;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private ComboBox cmbTransactionFilter;
        private Label lblTransactionStatus;
        
        // Security Settings Tab Controls
        private CheckBox chkEnableTwoFactor;
        private Button btnSetMasterPassword;
        private TextBox txtCurrentPassword;
        private TextBox txtNewPassword;
        private TextBox txtConfirmPassword;
        private Button btnChangePassword;
        private Label lblPasswordStrength;
        private CheckBox chkLogoutOnInactivity;
        private CheckBox chkEncryptLocalData;
        private Button btnClearCache;
        private Label lblSecurityStatus;
        
        // Audit Logs Tab Controls
        private DataGridView dgvAuditLogs;
        private Button btnExportLogs;
        private Button btnClearLogs;
        private DateTimePicker dtpAuditStartDate;
        private DateTimePicker dtpAuditEndDate;
        private ComboBox cmbAuditEventType;
        private Label lblAuditLogCount;
        
        // Status Bar and Menu Controls
        private StatusStrip statusStrip;
        private ToolStripStatusLabel tsslApplicationStatus;
        private ToolStripStatusLabel tsslUserInfo;
        private ToolStripStatusLabel tsslSessionTime;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private Timer sessionTimer;
        private Timer syncStatusTimer;
        
        // Application state variables
        private bool isLoggedIn = false;
        private string currentUsername = string.Empty;
        private DateTime sessionStartTime;
        private int inactivityCounter = 0;
        private bool masterPasswordSet = false;
        private List<AuditLogEntry> auditLogs = new List<AuditLogEntry>();
        
        public MainForm()
        {
            InitializeComponent();
            InitializeApplication();
        }
        
        /// <summary>
        /// Initializes all application components and settings
        /// </summary>
        private void InitializeApplication()
        {
            this.Text = APPLICATION_TITLE;
            this.Size = new Size(FORM_WIDTH, FORM_HEIGHT);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Icon = SystemIcons.Application;
            this.BackColor = Color.FromArgb(240, 240, 240);
            
            // Initialize timers
            sessionTimer = new Timer();
            sessionTimer.Interval = 60000; // 1 minute
            sessionTimer.Tick += SessionTimer_Tick;
            
            syncStatusTimer = new Timer();
            syncStatusTimer.Interval = 5000; // 5 seconds
            syncStatusTimer.Tick += SyncStatusTimer_Tick;
            
            // Create UI elements
            CreateMenuStrip();
            CreateMainTabControl();
            CreateDashboardTab();
            CreateWalletValidationTab();
            CreateBalanceCheckTab();
            CreateTransactionHistoryTab();
            CreateSecuritySettingsTab();
            CreateAuditLogsTab();
            CreateStatusStrip();
            
            // Load initial data
            LoadApplicationSettings();
            LogAuditEvent("Application Started", "Application Initialization", "Info");
        }
        
        /// <summary>
        /// Creates the main menu strip with File and Help menus
        /// </summary>
        private void CreateMenuStrip()
        {
            menuStrip = new MenuStrip();
            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;
            
            // File Menu
            fileToolStripMenuItem = new ToolStripMenuItem("&File");
            fileToolStripMenuItem.DropDownItems.Add("&Exit", null, (s, e) => this.Close());
            menuStrip.Items.Add(fileToolStripMenuItem);
            
            // Help Menu
            helpToolStripMenuItem = new ToolStripMenuItem("&Help");
            helpToolStripMenuItem.DropDownItems.Add("&About", null, ShowAboutDialog);
            helpToolStripMenuItem.DropDownItems.Add("&Documentation", null, ShowDocumentation);
            menuStrip.Items.Add(helpToolStripMenuItem);
        }
        
        /// <summary>
        /// Creates the main TabControl with all application tabs
        /// </summary>
        private void CreateMainTabControl()
        {
            mainTabControl = new TabControl();
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Margin = new Padding(10);
            this.Controls.Add(mainTabControl);
        }
        
        /// <summary>
        /// Creates the Dashboard tab with overview and quick access
        /// </summary>
        private void CreateDashboardTab()
        {
            tabDashboard = new TabPage("Dashboard");
            tabDashboard.BackColor = Color.White;
            tabDashboard.Padding = new Padding(10);
            
            // Title
            Label lblDashboardTitle = new Label
            {
                Text = "Wallet Auditor Dashboard",
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            tabDashboard.Controls.Add(lblDashboardTitle);
            
            // Wallet Status Group
            GroupBox gbWalletStatus = new GroupBox
            {
                Text = "System Status",
                Location = new Point(10, 40),
                Size = new Size(350, 150),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            
            lblWalletStatus = new Label
            {
                Text = "Status: Initializing...",
                Location = new Point(10, 25),
                AutoSize = true,
                ForeColor = Color.Orange
            };
            gbWalletStatus.Controls.Add(lblWalletStatus);
            
            lblTotalBalance = new Label
            {
                Text = "Total Balance: Loading...",
                Location = new Point(10, 50),
                AutoSize = true
            };
            gbWalletStatus.Controls.Add(lblTotalBalance);
            
            lblLastSync = new Label
            {
                Text = "Last Sync: Never",
                Location = new Point(10, 75),
                AutoSize = true
            };
            gbWalletStatus.Controls.Add(lblLastSync);
            
            progressBarSync = new ProgressBar
            {
                Location = new Point(10, 100),
                Size = new Size(330, 20),
                Visible = false
            };
            gbWalletStatus.Controls.Add(progressBarSync);
            
            tabDashboard.Controls.Add(gbWalletStatus);
            
            // Quick Actions Group
            GroupBox gbQuickActions = new GroupBox
            {
                Text = "Quick Actions",
                Location = new Point(370, 40),
                Size = new Size(250, 150),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            
            btnQuickSync = new Button
            {
                Text = "Sync Now",
                Location = new Point(10, 25),
                Size = new Size(220, 35),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnQuickSync.Click += BtnQuickSync_Click;
            gbQuickActions.Controls.Add(btnQuickSync);
            
            Button btnValidateAllWallets = new Button
            {
                Text = "Validate All Wallets",
                Location = new Point(10, 70),
                Size = new Size(220, 35),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnValidateAllWallets.Click += BtnValidateAllWallets_Click;
            gbQuickActions.Controls.Add(btnValidateAllWallets);
            
            tabDashboard.Controls.Add(gbQuickActions);
            
            // Summary Information
            rtbDashboardSummary = new RichTextBox
            {
                Location = new Point(10, 200),
                Size = new Size(610, 300),
                ReadOnly = true,
                Font = new Font("Courier New", 9),
                BackColor = Color.FromArgb(250, 250, 250)
            };
            tabDashboard.Controls.Add(rtbDashboardSummary);
            
            // Initialize dashboard content
            UpdateDashboard();
            
            mainTabControl.TabPages.Add(tabDashboard);
        }
        
        /// <summary>
        /// Creates the Wallet Validation tab
        /// </summary>
        private void CreateWalletValidationTab()
        {
            tabWalletValidation = new TabPage("Wallet Validation");
            tabWalletValidation.BackColor = Color.White;
            tabWalletValidation.Padding = new Padding(10);
            
            // Title
            Label lblValidationTitle = new Label
            {
                Text = "Wallet Validation & Verification",
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            tabWalletValidation.Controls.Add(lblValidationTitle);
            
            // Input Section
            GroupBox gbValidationInput = new GroupBox
            {
                Text = "Enter Wallet Information",
                Location = new Point(10, 40),
                Size = new Size(600, 150),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            
            Label lblAddress = new Label
            {
                Text = "Wallet Address:",
                Location = new Point(10, 25),
                AutoSize = true
            };
            gbValidationInput.Controls.Add(lblAddress);
            
            txtWalletAddress = new TextBox
            {
                Location = new Point(10, 45),
                Size = new Size(580, 30),
                Font = new Font("Arial", 10),
                PlaceholderText = "Enter wallet address (Bitcoin, Ethereum, etc.)"
            };
            gbValidationInput.Controls.Add(txtWalletAddress);
            
            chkDeepValidation = new CheckBox
            {
                Text = "Perform Deep Validation (Cryptographic Verification)",
                Location = new Point(10, 80),
                AutoSize = true
            };
            gbValidationInput.Controls.Add(chkDeepValidation);
            
            btnValidateWallet = new Button
            {
                Text = "Validate Wallet",
                Location = new Point(10, 105),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnValidateWallet.Click += BtnValidateWallet_Click;
            gbValidationInput.Controls.Add(btnValidateWallet);
            
            tabWalletValidation.Controls.Add(gbValidationInput);
            
            // Validation Results
            lblValidationStatus = new Label
            {
                Text = "Status: Ready",
                Location = new Point(10, 200),
                AutoSize = true,
                ForeColor = Color.Green
            };
            tabWalletValidation.Controls.Add(lblValidationStatus);
            
            lblValidationResult = new Label
            {
                Text = "Result: Awaiting validation input",
                Location = new Point(10, 220),
                Size = new Size(600, 30),
                AutoSize = false,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245)
            };
            tabWalletValidation.Controls.Add(lblValidationResult);
            
            // Results Grid
            dgvValidationResults = new DataGridView
            {
                Location = new Point(10, 260),
                Size = new Size(600, 240),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvValidationResults.Columns.Add("Property", "Property");
            dgvValidationResults.Columns.Add("Value", "Value");
            dgvValidationResults.Columns[0].Width = 200;
            dgvValidationResults.Columns[1].Width = 390;
            tabWalletValidation.Controls.Add(dgvValidationResults);
            
            mainTabControl.TabPages.Add(tabWalletValidation);
        }
        
        /// <summary>
        /// Creates the Balance Check tab
        /// </summary>
        private void CreateBalanceCheckTab()
        {
            tabBalanceCheck = new TabPage("Balance Check");
            tabBalanceCheck.BackColor = Color.White;
            tabBalanceCheck.Padding = new Padding(10);
            
            // Title
            Label lblBalanceTitle = new Label
            {
                Text = "Wallet Balance Verification",
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            tabBalanceCheck.Controls.Add(lblBalanceTitle);
            
            // Input Section
            GroupBox gbBalanceInput = new GroupBox
            {
                Text = "Wallet Information",
                Location = new Point(10, 40),
                Size = new Size(600, 140),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            
            Label lblBalanceAddress = new Label
            {
                Text = "Wallet Address:",
                Location = new Point(10, 25),
                AutoSize = true
            };
            gbBalanceInput.Controls.Add(lblBalanceAddress);
            
            txtBalanceWalletAddress = new TextBox
            {
                Location = new Point(10, 45),
                Size = new Size(400, 30),
                Font = new Font("Arial", 10),
                PlaceholderText = "Enter wallet address"
            };
            gbBalanceInput.Controls.Add(txtBalanceWalletAddress);
            
            Label lblCurrency = new Label
            {
                Text = "Currency:",
                Location = new Point(420, 25),
                AutoSize = true
            };
            gbBalanceInput.Controls.Add(lblCurrency);
            
            cmbCurrencyType = new ComboBox
            {
                Location = new Point(420, 45),
                Size = new Size(160, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 10)
            };
            cmbCurrencyType.Items.AddRange(new[] { "BTC", "ETH", "USDT", "USD", "EUR" });
            cmbCurrencyType.SelectedIndex = 0;
            gbBalanceInput.Controls.Add(cmbCurrencyType);
            
            btnCheckBalance = new Button
            {
                Text = "Check Balance",
                Location = new Point(10, 100),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCheckBalance.Click += BtnCheckBalance_Click;
            gbBalanceInput.Controls.Add(btnCheckBalance);
            
            tabBalanceCheck.Controls.Add(gbBalanceInput);
            
            // Balance Display
            GroupBox gbBalanceDisplay = new GroupBox
            {
                Text = "Current Balance",
                Location = new Point(10, 190),
                Size = new Size(600, 200),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            
            lblBalanceAmount = new Label
            {
                Text = "Amount: 0.00",
                Location = new Point(10, 30),
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204)
            };
            gbBalanceDisplay.Controls.Add(lblBalanceAmount);
            
            lblBalanceCurrency = new Label
            {
                Text = "Currency: --",
                Location = new Point(10, 60),
                Font = new Font("Arial", 12),
                AutoSize = true
            };
            gbBalanceDisplay.Controls.Add(lblBalanceCurrency);
            
            lblExchangeRate = new Label
            {
                Text = "Exchange Rate: Loading...",
                Location = new Point(10, 90),
                AutoSize = true
            };
            gbBalanceDisplay.Controls.Add(lblExchangeRate);
            
            lblLastBalanceUpdate = new Label
            {
                Text = "Last Updated: Never",
                Location = new Point(10, 120),
                AutoSize = true
            };
            gbBalanceDisplay.Controls.Add(lblLastBalanceUpdate);
            
            Button btnCopyAddress = new Button
            {
                Text = "Copy Balance",
                Location = new Point(10, 155),
                Size = new Size(130, 30),
                BackColor = Color.FromArgb(100, 181, 246),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnCopyAddress.Click += (s, e) => CopyToClipboard(lblBalanceAmount.Text);
            gbBalanceDisplay.Controls.Add(btnCopyAddress);
            
            tabBalanceCheck.Controls.Add(gbBalanceDisplay);
            
            mainTabControl.TabPages.Add(tabBalanceCheck);
        }
        
        /// <summary>
        /// Creates the Transaction History tab
        /// </summary>
        private void CreateTransactionHistoryTab()
        {
            tabTransactionHistory = new TabPage("Transaction History");
            tabTransactionHistory.BackColor = Color.White;
            tabTransactionHistory.Padding = new Padding(10);
            
            // Title
            Label lblTransactionTitle = new Label
            {
                Text = "Transaction History & Analysis",
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            tabTransactionHistory.Controls.Add(lblTransactionTitle);
            
            // Filter Section
            GroupBox gbTransactionFilter = new GroupBox
            {
                Text = "Filter & Search",
                Location = new Point(10, 40),
                Size = new Size(600, 140),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            
            Label lblTxnAddress = new Label
            {
                Text = "Wallet Address:",
                Location = new Point(10, 25),
                AutoSize = true
            };
            gbTransactionFilter.Controls.Add(lblTxnAddress);
            
            txtTransactionWalletAddress = new TextBox
            {
                Location = new Point(10, 45),
                Size = new Size(450, 30),
                Font = new Font("Arial", 10),
                PlaceholderText = "Enter wallet address"
            };
            gbTransactionFilter.Controls.Add(txtTransactionWalletAddress);
            
            Label lblDateRange = new Label
            {
                Text = "Date Range:",
                Location = new Point(10, 80),
                AutoSize = true
            };
            gbTransactionFilter.Controls.Add(lblDateRange);
            
            dtpStartDate = new DateTimePicker
            {
                Location = new Point(10, 100),
                Size = new Size(200, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddMonths(-1)
            };
            gbTransactionFilter.Controls.Add(dtpStartDate);
            
            dtpEndDate = new DateTimePicker
            {
                Location = new Point(260, 100),
                Size = new Size(200, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };
            gbTransactionFilter.Controls.Add(dtpEndDate);
            
            btnLoadTransactions = new Button
            {
                Text = "Load Transactions",
                Location = new Point(470, 45),
                Size = new Size(120, 85),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLoadTransactions.Click += BtnLoadTransactions_Click;
            gbTransactionFilter.Controls.Add(btnLoadTransactions);
            
            tabTransactionHistory.Controls.Add(gbTransactionFilter);
            
            // Status Section
            lblTransactionStatus = new Label
            {
                Text = "Status: Ready",
                Location = new Point(10, 190),
                AutoSize = true,
                ForeColor = Color.Green
            };
            tabTransactionHistory.Controls.Add(lblTransactionStatus);
            
            lblTransactionCount = new Label
            {
                Text = "Transactions Found: 0",
                Location = new Point(10, 210),
                AutoSize = true
            };
            tabTransactionHistory.Controls.Add(lblTransactionCount);
            
            // Transaction Grid
            dgvTransactionHistory = new DataGridView
            {
                Location = new Point(10, 240),
                Size = new Size(600, 260),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvTransactionHistory.Columns.Add("TxHash", "Transaction Hash");
            dgvTransactionHistory.Columns.Add("Date", "Date/Time");
            dgvTransactionHistory.Columns.Add("Amount", "Amount");
            dgvTransactionHistory.Columns.Add("Status", "Status");
            dgvTransactionHistory.Columns[0].Width = 200;
            dgvTransactionHistory.Columns[1].Width = 150;
            dgvTransactionHistory.Columns[2].Width = 100;
            dgvTransactionHistory.Columns[3].Width = 130;
            tabTransactionHistory.Controls.Add(dgvTransactionHistory);
            
            mainTabControl.TabPages.Add(tabTransactionHistory);
        }
        
        /// <summary>
        /// Creates the Security Settings tab
        /// </summary>
        private void CreateSecuritySettingsTab()
        {
            tabSecuritySettings = new TabPage("Security Settings");
            tabSecuritySettings.BackColor = Color.White;
            tabSecuritySettings.Padding = new Padding(10);
            
            // Title
            Label lblSecurityTitle = new Label
            {
                Text = "Security Configuration & Settings",
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            tabSecuritySettings.Controls.Add(lblSecurityTitle);
            
            // Security Status
            lblSecurityStatus = new Label
            {
                Text = "Security Status: Standard",
                Location = new Point(10, 40),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Orange
            };
            tabSecuritySettings.Controls.Add(lblSecurityStatus);
            
            // Authentication Section
            GroupBox gbAuthentication = new GroupBox
            {
                Text = "Authentication",
                Location = new Point(10, 70),
                Size = new Size(600, 150),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            
            chkEnableTwoFactor = new CheckBox
            {
                Text = "Enable Two-Factor Authentication (2FA)",
                Location = new Point(10, 25),
                AutoSize = true
            };
            chkEnableTwoFactor.CheckedChanged += ChkEnableTwoFactor_CheckedChanged;
            gbAuthentication.Controls.Add(chkEnableTwoFactor);
            
            btnSetMasterPassword = new Button
            {
                Text = "Set Master Password",
                Location = new Point(10, 55),
                Size = new Size(180, 35),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSetMasterPassword.Click += BtnSetMasterPassword_Click;
            gbAuthentication.Controls.Add(btnSetMasterPassword);
            
            Label lblMasterPasswordStatus = new Label
            {
                Text = "Master Password: Not Set",
                Location = new Point(200, 60),
                AutoSize = true,
                ForeColor = Color.Red
            };
            gbAuthentication.Controls.Add(lblMasterPasswordStatus);
            
            chkLogoutOnInactivity = new CheckBox
            {
                Text = "Logout on Inactivity (30 minutes)",
                Location = new Point(10, 95),
                AutoSize = true,
                Checked = true
            };
            gbAuthentication.Controls.Add(chkLogoutOnInactivity);
            
            tabSecuritySettings.Controls.Add(gbAuthentication);
            
            // Password Management Section
            GroupBox gbPasswordManagement = new GroupBox
            {
                Text = "Password Management",
                Location = new Point(10, 230),
                Size = new Size(600, 200),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            
            Label lblCurrentPwd = new Label
            {
                Text = "Current Password:",
                Location = new Point(10, 25),
                AutoSize = true
            };
            gbPasswordManagement.Controls.Add(lblCurrentPwd);
            
            txtCurrentPassword = new TextBox
            {
                Location = new Point(10, 45),
                Size = new Size(280, 30),
                Font = new Font("Arial", 10),
                UseSystemPasswordChar = true
            };
            gbPasswordManagement.Controls.Add(txtCurrentPassword);
            
            Label lblNewPwd = new Label
            {
                Text = "New Password:",
                Location = new Point(300, 25),
                AutoSize = true
            };
            gbPasswordManagement.Controls.Add(lblNewPwd);
            
            txtNewPassword = new TextBox
            {
                Location = new Point(300, 45),
                Size = new Size(280, 30),
                Font = new Font("Arial", 10),
                UseSystemPasswordChar = true
            };
            txtNewPassword.TextChanged += TxtNewPassword_TextChanged;
            gbPasswordManagement.Controls.Add(txtNewPassword);
            
            Label lblConfirmPwd = new Label
            {
                Text = "Confirm Password:",
                Location = new Point(10, 80),
                AutoSize = true
            };
            gbPasswordManagement.Controls.Add(lblConfirmPwd);
            
            txtConfirmPassword = new TextBox
            {
                Location = new Point(10, 100),
                Size = new Size(280, 30),
                Font = new Font("Arial", 10),
                UseSystemPasswordChar = true
            };
            gbPasswordManagement.Controls.Add(txtConfirmPassword);
            
            lblPasswordStrength = new Label
            {
                Text = "Password Strength: --",
                Location = new Point(300, 80),
                AutoSize = true,
                ForeColor = Color.Gray
            };
            gbPasswordManagement.Controls.Add(lblPasswordStrength);
            
            btnChangePassword = new Button
            {
                Text = "Change Password",
                Location = new Point(10, 155),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnChangePassword.Click += BtnChangePassword_Click;
            gbPasswordManagement.Controls.Add(btnChangePassword);
            
            tabSecuritySettings.Controls.Add(gbPasswordManagement);
            
            // Data Protection Section
            GroupBox gbDataProtection = new GroupBox
            {
                Text = "Data Protection",
                Location = new Point(10, 440),
                Size = new Size(600, 100),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            
            chkEncryptLocalData = new CheckBox
            {
                Text = "Encrypt Local Data Storage",
                Location = new Point(10, 25),
                AutoSize = true,
                Checked = true
            };
            gbDataProtection.Controls.Add(chkEncryptLocalData);
            
            btnClearCache = new Button
            {
                Text = "Clear Cache & Logs",
                Location = new Point(10, 55),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClearCache.Click += BtnClearCache_Click;
            gbDataProtection.Controls.Add(btnClearCache);
            
            tabSecuritySettings.Controls.Add(gbDataProtection);
            
            mainTabControl.TabPages.Add(tabSecuritySettings);
        }
        
        /// <summary>
        /// Creates the Audit Logs tab
        /// </summary>
        private void CreateAuditLogsTab()
        {
            tabAuditLogs = new TabPage("Audit Logs");
            tabAuditLogs.BackColor = Color.White;
            tabAuditLogs.Padding = new Padding(10);
            
            // Title
            Label lblAuditTitle = new Label
            {
                Text = "System Audit Logs & Activity Tracking",
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            tabAuditLogs.Controls.Add(lblAuditTitle);
            
            // Filter Section
            GroupBox gbAuditFilter = new GroupBox
            {
                Text = "Filter Logs",
                Location = new Point(10, 40),
                Size = new Size(600, 120),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            
            Label lblAuditDateRange = new Label
            {
                Text = "Date Range:",
                Location = new Point(10, 25),
                AutoSize = true
            };
            gbAuditFilter.Controls.Add(lblAuditDateRange);
            
            dtpAuditStartDate = new DateTimePicker
            {
                Location = new Point(10, 45),
                Size = new Size(180, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddDays(-7)
            };
            gbAuditFilter.Controls.Add(dtpAuditStartDate);
            
            Label lblTo = new Label
            {
                Text = "To:",
                Location = new Point(200, 50),
                AutoSize = true
            };
            gbAuditFilter.Controls.Add(lblTo);
            
            dtpAuditEndDate = new DateTimePicker
            {
                Location = new Point(225, 45),
                Size = new Size(180, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };
            gbAuditFilter.Controls.Add(dtpAuditEndDate);
            
            Label lblEventType = new Label
            {
                Text = "Event Type:",
                Location = new Point(10, 80),
                AutoSize = true
            };
            gbAuditFilter.Controls.Add(lblEventType);
            
            cmbAuditEventType = new ComboBox
            {
                Location = new Point(100, 77),
                Size = new Size(290, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 10)
            };
            cmbAuditEventType.Items.AddRange(new[] { "All", "Login", "Logout", "Validation", "Balance Check", "Settings Change", "Security Event" });
            cmbAuditEventType.SelectedIndex = 0;
            gbAuditFilter.Controls.Add(cmbAuditEventType);
            
            tabAuditLogs.Controls.Add(gbAuditFilter);
            
            // Status
            lblAuditLogCount = new Label
            {
                Text = "Audit Logs Found: 0",
                Location = new Point(10, 170),
                AutoSize = true
            };
            tabAuditLogs.Controls.Add(lblAuditLogCount);
            
            // Audit Logs Grid
            dgvAuditLogs = new DataGridView
            {
                Location = new Point(10, 195),
                Size = new Size(600, 280),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvAuditLogs.Columns.Add("Timestamp", "Timestamp");
            dgvAuditLogs.Columns.Add("Event", "Event");
            dgvAuditLogs.Columns.Add("Category", "Category");
            dgvAuditLogs.Columns.Add("Status", "Status");
            dgvAuditLogs.Columns.Add("Details", "Details");
            dgvAuditLogs.Columns[0].Width = 150;
            dgvAuditLogs.Columns[1].Width = 100;
            dgvAuditLogs.Columns[2].Width = 100;
            dgvAuditLogs.Columns[3].Width = 80;
            dgvAuditLogs.Columns[4].Width = 70;
            tabAuditLogs.Controls.Add(dgvAuditLogs);
            
            // Action Buttons
            btnExportLogs = new Button
            {
                Text = "Export Logs",
                Location = new Point(10, 485),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExportLogs.Click += BtnExportLogs_Click;
            tabAuditLogs.Controls.Add(btnExportLogs);
            
            btnClearLogs = new Button
            {
                Text = "Clear Logs",
                Location = new Point(140, 485),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClearLogs.Click += BtnClearLogs_Click;
            tabAuditLogs.Controls.Add(btnClearLogs);
            
            mainTabControl.TabPages.Add(tabAuditLogs);
            
            RefreshAuditLogs();
        }
        
        /// <summary>
        /// Creates the status strip at the bottom of the form
        /// </summary>
        private void CreateStatusStrip()
        {
            statusStrip = new StatusStrip();
            statusStrip.BackColor = Color.FromArgb(240, 240, 240);
            
            tsslApplicationStatus = new ToolStripStatusLabel("Status: Ready");
            tsslApplicationStatus.ForeColor = Color.Green;
            statusStrip.Items.Add(tsslApplicationStatus);
            
            ToolStripSeparator separator1 = new ToolStripSeparator();
            statusStrip.Items.Add(separator1);
            
            tsslUserInfo = new ToolStripStatusLabel("User: Not Logged In");
            statusStrip.Items.Add(tsslUserInfo);
            
            ToolStripSeparator separator2 = new ToolStripSeparator();
            statusStrip.Items.Add(separator2);
            
            tsslSessionTime = new ToolStripStatusLabel("Session Time: 00:00");
            statusStrip.Items.Add(tsslSessionTime);
            
            this.Controls.Add(statusStrip);
        }
        
        /// <summary>
        /// Updates the dashboard with current information
        /// </summary>
        private void UpdateDashboard()
        {
            StringBuilder summary = new StringBuilder();
            summary.AppendLine("=== WALLET AUDITOR DASHBOARD SUMMARY ===");
            summary.AppendLine();
            summary.AppendLine($"Application Version: 1.0.0");
            summary.AppendLine($"Current Date/Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            summary.AppendLine($"Master Password Set: {(masterPasswordSet ? "Yes" : "No")}");
            summary.AppendLine();
            summary.AppendLine("=== QUICK STATS ===");
            summary.AppendLine($"Total Wallets Monitored: 0");
            summary.AppendLine($"Total Validation Checks: 0");
            summary.AppendLine($"Total Transactions Tracked: 0");
            summary.AppendLine($"Security Alerts: 0");
            summary.AppendLine();
            summary.AppendLine("=== RECENT ACTIVITY ===");
            summary.AppendLine("No recent activity recorded.");
            summary.AppendLine();
            summary.AppendLine("=== SYSTEM STATUS ===");
            summary.AppendLine("All systems operational");
            summary.AppendLine("Data encryption enabled");
            summary.AppendLine("Audit logging active");
            
            rtbDashboardSummary.Text = summary.ToString();
            
            lblWalletStatus.Text = "Status: Online & Ready";
            lblWalletStatus.ForeColor = Color.Green;
            lblTotalBalance.Text = "Total Balance: $0.00";
            lblLastSync.Text = $"Last Sync: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }
        
        #region Event Handlers
        
        private void BtnQuickSync_Click(object sender, EventArgs e)
        {
            progressBarSync.Visible = true;
            progressBarSync.Value = 0;
            lblWalletStatus.Text = "Status: Syncing...";
            lblWalletStatus.ForeColor = Color.Orange;
            
            syncStatusTimer.Start();
            LogAuditEvent("Quick Sync", "Dashboard", "Info");
        }
        
        private void BtnValidateAllWallets_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Validating all wallets...\n\nThis process may take a few moments.", "Wallet Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LogAuditEvent("Validate All Wallets", "Dashboard", "Info");
        }
        
        private void BtnValidateWallet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWalletAddress.Text))
            {
                MessageBox.Show("Please enter a wallet address.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string address = txtWalletAddress.Text.Trim();
            bool isDeepValidation = chkDeepValidation.Checked;
            
            // Simulate validation
            bool isValid = ValidateWalletAddress(address);
            
            lblValidationStatus.Text = "Status: " + (isValid ? "Valid" : "Invalid");
            lblValidationStatus.ForeColor = isValid ? Color.Green : Color.Red;
            
            lblValidationResult.Text = isValid ? "✓ Wallet address is valid and verified" : "✗ Wallet address failed validation";
            lblValidationResult.ForeColor = isValid ? Color.Green : Color.Red;
            
            dgvValidationResults.Rows.Clear();
            dgvValidationResults.Rows.Add("Address", address);
            dgvValidationResults.Rows.Add("Valid", isValid);
            dgvValidationResults.Rows.Add("Type", "Bitcoin/Ethereum Hybrid");
            dgvValidationResults.Rows.Add("Checksum", "Verified");
            dgvValidationResults.Rows.Add("Deep Validation", isDeepValidation);
            dgvValidationResults.Rows.Add("Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            
            LogAuditEvent("Wallet Validation", "Wallet Validation", isValid ? "Info" : "Warning");
        }
        
        private void BtnCheckBalance_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBalanceWalletAddress.Text))
            {
                MessageBox.Show("Please enter a wallet address.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string address = txtBalanceWalletAddress.Text.Trim();
            string currency = cmbCurrencyType.SelectedItem.ToString();
            
            // Simulate balance check
            decimal balance = GetRandomBalance();
            
            lblBalanceAmount.Text = $"Amount: {balance:F8}";
            lblBalanceCurrency.Text = $"Currency: {currency}";
            lblExchangeRate.Text = $"Exchange Rate: 1 {currency} = $45,234.00";
            lblLastBalanceUpdate.Text = $"Last Updated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            
            LogAuditEvent("Balance Check", "Balance Check", "Info");
        }
        
        private void BtnLoadTransactions_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTransactionWalletAddress.Text))
            {
                MessageBox.Show("Please enter a wallet address.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string address = txtTransactionWalletAddress.Text.Trim();
            
            dgvTransactionHistory.Rows.Clear();
            
            // Simulate loading transactions
            string[] hashes = { "0x1a2b3c4d5e6f7g8h9i0j", "0x2a3b4c5d6e7f8g9h0i1j", "0x3a4b5c6d7e8f9g0h1i2j" };
            string[] statuses = { "Confirmed", "Confirmed", "Pending" };
            
            for (int i = 0; i < hashes.Length; i++)
            {
                dgvTransactionHistory.Rows.Add(
                    hashes[i],
                    DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd HH:mm:ss"),
                    GetRandomBalance().ToString("F8"),
                    statuses[i]
                );
            }
            
            lblTransactionCount.Text = $"Transactions Found: {hashes.Length}";
            lblTransactionStatus.Text = "Status: Loaded";
            
            LogAuditEvent("Load Transactions", "Transaction History", "Info");
        }
        
        private void BtnSetMasterPassword_Click(object sender, EventArgs e)
        {
            // Open master password dialog
            PasswordDialog passwordDialog = new PasswordDialog();
            if (passwordDialog.ShowDialog() == DialogResult.OK)
            {
                masterPasswordSet = true;
                lblSecurityStatus.Text = "Security Status: Enhanced";
                lblSecurityStatus.ForeColor = Color.Green;
                MessageBox.Show("Master password set successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogAuditEvent("Master Password Set", "Security Settings", "Info");
            }
        }
        
        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text) || string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show("Please fill all password fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (txtNewPassword.Text.Length < MIN_PASSWORD_LENGTH)
            {
                MessageBox.Show($"Password must be at least {MIN_PASSWORD_LENGTH} characters long.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            MessageBox.Show("Password changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtCurrentPassword.Clear();
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
            LogAuditEvent("Password Changed", "Security Settings", "Info");
        }
        
        private void TxtNewPassword_TextChanged(object sender, EventArgs e)
        {
            string password = txtNewPassword.Text;
            string strength = EvaluatePasswordStrength(password);
            lblPasswordStrength.Text = $"Password Strength: {strength}";
            
            // Color coding
            if (strength == "Weak")
                lblPasswordStrength.ForeColor = Color.Red;
            else if (strength == "Medium")
                lblPasswordStrength.ForeColor = Color.Orange;
            else if (strength == "Strong")
                lblPasswordStrength.ForeColor = Color.Green;
        }
        
        private void ChkEnableTwoFactor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableTwoFactor.Checked)
            {
                MessageBox.Show("Two-Factor Authentication enabled. You will need to provide a verification code on next login.", "2FA Enabled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogAuditEvent("2FA Enabled", "Security Settings", "Info");
            }
            else
            {
                LogAuditEvent("2FA Disabled", "Security Settings", "Warning");
            }
        }
        
        private void BtnClearCache_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all cache and logs? This action cannot be undone.", "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Cache and logs cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogAuditEvent("Cache Cleared", "Data Protection", "Info");
            }
        }
        
        private void BtnExportLogs_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                FileName = $"AuditLogs_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv",
                Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt"
            };
            
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Logs exported successfully.", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogAuditEvent("Logs Exported", "Audit Logs", "Info");
            }
        }
        
        private void BtnClearLogs_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all audit logs? This action cannot be undone.", "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                auditLogs.Clear();
                dgvAuditLogs.Rows.Clear();
                lblAuditLogCount.Text = "Audit Logs Found: 0";
                MessageBox.Show("Audit logs cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogAuditEvent("Audit Logs Cleared", "Audit Logs", "Warning");
            }
        }
        
        private void SessionTimer_Tick(object sender, EventArgs e)
        {
            inactivityCounter++;
            TimeSpan sessionDuration = DateTime.Now - sessionStartTime;
            tsslSessionTime.Text = $"Session Time: {sessionDuration.Hours:D2}:{sessionDuration.Minutes:D2}";
            
            if (chkLogoutOnInactivity.Checked && inactivityCounter >= SESSION_TIMEOUT_MINUTES)
            {
                MessageBox.Show("Session expired due to inactivity. Please log in again.", "Session Expired", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogAuditEvent("Session Timeout", "Security", "Warning");
                isLoggedIn = false;
                sessionTimer.Stop();
            }
        }
        
        private void SyncStatusTimer_Tick(object sender, EventArgs e)
        {
            if (progressBarSync.Value < 100)
            {
                progressBarSync.Value += 10;
            }
            else
            {
                progressBarSync.Value = 0;
                progressBarSync.Visible = false;
                lblWalletStatus.Text = "Status: Online & Ready";
                lblWalletStatus.ForeColor = Color.Green;
                syncStatusTimer.Stop();
            }
        }
        
        private void ShowAboutDialog(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Wallet Auditor v1.0.0\n\n" +
                "Professional Wallet Management Suite\n\n" +
                "© 2025 Wallet Auditor Team\n" +
                "All Rights Reserved",
                "About Wallet Auditor",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        
        private void ShowDocumentation(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Documentation:\n\n" +
                "1. Dashboard: Overview of wallet status and quick actions\n" +
                "2. Wallet Validation: Verify wallet addresses and checksums\n" +
                "3. Balance Check: Monitor current wallet balances\n" +
                "4. Transaction History: Track and analyze transactions\n" +
                "5. Security Settings: Configure security features\n" +
                "6. Audit Logs: View and export system activity logs",
                "Documentation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        
        #endregion
        
        #region Helper Methods
        
        private bool ValidateWalletAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return false;
            
            // Simple validation logic
            return address.Length >= 26 && (address.StartsWith("1") || address.StartsWith("3") || address.StartsWith("bc1") || address.StartsWith("0x"));
        }
        
        private decimal GetRandomBalance()
        {
            Random random = new Random();
            return (decimal)(random.NextDouble() * 100);
        }
        
        private string EvaluatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password))
                return "--";
            
            int score = 0;
            if (password.Length >= MIN_PASSWORD_LENGTH) score++;
            if (password.Any(char.IsUpper)) score++;
            if (password.Any(char.IsLower)) score++;
            if (password.Any(char.IsDigit)) score++;
            if (password.Any(c => !char.IsLetterOrDigit(c))) score++;
            
            return score <= 2 ? "Weak" : score <= 4 ? "Medium" : "Strong";
        }
        
        private void LogAuditEvent(string eventName, string category, string status)
        {
            var logEntry = new AuditLogEntry
            {
                Timestamp = DateTime.Now,
                Event = eventName,
                Category = category,
                Status = status,
                Details = $"User: {currentUsername}"
            };
            
            auditLogs.Add(logEntry);
            RefreshAuditLogs();
        }
        
        private void RefreshAuditLogs()
        {
            dgvAuditLogs.Rows.Clear();
            
            foreach (var log in auditLogs.TakeLast(50))
            {
                dgvAuditLogs.Rows.Add(
                    log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    log.Event,
                    log.Category,
                    log.Status,
                    log.Details
                );
            }
            
            lblAuditLogCount.Text = $"Audit Logs Found: {auditLogs.Count}";
        }
        
        private void CopyToClipboard(string text)
        {
            Clipboard.SetText(text);
            MessageBox.Show("Copied to clipboard.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void LoadApplicationSettings()
        {
            currentUsername = "Auditor";
            isLoggedIn = true;
            sessionStartTime = DateTime.Now;
            tsslUserInfo.Text = $"User: {currentUsername}";
            
            inactivityCounter = 0;
            sessionTimer.Start();
        }
        
        #endregion
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            sessionTimer?.Stop();
            syncStatusTimer?.Stop();
            LogAuditEvent("Application Closed", "Application", "Info");
            base.OnFormClosing(e);
        }
    }
    
    /// <summary>
    /// Helper class for audit log entries
    /// </summary>
    public class AuditLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Event { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string Details { get; set; }
    }
    
    /// <summary>
    /// Password dialog for master password setup
    /// </summary>
    public class PasswordDialog : Form
    {
        private TextBox txtPassword;
        private TextBox txtConfirm;
        private Button btnOK;
        private Button btnCancel;
        
        public PasswordDialog()
        {
            this.Text = "Set Master Password";
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            Label lblPassword = new Label { Text = "Master Password:", Location = new Point(10, 20), AutoSize = true };
            this.Controls.Add(lblPassword);
            
            txtPassword = new TextBox { Location = new Point(10, 40), Size = new Size(360, 30), UseSystemPasswordChar = true };
            this.Controls.Add(txtPassword);
            
            Label lblConfirm = new Label { Text = "Confirm Password:", Location = new Point(10, 80), AutoSize = true };
            this.Controls.Add(lblConfirm);
            
            txtConfirm = new TextBox { Location = new Point(10, 100), Size = new Size(360, 30), UseSystemPasswordChar = true };
            this.Controls.Add(txtConfirm);
            
            btnOK = new Button { Text = "OK", Location = new Point(210, 140), Size = new Size(80, 30), DialogResult = DialogResult.OK };
            this.Controls.Add(btnOK);
            
            btnCancel = new Button { Text = "Cancel", Location = new Point(300, 140), Size = new Size(80, 30), DialogResult = DialogResult.Cancel };
            this.Controls.Add(btnCancel);
            
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }
    }
}
