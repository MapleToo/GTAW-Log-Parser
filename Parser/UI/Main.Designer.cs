namespace Parser.UI
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.DirectoryPath = new System.Windows.Forms.RichTextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.Parsed = new System.Windows.Forms.RichTextBox();
            this.CopyParsedToClipboard = new System.Windows.Forms.Button();
            this.SaveParsed = new System.Windows.Forms.Button();
            this.Parse = new System.Windows.Forms.Button();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.PathLabel = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.RemoveTimestamps = new System.Windows.Forms.CheckBox();
            this.DirectoryBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.ServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // DirectoryPath
            // 
            this.DirectoryPath.DetectUrls = false;
            resources.ApplyResources(this.DirectoryPath, "DirectoryPath");
            this.DirectoryPath.Name = "DirectoryPath";
            this.DirectoryPath.ShortcutsEnabled = false;
            this.DirectoryPath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DirectoryPath_MouseClick);
            this.DirectoryPath.TextChanged += new System.EventHandler(this.DirectoryPath_TextChanged);
            this.DirectoryPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DirectoryPath_KeyDown);
            // 
            // Browse
            // 
            resources.ApplyResources(this.Browse, "Browse");
            this.Browse.Name = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Parsed
            // 
            this.Parsed.DetectUrls = false;
            resources.ApplyResources(this.Parsed, "Parsed");
            this.Parsed.Name = "Parsed";
            // 
            // CopyParsedToClipboard
            // 
            resources.ApplyResources(this.CopyParsedToClipboard, "CopyParsedToClipboard");
            this.CopyParsedToClipboard.Name = "CopyParsedToClipboard";
            this.CopyParsedToClipboard.UseVisualStyleBackColor = true;
            this.CopyParsedToClipboard.Click += new System.EventHandler(this.CopyParsedToClipboard_Click);
            // 
            // SaveParsed
            // 
            resources.ApplyResources(this.SaveParsed, "SaveParsed");
            this.SaveParsed.Name = "SaveParsed";
            this.SaveParsed.UseVisualStyleBackColor = true;
            this.SaveParsed.Click += new System.EventHandler(this.SaveParsed_Click);
            // 
            // Parse
            // 
            resources.ApplyResources(this.Parse, "Parse");
            this.Parse.Name = "Parse";
            this.Parse.UseVisualStyleBackColor = true;
            this.Parse.Click += new System.EventHandler(this.Parse_Click);
            // 
            // PathLabel
            // 
            resources.ApplyResources(this.PathLabel, "PathLabel");
            this.PathLabel.Name = "PathLabel";
            // 
            // Version
            // 
            resources.ApplyResources(this.Version, "Version");
            this.Version.Name = "Version";
            // 
            // RemoveTimestamps
            // 
            resources.ApplyResources(this.RemoveTimestamps, "RemoveTimestamps");
            this.RemoveTimestamps.Name = "RemoveTimestamps";
            this.RemoveTimestamps.UseVisualStyleBackColor = true;
            // 
            // DirectoryBrowserDialog
            // 
            resources.ApplyResources(this.DirectoryBrowserDialog, "DirectoryBrowserDialog");
            this.DirectoryBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.DirectoryBrowserDialog.ShowNewFolderButton = false;
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ServerToolStripMenuItem,
            this.AboutToolStripMenuItem});
            resources.ApplyResources(this.MenuStrip, "MenuStrip");
            this.MenuStrip.Name = "MenuStrip";
            // 
            // ServerToolStripMenuItem
            // 
            this.ServerToolStripMenuItem.Name = "ServerToolStripMenuItem";
            resources.ApplyResources(this.ServerToolStripMenuItem, "ServerToolStripMenuItem");
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            resources.ApplyResources(this.AboutToolStripMenuItem, "AboutToolStripMenuItem");
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RemoveTimestamps);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.PathLabel);
            this.Controls.Add(this.Parse);
            this.Controls.Add(this.SaveParsed);
            this.Controls.Add(this.CopyParsedToClipboard);
            this.Controls.Add(this.Parsed);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.DirectoryPath);
            this.Controls.Add(this.MenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.MenuStrip;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox DirectoryPath;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.RichTextBox Parsed;
        private System.Windows.Forms.Button CopyParsedToClipboard;
        private System.Windows.Forms.Button SaveParsed;
        private System.Windows.Forms.Button Parse;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.Label PathLabel;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.CheckBox RemoveTimestamps;
        private System.Windows.Forms.FolderBrowserDialog DirectoryBrowserDialog;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
    }
}

