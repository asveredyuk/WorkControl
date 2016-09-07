namespace WorkControl
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.btSitesReport = new System.Windows.Forms.Button();
            this.btProcessReport = new System.Windows.Forms.Button();
            this.btActiveReport = new System.Windows.Forms.Button();
            this.btWorkReport = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editListsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sitesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btClearLog = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(12, 64);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(554, 254);
            this.textBox1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(491, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btSitesReport
            // 
            this.btSitesReport.Location = new System.Drawing.Point(12, 29);
            this.btSitesReport.Name = "btSitesReport";
            this.btSitesReport.Size = new System.Drawing.Size(75, 23);
            this.btSitesReport.TabIndex = 2;
            this.btSitesReport.Text = "Sites report";
            this.btSitesReport.UseVisualStyleBackColor = true;
            this.btSitesReport.Click += new System.EventHandler(this.btSitesReport_Click);
            // 
            // btProcessReport
            // 
            this.btProcessReport.Location = new System.Drawing.Point(95, 29);
            this.btProcessReport.Name = "btProcessReport";
            this.btProcessReport.Size = new System.Drawing.Size(88, 23);
            this.btProcessReport.TabIndex = 3;
            this.btProcessReport.Text = "Process report";
            this.btProcessReport.UseVisualStyleBackColor = true;
            this.btProcessReport.Click += new System.EventHandler(this.btProcessReport_Click);
            // 
            // btActiveReport
            // 
            this.btActiveReport.Location = new System.Drawing.Point(189, 29);
            this.btActiveReport.Name = "btActiveReport";
            this.btActiveReport.Size = new System.Drawing.Size(75, 23);
            this.btActiveReport.TabIndex = 4;
            this.btActiveReport.Text = "Active report";
            this.btActiveReport.UseVisualStyleBackColor = true;
            this.btActiveReport.Click += new System.EventHandler(this.btActiveReport_Click);
            // 
            // btWorkReport
            // 
            this.btWorkReport.Location = new System.Drawing.Point(270, 29);
            this.btWorkReport.Name = "btWorkReport";
            this.btWorkReport.Size = new System.Drawing.Size(75, 23);
            this.btWorkReport.TabIndex = 5;
            this.btWorkReport.Text = "Work report";
            this.btWorkReport.UseVisualStyleBackColor = true;
            this.btWorkReport.Click += new System.EventHandler(this.btWorkReport_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Text = "WorkControl";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(578, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editListsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // editListsToolStripMenuItem
            // 
            this.editListsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processesToolStripMenuItem,
            this.sitesToolStripMenuItem});
            this.editListsToolStripMenuItem.Name = "editListsToolStripMenuItem";
            this.editListsToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.editListsToolStripMenuItem.Text = "Edit lists";
            // 
            // processesToolStripMenuItem
            // 
            this.processesToolStripMenuItem.Name = "processesToolStripMenuItem";
            this.processesToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.processesToolStripMenuItem.Text = "Processes";
            this.processesToolStripMenuItem.Click += new System.EventHandler(this.processesToolStripMenuItem_Click);
            // 
            // sitesToolStripMenuItem
            // 
            this.sitesToolStripMenuItem.Name = "sitesToolStripMenuItem";
            this.sitesToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.sitesToolStripMenuItem.Text = "Sites";
            this.sitesToolStripMenuItem.Click += new System.EventHandler(this.sitesToolStripMenuItem_Click);
            // 
            // btClearLog
            // 
            this.btClearLog.Location = new System.Drawing.Point(351, 29);
            this.btClearLog.Name = "btClearLog";
            this.btClearLog.Size = new System.Drawing.Size(75, 23);
            this.btClearLog.TabIndex = 7;
            this.btClearLog.Text = "Clear log";
            this.btClearLog.UseVisualStyleBackColor = true;
            this.btClearLog.Click += new System.EventHandler(this.btClearLog_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 308);
            this.Controls.Add(this.btClearLog);
            this.Controls.Add(this.btWorkReport);
            this.Controls.Add(this.btActiveReport);
            this.Controls.Add(this.btProcessReport);
            this.Controls.Add(this.btSitesReport);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btSitesReport;
        private System.Windows.Forms.Button btProcessReport;
        private System.Windows.Forms.Button btActiveReport;
        private System.Windows.Forms.Button btWorkReport;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editListsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sitesToolStripMenuItem;
        private System.Windows.Forms.Button btClearLog;
    }
}

