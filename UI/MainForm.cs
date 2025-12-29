using System;
using System.Windows.Forms;

namespace WalletAuditor.UI
{
    /// <summary>
    /// MainForm - The main entry point for the WalletAuditor application's user interface.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the MainForm class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            this.Text = "Wallet Auditor";
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Initialize component method for designer-generated code.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Wallet Auditor";
            
            this.ResumeLayout(false);
        }
    }
}
