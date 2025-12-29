using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WalletAuditor.Services;

namespace WalletAuditor.UI
{
    public partial class MainForm : Form
    {
        private WalletService _walletService;
        private AuditService _auditService;
        private ListView walletListView;
        private Button auditButton;
        private Button refreshButton;

        public MainForm()
        {
            InitializeComponent();
            this.Text = "Wallet Auditor";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeServices();
            BuildUI();
            LoadWalletData();
        }

        private void InitializeServices()
        {
            _walletService = new WalletService();
            _auditService = new AuditService();
        }

        private void BuildUI()
        {
            var mainPanel = new Panel { Dock = DockStyle.Fill };

            var toolbarPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = SystemColors.ControlLight
            };

            refreshButton = new Button
            {
                Text = "Refresh",
                Location = new Point(10, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            refreshButton.Click += (s, e) => LoadWalletData();
            toolbarPanel.Controls.Add(refreshButton);

            auditButton = new Button
            {
                Text = "Audit Selected",
                Location = new Point(120, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White
            };
            auditButton.Click += AuditButton_Click;
            toolbarPanel.Controls.Add(auditButton);

            mainPanel.Controls.Add(toolbarPanel);

            walletListView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Location = new Point(0, 50)
            };
            walletListView.Columns.Add("Name", 150);
            walletListView.Columns.Add("Address", 250);
            walletListView.Columns.Add("Currency", 100);
            walletListView.Columns.Add("Balance", 100);

            mainPanel.Controls.Add(walletListView);

            var statusBar = new StatusStrip();
            var statusLabel = new ToolStripStatusLabel("Ready");
            statusBar.Items.Add(statusLabel);
            mainPanel.Controls.Add(statusBar);

            this.Controls.Add(mainPanel);
        }

        private void LoadWalletData()
        {
            try
            {
                walletListView.Items.Clear();
                var wallets = _walletService.GetAllWallets();
                foreach (var wallet in wallets)
                {
                    var item = new ListViewItem(wallet.Name);
                    item.SubItems.Add(wallet.Address);
                    item.SubItems.Add(wallet.Currency);
                    item.SubItems.Add(wallet.Balance.ToString("F8"));
                    item.Tag = wallet;
                    walletListView.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading wallets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void AuditButton_Click(object sender, EventArgs e)
        {
            if (walletListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a wallet to audit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                auditButton.Enabled = false;
                var selectedWallet = walletListView.SelectedItems[0].Tag as Wallet;
                if (selectedWallet != null)
                {
                    var walletData = new WalletData
                    {
                        WalletId = selectedWallet.Name,
                        Balance = selectedWallet.Balance,
                        CreatedDate = selectedWallet.CreatedDate,
                        Transactions = new List<Transaction>()
                    };

                    var auditResult = await _auditService.PerformWalletAuditAsync(selectedWallet.Name, walletData);
                    var message = $"Audit Result: {auditResult.Status}\n\n";
                    message += $"Valid: {auditResult.IsValid}\n";
                    message += $"Errors: {auditResult.Errors.Count}\n";
                    message += $"Warnings: {auditResult.Warnings.Count}";
                    MessageBox.Show(message, "Audit Result", MessageBoxButtons.OK, auditResult.IsValid ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during audit: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                auditButton.Enabled = true;
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private new void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}
