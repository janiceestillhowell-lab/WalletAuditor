using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WalletAuditor.UI
{
    /// <summary>
    /// MainForm - Professional Cryptocurrency Wallet Auditor Application
    /// Features: Multi-tab interface, batch processing, single address checking, and results export
    /// </summary>
    public partial class MainForm : Form
    {
        private TabControl mainTabControl;
        private TabPage batchAuditTab;
        private TabPage singleCheckTab;
        private TabPage resultsTab;
        private TabPage settingsTab;

        // Batch Audit Controls
        private TextBox batchFilePathTextBox;
        private Button browseButton;
        private Button startBatchButton;
        private Button clearBatchButton;
        private DataGridView batchProgressGridView;
        private ProgressBar batchProgressBar;
        private Label batchStatusLabel;
        private CheckBox skipErrorsCheckBox;

        // Single Address Check Controls
        private TextBox addressInputTextBox;
        private ComboBox blockchainComboBox;
        private Button checkAddressButton;
        private DataGridView addressResultsGridView;
        private Label addressStatusLabel;

        // Results Tab Controls
        private DataGridView resultsGridView;
        private Button exportCSVButton;
        private Button exportJSONButton;
        private Button exportXMLButton;
        private Button clearResultsButton;
        private ComboBox filterComboBox;

        // Settings Tab Controls
        private CheckBox verifySignaturesCheckBox;
        private CheckBox enableDetailedLogsCheckBox;
        private TextBox timeoutTextBox;
        private Button saveSettingsButton;
        private Label settingsStatusLabel;

        private List<AuditResult> auditResults = new List<AuditResult>();
        private bool isProcessing = false;

        public MainForm()
        {
            InitializeComponent();
            this.Text = "Wallet Auditor - Professional Cryptocurrency Wallet Analysis Tool";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Icon = SystemIcons.Application;
        }

        private void InitializeComponent()
        {
            // Main Container
            this.SuspendLayout();

            // Create Tab Control
            mainTabControl = new TabControl();
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Padding = new Point(10, 5);

            // Initialize Tabs
            InitializeBatchAuditTab();
            InitializeSingleCheckTab();
            InitializeResultsTab();
            InitializeSettingsTab();

            mainTabControl.TabPages.Add(batchAuditTab);
            mainTabControl.TabPages.Add(singleCheckTab);
            mainTabControl.TabPages.Add(resultsTab);
            mainTabControl.TabPages.Add(settingsTab);

            this.Controls.Add(mainTabControl);
            this.ResumeLayout(false);
        }

        private void InitializeBatchAuditTab()
        {
            batchAuditTab = new TabPage();
            batchAuditTab.Text = "Batch Audit";
            batchAuditTab.BackColor = Color.FromArgb(245, 245, 245);

            var panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(15);
            panel.ColumnCount = 3;
            panel.RowCount = 7;
            panel.AutoSize = true;

            // Title
            var titleLabel = new Label();
            titleLabel.Text = "Batch Wallet Audit Processing";
            titleLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            titleLabel.AutoSize = true;
            panel.Controls.Add(titleLabel, 0, 0);
            panel.SetColumnSpan(titleLabel, 3);

            // File Selection Section
            var fileLabel = new Label();
            fileLabel.Text = "Select Audit File (.csv, .txt, .json):";
            fileLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            fileLabel.AutoSize = true;
            panel.Controls.Add(fileLabel, 0, 1);

            batchFilePathTextBox = new TextBox();
            batchFilePathTextBox.ReadOnly = true;
            batchFilePathTextBox.Width = 600;
            panel.Controls.Add(batchFilePathTextBox, 0, 2);

            browseButton = new Button();
            browseButton.Text = "Browse...";
            browseButton.Click += BrowseButton_Click;
            browseButton.Width = 100;
            browseButton.Height = 25;
            panel.Controls.Add(browseButton, 1, 2);

            // Options Section
            skipErrorsCheckBox = new CheckBox();
            skipErrorsCheckBox.Text = "Skip errors and continue processing";
            skipErrorsCheckBox.Checked = true;
            skipErrorsCheckBox.AutoSize = true;
            panel.Controls.Add(skipErrorsCheckBox, 0, 3);
            panel.SetColumnSpan(skipErrorsCheckBox, 2);

            // Progress Section
            var progressLabel = new Label();
            progressLabel.Text = "Processing Progress:";
            progressLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            progressLabel.AutoSize = true;
            panel.Controls.Add(progressLabel, 0, 4);

            batchProgressBar = new ProgressBar();
            batchProgressBar.Width = 600;
            batchProgressBar.Height = 25;
            panel.Controls.Add(batchProgressBar, 0, 5);
            panel.SetColumnSpan(batchProgressBar, 2);

            // Status and Buttons Section
            var buttonPanel = new FlowLayoutPanel();
            buttonPanel.AutoSize = true;
            buttonPanel.FlowDirection = FlowDirection.LeftToRight;

            startBatchButton = new Button();
            startBatchButton.Text = "Start Batch Audit";
            startBatchButton.Click += StartBatchButton_Click;
            startBatchButton.Width = 120;
            startBatchButton.Height = 35;
            startBatchButton.BackColor = Color.FromArgb(0, 120, 215);
            startBatchButton.ForeColor = Color.White;
            startBatchButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            buttonPanel.Controls.Add(startBatchButton);

            clearBatchButton = new Button();
            clearBatchButton.Text = "Clear";
            clearBatchButton.Click += ClearBatchButton_Click;
            clearBatchButton.Width = 100;
            clearBatchButton.Height = 35;
            buttonPanel.Controls.Add(clearBatchButton);

            panel.Controls.Add(buttonPanel, 0, 6);
            panel.SetColumnSpan(buttonPanel, 2);

            batchStatusLabel = new Label();
            batchStatusLabel.Text = "Ready";
            batchStatusLabel.Font = new Font("Segoe UI", 9);
            batchStatusLabel.AutoSize = true;
            panel.Controls.Add(batchStatusLabel, 0, 6);

            // Progress Grid
            batchProgressGridView = new DataGridView();
            batchProgressGridView.AllowUserToAddRows = false;
            batchProgressGridView.AllowUserToDeleteRows = false;
            batchProgressGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            batchProgressGridView.Height = 300;
            batchProgressGridView.Dock = DockStyle.Bottom;
            batchProgressGridView.ColumnCount = 4;
            batchProgressGridView.Columns[0].Name = "Address";
            batchProgressGridView.Columns[1].Name = "Status";
            batchProgressGridView.Columns[2].Name = "Risk Level";
            batchProgressGridView.Columns[3].Name = "Details";
            batchProgressGridView.ReadOnly = true;

            batchAuditTab.Controls.Add(panel);
            batchAuditTab.Controls.Add(batchProgressGridView);
        }

        private void InitializeSingleCheckTab()
        {
            singleCheckTab = new TabPage();
            singleCheckTab.Text = "Single Address Check";
            singleCheckTab.BackColor = Color.FromArgb(245, 245, 245);

            var panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(15);
            panel.ColumnCount = 3;
            panel.RowCount = 6;
            panel.AutoSize = true;

            // Title
            var titleLabel = new Label();
            titleLabel.Text = "Analyze Single Cryptocurrency Address";
            titleLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            titleLabel.AutoSize = true;
            panel.Controls.Add(titleLabel, 0, 0);
            panel.SetColumnSpan(titleLabel, 3);

            // Blockchain Selection
            var blockchainLabel = new Label();
            blockchainLabel.Text = "Blockchain:";
            blockchainLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            blockchainLabel.AutoSize = true;
            panel.Controls.Add(blockchainLabel, 0, 1);

            blockchainComboBox = new ComboBox();
            blockchainComboBox.Items.AddRange(new string[] { "Bitcoin", "Ethereum", "Ripple", "Litecoin", "Cardano", "Solana", "Polkadot", "Monero" });
            blockchainComboBox.SelectedIndex = 0;
            blockchainComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            blockchainComboBox.Width = 150;
            panel.Controls.Add(blockchainComboBox, 1, 1);

            // Address Input
            var addressLabel = new Label();
            addressLabel.Text = "Wallet Address:";
            addressLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            addressLabel.AutoSize = true;
            panel.Controls.Add(addressLabel, 0, 2);

            addressInputTextBox = new TextBox();
            addressInputTextBox.Multiline = false;
            addressInputTextBox.Width = 600;
            addressInputTextBox.Height = 25;
            panel.Controls.Add(addressInputTextBox, 0, 3);
            panel.SetColumnSpan(addressInputTextBox, 2);

            // Check Button
            checkAddressButton = new Button();
            checkAddressButton.Text = "Analyze Address";
            checkAddressButton.Click += CheckAddressButton_Click;
            checkAddressButton.Width = 120;
            checkAddressButton.Height = 35;
            checkAddressButton.BackColor = Color.FromArgb(0, 120, 215);
            checkAddressButton.ForeColor = Color.White;
            checkAddressButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            panel.Controls.Add(checkAddressButton, 1, 3);

            // Status
            addressStatusLabel = new Label();
            addressStatusLabel.Text = "Ready to analyze";
            addressStatusLabel.Font = new Font("Segoe UI", 9);
            addressStatusLabel.AutoSize = true;
            panel.Controls.Add(addressStatusLabel, 0, 4);

            // Results Grid
            addressResultsGridView = new DataGridView();
            addressResultsGridView.AllowUserToAddRows = false;
            addressResultsGridView.AllowUserToDeleteRows = false;
            addressResultsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            addressResultsGridView.Height = 350;
            addressResultsGridView.ColumnCount = 2;
            addressResultsGridView.Columns[0].Name = "Property";
            addressResultsGridView.Columns[1].Name = "Value";
            addressResultsGridView.ReadOnly = true;

            singleCheckTab.Controls.Add(panel);
            singleCheckTab.Controls.Add(addressResultsGridView);
        }

        private void InitializeResultsTab()
        {
            resultsTab = new TabPage();
            resultsTab.Text = "Results & Export";
            resultsTab.BackColor = Color.FromArgb(245, 245, 245);

            var panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Top;
            panel.Padding = new Padding(15);
            panel.ColumnCount = 4;
            panel.RowCount = 2;
            panel.Height = 80;

            // Title
            var titleLabel = new Label();
            titleLabel.Text = "Audit Results Management";
            titleLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            titleLabel.AutoSize = true;
            panel.Controls.Add(titleLabel, 0, 0);
            panel.SetColumnSpan(titleLabel, 4);

            // Filter
            var filterLabel = new Label();
            filterLabel.Text = "Filter by Risk:";
            filterLabel.Font = new Font("Segoe UI", 9);
            filterLabel.AutoSize = true;
            panel.Controls.Add(filterLabel, 0, 1);

            filterComboBox = new ComboBox();
            filterComboBox.Items.AddRange(new string[] { "All", "Critical", "High", "Medium", "Low" });
            filterComboBox.SelectedIndex = 0;
            filterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            filterComboBox.Width = 120;
            filterComboBox.SelectedIndexChanged += FilterComboBox_SelectedIndexChanged;
            panel.Controls.Add(filterComboBox, 1, 1);

            // Export Buttons
            exportCSVButton = new Button();
            exportCSVButton.Text = "Export CSV";
            exportCSVButton.Click += ExportCSVButton_Click;
            exportCSVButton.Width = 100;
            exportCSVButton.Height = 30;
            panel.Controls.Add(exportCSVButton, 2, 1);

            exportJSONButton = new Button();
            exportJSONButton.Text = "Export JSON";
            exportJSONButton.Click += ExportJSONButton_Click;
            exportJSONButton.Width = 100;
            exportJSONButton.Height = 30;
            panel.Controls.Add(exportJSONButton, 3, 1);

            exportXMLButton = new Button();
            exportXMLButton.Text = "Export XML";
            exportXMLButton.Click += ExportXMLButton_Click;
            exportXMLButton.Width = 100;
            exportXMLButton.Height = 30;
            panel.Controls.Add(exportXMLButton, 0, 2);

            clearResultsButton = new Button();
            clearResultsButton.Text = "Clear Results";
            clearResultsButton.Click += ClearResultsButton_Click;
            clearResultsButton.Width = 100;
            clearResultsButton.Height = 30;
            panel.Controls.Add(clearResultsButton, 1, 2);

            // Results Grid
            resultsGridView = new DataGridView();
            resultsGridView.AllowUserToAddRows = false;
            resultsGridView.AllowUserToDeleteRows = false;
            resultsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            resultsGridView.Dock = DockStyle.Fill;
            resultsGridView.ColumnCount = 8;
            resultsGridView.Columns[0].Name = "Address";
            resultsGridView.Columns[1].Name = "Blockchain";
            resultsGridView.Columns[2].Name = "Risk Level";
            resultsGridView.Columns[3].Name = "Status";
            resultsGridView.Columns[4].Name = "Transaction Count";
            resultsGridView.Columns[5].Name = "Balance";
            resultsGridView.Columns[6].Name = "Last Activity";
            resultsGridView.Columns[7].Name = "Issues Detected";
            resultsGridView.ReadOnly = true;

            resultsTab.Controls.Add(resultsGridView);
            resultsTab.Controls.Add(panel);
        }

        private void InitializeSettingsTab()
        {
            settingsTab = new TabPage();
            settingsTab.Text = "Settings";
            settingsTab.BackColor = Color.FromArgb(245, 245, 245);

            var panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Top;
            panel.Padding = new Padding(15);
            panel.ColumnCount = 2;
            panel.RowCount = 5;
            panel.Height = 250;
            panel.AutoSize = true;

            // Title
            var titleLabel = new Label();
            titleLabel.Text = "Application Settings";
            titleLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            titleLabel.AutoSize = true;
            panel.Controls.Add(titleLabel, 0, 0);
            panel.SetColumnSpan(titleLabel, 2);

            // Verify Signatures
            verifySignaturesCheckBox = new CheckBox();
            verifySignaturesCheckBox.Text = "Verify cryptographic signatures";
            verifySignaturesCheckBox.Checked = true;
            verifySignaturesCheckBox.AutoSize = true;
            panel.Controls.Add(verifySignaturesCheckBox, 0, 1);

            // Detailed Logs
            enableDetailedLogsCheckBox = new CheckBox();
            enableDetailedLogsCheckBox.Text = "Enable detailed audit logs";
            enableDetailedLogsCheckBox.Checked = true;
            enableDetailedLogsCheckBox.AutoSize = true;
            panel.Controls.Add(enableDetailedLogsCheckBox, 0, 2);

            // Timeout
            var timeoutLabel = new Label();
            timeoutLabel.Text = "API Request Timeout (seconds):";
            timeoutLabel.Font = new Font("Segoe UI", 9);
            timeoutLabel.AutoSize = true;
            panel.Controls.Add(timeoutLabel, 0, 3);

            timeoutTextBox = new TextBox();
            timeoutTextBox.Text = "30";
            timeoutTextBox.Width = 100;
            panel.Controls.Add(timeoutTextBox, 1, 3);

            // Save Button
            saveSettingsButton = new Button();
            saveSettingsButton.Text = "Save Settings";
            saveSettingsButton.Click += SaveSettingsButton_Click;
            saveSettingsButton.Width = 120;
            saveSettingsButton.Height = 35;
            saveSettingsButton.BackColor = Color.FromArgb(0, 120, 215);
            saveSettingsButton.ForeColor = Color.White;
            saveSettingsButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            panel.Controls.Add(saveSettingsButton, 0, 4);

            // Status
            settingsStatusLabel = new Label();
            settingsStatusLabel.Text = "Settings loaded";
            settingsStatusLabel.Font = new Font("Segoe UI", 9);
            settingsStatusLabel.AutoSize = true;
            panel.Controls.Add(settingsStatusLabel, 1, 4);

            settingsTab.Controls.Add(panel);
        }

        // Event Handlers
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    batchFilePathTextBox.Text = openFileDialog.FileName;
                    batchStatusLabel.Text = $"File loaded: {Path.GetFileName(openFileDialog.FileName)}";
                }
            }
        }

        private void StartBatchButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(batchFilePathTextBox.Text))
            {
                MessageBox.Show("Please select a batch file first.", "No File Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(batchFilePathTextBox.Text))
            {
                MessageBox.Show("Selected file does not exist.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            isProcessing = true;
            startBatchButton.Enabled = false;
            browseButton.Enabled = false;
            batchStatusLabel.Text = "Processing batch audit...";

            // Simulate batch processing
            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += (s, e) => SimulateBatchAudit();
            worker.RunWorkerCompleted += (s, e) =>
            {
                isProcessing = false;
                startBatchButton.Enabled = true;
                browseButton.Enabled = true;
                batchStatusLabel.Text = "Batch audit completed";
                MessageBox.Show("Batch audit processing completed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            worker.RunWorkerAsync();
        }

        private void SimulateBatchAudit()
        {
            var addresses = new[] { "1A1z7agoat2YYSW3PLeM09xvjG5Wrx6KF", "3J98t1WpEZ73CNmYviecrnyiWrnqRhWNLy", "1BvBMSEYstWetqTFn5Au4m4GFg7xJaNVN2" };
            var riskLevels = new[] { "Low", "Medium", "High", "Critical" };

            for (int i = 0; i < addresses.Length; i++)
            {
                var result = new AuditResult
                {
                    Address = addresses[i],
                    Blockchain = "Bitcoin",
                    RiskLevel = riskLevels[i % riskLevels.Length],
                    Status = "Completed",
                    TransactionCount = (i + 1) * 150,
                    Balance = (i + 1) * 2.5m,
                    LastActivity = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd"),
                    IssuesDetected = i > 0 ? $"{i} issues found" : "No issues"
                };

                auditResults.Add(result);

                this.Invoke((MethodInvoker)(() =>
                {
                    batchProgressGridView.Rows.Add(result.Address, result.Status, result.RiskLevel, result.IssuesDetected);
                    batchProgressBar.Value = (int)((i + 1) / (float)addresses.Length * 100);
                    UpdateResultsGrid();
                }));

                System.Threading.Thread.Sleep(500); // Simulate processing delay
            }
        }

        private void ClearBatchButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear batch progress data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                batchProgressGridView.Rows.Clear();
                batchProgressBar.Value = 0;
                batchFilePathTextBox.Text = "";
                batchStatusLabel.Text = "Cleared";
            }
        }

        private void CheckAddressButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(addressInputTextBox.Text))
            {
                MessageBox.Show("Please enter a wallet address.", "No Address", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            addressStatusLabel.Text = "Analyzing address...";
            checkAddressButton.Enabled = false;

            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += (s, e) => SimulateAddressAnalysis();
            worker.RunWorkerCompleted += (s, e) =>
            {
                checkAddressButton.Enabled = true;
                addressStatusLabel.Text = "Analysis complete";
            };
            worker.RunWorkerAsync();
        }

        private void SimulateAddressAnalysis()
        {
            var analysisData = new Dictionary<string, string>
            {
                { "Address", addressInputTextBox.Text },
                { "Blockchain", blockchainComboBox.SelectedItem.ToString() },
                { "Account Age", "2017-03-15" },
                { "Total Transactions", "342" },
                { "Current Balance", "4.52 BTC" },
                { "Last Activity", "2 hours ago" },
                { "Risk Assessment", "Low" },
                { "Connected Addresses", "12" },
                { "Wallet Type", "Exchange Custody" },
                { "Status", "Active" }
            };

            System.Threading.Thread.Sleep(1000); // Simulate API call

            this.Invoke((MethodInvoker)(() =>
            {
                addressResultsGridView.Rows.Clear();
                foreach (var item in analysisData)
                {
                    addressResultsGridView.Rows.Add(item.Key, item.Value);
                }

                var result = new AuditResult
                {
                    Address = addressInputTextBox.Text,
                    Blockchain = blockchainComboBox.SelectedItem.ToString(),
                    RiskLevel = "Low",
                    Status = "Active",
                    TransactionCount = 342,
                    Balance = 4.52m,
                    LastActivity = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd"),
                    IssuesDetected = "None"
                };

                auditResults.Add(result);
                UpdateResultsGrid();
            }));
        }

        private void ExportCSVButton_Click(object sender, EventArgs e)
        {
            if (auditResults.Count == 0)
            {
                MessageBox.Show("No results to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
                saveFileDialog.FileName = $"AuditResults_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(saveFileDialog.FileName);
                    MessageBox.Show($"Results exported to {saveFileDialog.FileName}", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ExportToCSV(string filePath)
        {
            var results = GetFilteredResults();
            var sb = new StringBuilder();
            sb.AppendLine("Address,Blockchain,Risk Level,Status,Transaction Count,Balance,Last Activity,Issues Detected");

            foreach (var result in results)
            {
                sb.AppendLine($"\"{result.Address}\",\"{result.Blockchain}\",\"{result.RiskLevel}\",\"{result.Status}\",{result.TransactionCount},{result.Balance},\"{result.LastActivity}\",\"{result.IssuesDetected}\"");
            }

            File.WriteAllText(filePath, sb.ToString());
        }

        private void ExportJSONButton_Click(object sender, EventArgs e)
        {
            if (auditResults.Count == 0)
            {
                MessageBox.Show("No results to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON Files (*.json)|*.json";
                saveFileDialog.FileName = $"AuditResults_{DateTime.Now:yyyyMMdd_HHmmss}.json";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToJSON(saveFileDialog.FileName);
                    MessageBox.Show($"Results exported to {saveFileDialog.FileName}", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ExportToJSON(string filePath)
        {
            var results = GetFilteredResults();
            var json = System.Text.Json.JsonSerializer.Serialize(results, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        private void ExportXMLButton_Click(object sender, EventArgs e)
        {
            if (auditResults.Count == 0)
            {
                MessageBox.Show("No results to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
                saveFileDialog.FileName = $"AuditResults_{DateTime.Now:yyyyMMdd_HHmmss}.xml";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToXML(saveFileDialog.FileName);
                    MessageBox.Show($"Results exported to {saveFileDialog.FileName}", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ExportToXML(string filePath)
        {
            var results = GetFilteredResults();
            var xdoc = new System.Xml.XmlDocument();
            var root = xdoc.CreateElement("AuditResults");
            xdoc.AppendChild(root);

            foreach (var result in results)
            {
                var element = xdoc.CreateElement("Result");
                element.SetAttribute("address", result.Address);
                element.SetAttribute("blockchain", result.Blockchain);
                element.SetAttribute("riskLevel", result.RiskLevel);
                element.SetAttribute("status", result.Status);
                element.SetAttribute("transactionCount", result.TransactionCount.ToString());
                element.SetAttribute("balance", result.Balance.ToString());
                element.SetAttribute("lastActivity", result.LastActivity);
                element.SetAttribute("issuesDetected", result.IssuesDetected);
                root.AppendChild(element);
            }

            xdoc.Save(filePath);
        }

        private void ClearResultsButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear all audit results?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                auditResults.Clear();
                resultsGridView.Rows.Clear();
                batchProgressGridView.Rows.Clear();
                addressResultsGridView.Rows.Clear();
            }
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResultsGrid();
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(timeoutTextBox.Text, out int timeout) || timeout <= 0)
            {
                MessageBox.Show("Invalid timeout value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            settingsStatusLabel.Text = "Settings saved successfully";
            MessageBox.Show("Settings have been saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateResultsGrid()
        {
            resultsGridView.Rows.Clear();
            var results = GetFilteredResults();

            foreach (var result in results)
            {
                resultsGridView.Rows.Add(
                    result.Address,
                    result.Blockchain,
                    result.RiskLevel,
                    result.Status,
                    result.TransactionCount,
                    result.Balance,
                    result.LastActivity,
                    result.IssuesDetected
                );
            }
        }

        private List<AuditResult> GetFilteredResults()
        {
            string selectedFilter = filterComboBox.SelectedItem?.ToString() ?? "All";
            if (selectedFilter == "All")
                return auditResults;

            return auditResults.Where(r => r.RiskLevel == selectedFilter).ToList();
        }
    }

    /// <summary>
    /// Data model for audit results
    /// </summary>
    public class AuditResult
    {
        public string Address { get; set; }
        public string Blockchain { get; set; }
        public string RiskLevel { get; set; }
        public string Status { get; set; }
        public int TransactionCount { get; set; }
        public decimal Balance { get; set; }
        public string LastActivity { get; set; }
        public string IssuesDetected { get; set; }
    }
}
