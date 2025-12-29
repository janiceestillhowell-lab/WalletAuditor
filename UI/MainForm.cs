using System;
using System.Windows.Forms;
using System.Linq;
using WalletAuditor.Core;
using WalletAuditor.Services;

namespace WalletAuditor.UI
{
    public partial class MainForm : Form
    {
        private WalletService _walletService;
        private AuditService _auditService;

        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
        }

        private void InitializeServices()
        {
            _walletService = new WalletService();
            _auditService = new AuditService();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadWalletData();
        }

        private void LoadWalletData()
        {
            try
            {
                var wallets = _walletService.GetAllWallets().ToList();
                RefreshWalletList(wallets);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading wallet data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshWalletList(System.Collections.Generic.List<WalletModel> wallets)
        {
            // Clear existing items
            walletListView.Items.Clear();

            // Bind wallet data to list view
            foreach (var wallet in wallets)
            {
                var item = new ListViewItem(wallet.Name);
                item.SubItems.Add(wallet.Balance.ToString("C"));
                item.SubItems.Add(wallet.LastUpdated.ToString("g"));
                walletListView.Items.Add(item);
            }
        }

        private void auditButton_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedItems = walletListView.SelectedItems;
                if (selectedItems.Count == 0)
                {
                    MessageBox.Show("Please select a wallet to audit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedWallet = selectedItems[0].Text;
                var auditResult = _auditService.PerformAudit(selectedWallet);

                DisplayAuditResult(auditResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error performing audit: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayAuditResult(AuditResult auditResult)
        {
            if (auditResult.IsValid)
            {
                MessageBox.Show("Audit passed. No discrepancies found.", "Audit Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var discrepancies = string.Join(Environment.NewLine, auditResult.Discrepancies.Take(10));
                MessageBox.Show($"Audit failed. Discrepancies found:{Environment.NewLine}{discrepancies}", "Audit Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
