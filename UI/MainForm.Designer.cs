namespace WalletAuditor.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            
            // Main MenuStrip
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            // Main ToolStrip
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();

            // StatusStrip
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();

            // Main Container Panel
            this.mainPanel = new System.Windows.Forms.Panel();
            
            // Tab Control
            this.tabControl = new System.Windows.Forms.TabControl();
            this.auditTabPage = new System.Windows.Forms.TabPage();
            this.reportTabPage = new System.Windows.Forms.TabPage();
            this.settingsTabPage = new System.Windows.Forms.TabPage();

            // Audit Tab Controls
            this.auditDescriptionLabel = new System.Windows.Forms.Label();
            this.walletListBox = new System.Windows.Forms.ListBox();
            this.auditButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();

            // Report Tab Controls
            this.reportDescriptionLabel = new System.Windows.Forms.Label();
            this.generateReportButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.reportTextBox = new System.Windows.Forms.TextBox();

            // Settings Tab Controls
            this.settingsDescriptionLabel = new System.Windows.Forms.Label();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.autoAuditCheckBox = new System.Windows.Forms.CheckBox();
            this.applySettingsButton = new System.Windows.Forms.Button();
            this.resetSettingsButton = new System.Windows.Forms.Button();

            // MenuStrip Setup
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.fileToolStripMenuItem,
                this.editToolStripMenuItem,
                this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(800, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";

            // File Menu
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.openToolStripMenuItem,
                this.saveToolStripMenuItem,
                this.toolStripSeparator1,
                this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";

            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);

            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);

            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);

            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);

            // Edit Menu
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";

            // Help Menu
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";

            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);

            // ToolStrip Setup
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.openToolStripButton,
                this.saveToolStripButton,
                this.toolStripSeparator2});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 24);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(800, 25);
            this.mainToolStrip.TabIndex = 1;
            this.mainToolStrip.Text = "toolStrip1";

            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open";

            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";

            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);

            // StatusStrip Setup
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 429);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";

            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Ready";

            // Main Panel Setup
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Controls.Add(this.tabControl);
            this.mainPanel.Location = new System.Drawing.Point(0, 49);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(800, 380);
            this.mainPanel.TabIndex = 3;

            // TabControl Setup
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.auditTabPage);
            this.tabControl.Controls.Add(this.reportTabPage);
            this.tabControl.Controls.Add(this.settingsTabPage);
            this.tabControl.Location = new System.Drawing.Point(3, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(794, 374);
            this.tabControl.TabIndex = 0;

            // Audit Tab Page
            this.auditTabPage.Controls.Add(this.refreshButton);
            this.auditTabPage.Controls.Add(this.auditButton);
            this.auditTabPage.Controls.Add(this.walletListBox);
            this.auditTabPage.Controls.Add(this.auditDescriptionLabel);
            this.auditTabPage.Location = new System.Drawing.Point(4, 22);
            this.auditTabPage.Name = "auditTabPage";
            this.auditTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.auditTabPage.Size = new System.Drawing.Size(786, 348);
            this.auditTabPage.TabIndex = 0;
            this.auditTabPage.Text = "Audit";
            this.auditTabPage.UseVisualStyleBackColor = true;

            this.auditDescriptionLabel.AutoSize = true;
            this.auditDescriptionLabel.Location = new System.Drawing.Point(6, 6);
            this.auditDescriptionLabel.Name = "auditDescriptionLabel";
            this.auditDescriptionLabel.Size = new System.Drawing.Size(299, 13);
            this.auditDescriptionLabel.TabIndex = 0;
            this.auditDescriptionLabel.Text = "Review and audit wallet transactions:";

            this.walletListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.walletListBox.FormattingEnabled = true;
            this.walletListBox.Location = new System.Drawing.Point(6, 25);
            this.walletListBox.Name = "walletListBox";
            this.walletListBox.Size = new System.Drawing.Size(774, 290);
            this.walletListBox.TabIndex = 1;

            this.auditButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.auditButton.Location = new System.Drawing.Point(705, 319);
            this.auditButton.Name = "auditButton";
            this.auditButton.Size = new System.Drawing.Size(75, 23);
            this.auditButton.TabIndex = 2;
            this.auditButton.Text = "&Audit";
            this.auditButton.UseVisualStyleBackColor = true;
            this.auditButton.Click += new System.EventHandler(this.AuditButton_Click);

            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.Location = new System.Drawing.Point(624, 319);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 3;
            this.refreshButton.Text = "&Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);

            // Report Tab Page
            this.reportTabPage.Controls.Add(this.exportButton);
            this.reportTabPage.Controls.Add(this.generateReportButton);
            this.reportTabPage.Controls.Add(this.reportTextBox);
            this.reportTabPage.Controls.Add(this.reportDescriptionLabel);
            this.reportTabPage.Location = new System.Drawing.Point(4, 22);
            this.reportTabPage.Name = "reportTabPage";
            this.reportTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.reportTabPage.Size = new System.Drawing.Size(786, 348);
            this.reportTabPage.TabIndex = 1;
            this.reportTabPage.Text = "Report";
            this.reportTabPage.UseVisualStyleBackColor = true;

            this.reportDescriptionLabel.AutoSize = true;
            this.reportDescriptionLabel.Location = new System.Drawing.Point(6, 6);
            this.reportDescriptionLabel.Name = "reportDescriptionLabel";
            this.reportDescriptionLabel.Size = new System.Drawing.Size(217, 13);
            this.reportDescriptionLabel.TabIndex = 0;
            this.reportDescriptionLabel.Text = "Generate and view audit reports:";

            this.reportTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.reportTextBox.Location = new System.Drawing.Point(6, 25);
            this.reportTextBox.Multiline = true;
            this.reportTextBox.Name = "reportTextBox";
            this.reportTextBox.ReadOnly = true;
            this.reportTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.reportTextBox.Size = new System.Drawing.Size(774, 290);
            this.reportTextBox.TabIndex = 1;

            this.generateReportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.generateReportButton.Location = new System.Drawing.Point(624, 319);
            this.generateReportButton.Name = "generateReportButton";
            this.generateReportButton.Size = new System.Drawing.Size(75, 23);
            this.generateReportButton.TabIndex = 2;
            this.generateReportButton.Text = "&Generate";
            this.generateReportButton.UseVisualStyleBackColor = true;
            this.generateReportButton.Click += new System.EventHandler(this.GenerateReportButton_Click);

            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportButton.Location = new System.Drawing.Point(705, 319);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(75, 23);
            this.exportButton.TabIndex = 3;
            this.exportButton.Text = "&Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);

            // Settings Tab Page
            this.settingsTabPage.Controls.Add(this.resetSettingsButton);
            this.settingsTabPage.Controls.Add(this.applySettingsButton);
            this.settingsTabPage.Controls.Add(this.settingsGroupBox);
            this.settingsTabPage.Controls.Add(this.settingsDescriptionLabel);
            this.settingsTabPage.Location = new System.Drawing.Point(4, 22);
            this.settingsTabPage.Name = "settingsTabPage";
            this.settingsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.settingsTabPage.Size = new System.Drawing.Size(786, 348);
            this.settingsTabPage.TabIndex = 2;
            this.settingsTabPage.Text = "Settings";
            this.settingsTabPage.UseVisualStyleBackColor = true;

            this.settingsDescriptionLabel.AutoSize = true;
            this.settingsDescriptionLabel.Location = new System.Drawing.Point(6, 6);
            this.settingsDescriptionLabel.Name = "settingsDescriptionLabel";
            this.settingsDescriptionLabel.Size = new System.Drawing.Size(202, 13);
            this.settingsDescriptionLabel.TabIndex = 0;
            this.settingsDescriptionLabel.Text = "Configure application settings:";

            this.settingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsGroupBox.Controls.Add(this.autoAuditCheckBox);
            this.settingsGroupBox.Location = new System.Drawing.Point(6, 25);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(774, 75);
            this.settingsGroupBox.TabIndex = 1;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Preferences";

            this.autoAuditCheckBox.AutoSize = true;
            this.autoAuditCheckBox.Location = new System.Drawing.Point(6, 19);
            this.autoAuditCheckBox.Name = "autoAuditCheckBox";
            this.autoAuditCheckBox.Size = new System.Drawing.Size(133, 17);
            this.autoAuditCheckBox.TabIndex = 0;
            this.autoAuditCheckBox.Text = "Enable Auto Audit";
            this.autoAuditCheckBox.UseVisualStyleBackColor = true;

            this.applySettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.applySettingsButton.Location = new System.Drawing.Point(624, 319);
            this.applySettingsButton.Name = "applySettingsButton";
            this.applySettingsButton.Size = new System.Drawing.Size(75, 23);
            this.applySettingsButton.TabIndex = 2;
            this.applySettingsButton.Text = "&Apply";
            this.applySettingsButton.UseVisualStyleBackColor = true;
            this.applySettingsButton.Click += new System.EventHandler(this.ApplySettingsButton_Click);

            this.resetSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resetSettingsButton.Location = new System.Drawing.Point(705, 319);
            this.resetSettingsButton.Name = "resetSettingsButton";
            this.resetSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.resetSettingsButton.TabIndex = 3;
            this.resetSettingsButton.Text = "&Reset";
            this.resetSettingsButton.UseVisualStyleBackColor = true;
            this.resetSettingsButton.Click += new System.EventHandler(this.ResetSettingsButton_Click);

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.mainToolStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.Text = "Wallet Auditor";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.auditTabPage.ResumeLayout(false);
            this.auditTabPage.PerformLayout();
            this.reportTabPage.ResumeLayout(false);
            this.reportTabPage.PerformLayout();
            this.settingsTabPage.ResumeLayout(false);
            this.settingsTabPage.PerformLayout();
            this.settingsGroupBox.ResumeLayout(false);
            this.settingsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage auditTabPage;
        private System.Windows.Forms.Label auditDescriptionLabel;
        private System.Windows.Forms.ListBox walletListBox;
        private System.Windows.Forms.Button auditButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.TabPage reportTabPage;
        private System.Windows.Forms.Label reportDescriptionLabel;
        private System.Windows.Forms.TextBox reportTextBox;
        private System.Windows.Forms.Button generateReportButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.TabPage settingsTabPage;
        private System.Windows.Forms.Label settingsDescriptionLabel;
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.CheckBox autoAuditCheckBox;
        private System.Windows.Forms.Button applySettingsButton;
        private System.Windows.Forms.Button resetSettingsButton;
    }
}
