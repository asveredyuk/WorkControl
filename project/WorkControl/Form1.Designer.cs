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
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(12, 138);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(554, 139);
            this.textBox1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(485, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btSitesReport
            // 
            this.btSitesReport.Location = new System.Drawing.Point(12, 13);
            this.btSitesReport.Name = "btSitesReport";
            this.btSitesReport.Size = new System.Drawing.Size(75, 23);
            this.btSitesReport.TabIndex = 2;
            this.btSitesReport.Text = "Sites report";
            this.btSitesReport.UseVisualStyleBackColor = true;
            this.btSitesReport.Click += new System.EventHandler(this.btSitesReport_Click);
            // 
            // btProcessReport
            // 
            this.btProcessReport.Location = new System.Drawing.Point(94, 13);
            this.btProcessReport.Name = "btProcessReport";
            this.btProcessReport.Size = new System.Drawing.Size(88, 23);
            this.btProcessReport.TabIndex = 3;
            this.btProcessReport.Text = "Process report";
            this.btProcessReport.UseVisualStyleBackColor = true;
            this.btProcessReport.Click += new System.EventHandler(this.btProcessReport_Click);
            // 
            // btActiveReport
            // 
            this.btActiveReport.Location = new System.Drawing.Point(189, 13);
            this.btActiveReport.Name = "btActiveReport";
            this.btActiveReport.Size = new System.Drawing.Size(75, 23);
            this.btActiveReport.TabIndex = 4;
            this.btActiveReport.Text = "Active report";
            this.btActiveReport.UseVisualStyleBackColor = true;
            this.btActiveReport.Click += new System.EventHandler(this.btActiveReport_Click);
            // 
            // btWorkReport
            // 
            this.btWorkReport.Location = new System.Drawing.Point(271, 13);
            this.btWorkReport.Name = "btWorkReport";
            this.btWorkReport.Size = new System.Drawing.Size(75, 23);
            this.btWorkReport.TabIndex = 5;
            this.btWorkReport.Text = "Work report";
            this.btWorkReport.UseVisualStyleBackColor = true;
            this.btWorkReport.Click += new System.EventHandler(this.btWorkReport_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 289);
            this.Controls.Add(this.btWorkReport);
            this.Controls.Add(this.btActiveReport);
            this.Controls.Add(this.btProcessReport);
            this.Controls.Add(this.btSitesReport);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
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
    }
}

