using System;
using System.Windows.Forms;
using WalletAuditor.Services;

namespace WalletAuditor
{
    /// <summary>
    /// Clean entry point for Wallet Auditor application
    /// </summary>
    public class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    /// <summary>
    /// Simple main form
    /// </summary>
    public class MainForm : Form
    {
        private readonly WalletService _walletService;
        private readonly AuditService _auditService;
        private ListView _walletListView = null!;
        private Button _auditButton = null!;
        private Button _refreshButton = null!;

        public MainForm()
        {
            _walletService = new WalletService();
            _auditService = new AuditService();
            
            InitializeForm();
            BuildUI();
            LoadWallets();
        }

        private void InitializeForm()
        {
            Text = "Wallet Auditor";
            Size = new System.Drawing.Size(800, 500);
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void BuildUI()
        {
            // Toolbar
            var toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = System.Drawing.SystemColors.ControlLight
            };

            _refreshButton = new Button
            {
                Text = "Refresh",
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(100, 30)
            };
            _refreshButton.Click += (s, e) => LoadWallets();

            _auditButton = new Button
            {
                Text = "Audit",
                Location = new System.Drawing.Point(120, 10),
                Size = new System.Drawing.Size(100, 30)
            };
            _auditButton.Click += async (s, e) => await AuditSelectedWallet();

            toolbar.Controls.Add(_refreshButton);
            toolbar.Controls.Add(_auditButton);

            // Wallet list
            _walletListView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            _walletListView.Columns.Add("Name", 200);
            _walletListView.Columns.Add("Address", 350);
            _walletListView.Columns.Add("Balance", 150);

            Controls.Add(_walletListView);
            Controls.Add(toolbar);
        }

        private void LoadWallets()
        {
            _walletListView.Items.Clear();
            var wallets = _walletService.GetAllWallets();
            
            foreach (var wallet in wallets)
            {
                var item = new ListViewItem(wallet.Name);
                item.SubItems.Add(wallet.Address);
                item.SubItems.Add(wallet.Balance.ToString("F2"));
                item.Tag = wallet;
                _walletListView.Items.Add(item);
            }
        }

        private async System.Threading.Tasks.Task AuditSelectedWallet()
        {
            if (_walletListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a wallet to audit.", "Info", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var wallet = _walletListView.SelectedItems[0].Tag as Core.Wallet;
            if (wallet == null) return;

            _auditButton.Enabled = false;
            try
            {
                var result = await _auditService.AuditWalletAsync(wallet);
                var message = $"{result.Message}\n\nFindings:\n{string.Join("\n", result.Findings)}";
                MessageBox.Show(message, "Audit Result", MessageBoxButtons.OK,
                    result.IsSuccessful ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Audit error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _auditButton.Enabled = true;
            }
        }
    }
}
