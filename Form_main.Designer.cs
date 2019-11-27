namespace Sword
{
    partial class Form_main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_main));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.label_1 = new System.Windows.Forms.Label();
            this.label_numMsgRx = new System.Windows.Forms.Label();
            this.label_msgRate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_peakMsgRate = new System.Windows.Forms.Label();
            this.label_Time = new System.Windows.Forms.Label();
            this.label_msgRateAvg = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_msgRate_T1 = new System.Windows.Forms.Label();
            this.label_msgRate_T2 = new System.Windows.Forms.Label();
            this.label_msgRate_T3 = new System.Windows.Forms.Label();
            this.label_msgRate_T4 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_pub = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_sub = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_root = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "DDS";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // label_1
            // 
            this.label_1.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_1.Location = new System.Drawing.Point(16, 9);
            this.label_1.Name = "label_1";
            this.label_1.Size = new System.Drawing.Size(350, 24);
            this.label_1.TabIndex = 0;
            this.label_1.Text = "Number of message received";
            // 
            // label_numMsgRx
            // 
            this.label_numMsgRx.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_numMsgRx.Location = new System.Drawing.Point(249, 33);
            this.label_numMsgRx.Name = "label_numMsgRx";
            this.label_numMsgRx.Size = new System.Drawing.Size(117, 24);
            this.label_numMsgRx.TabIndex = 1;
            this.label_numMsgRx.Text = "0";
            this.label_numMsgRx.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_msgRate
            // 
            this.label_msgRate.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_msgRate.Location = new System.Drawing.Point(284, 78);
            this.label_msgRate.Name = "label_msgRate";
            this.label_msgRate.Size = new System.Drawing.Size(82, 24);
            this.label_msgRate.TabIndex = 3;
            this.label_msgRate.Text = "0";
            this.label_msgRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(346, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Message Rate Per Sec";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(342, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Peak Message Rate ";
            // 
            // label_peakMsgRate
            // 
            this.label_peakMsgRate.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_peakMsgRate.Location = new System.Drawing.Point(245, 177);
            this.label_peakMsgRate.Name = "label_peakMsgRate";
            this.label_peakMsgRate.Size = new System.Drawing.Size(121, 24);
            this.label_peakMsgRate.TabIndex = 5;
            this.label_peakMsgRate.Text = "0";
            this.label_peakMsgRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_Time
            // 
            this.label_Time.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Time.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_Time.Location = new System.Drawing.Point(16, 177);
            this.label_Time.Name = "label_Time";
            this.label_Time.Size = new System.Drawing.Size(215, 24);
            this.label_Time.TabIndex = 6;
            this.label_Time.Text = "N/A";
            this.label_Time.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_msgRateAvg
            // 
            this.label_msgRateAvg.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_msgRateAvg.Location = new System.Drawing.Point(235, 129);
            this.label_msgRateAvg.Name = "label_msgRateAvg";
            this.label_msgRateAvg.Size = new System.Drawing.Size(131, 24);
            this.label_msgRateAvg.TabIndex = 8;
            this.label_msgRateAvg.Text = " N/A";
            this.label_msgRateAvg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(342, 24);
            this.label4.TabIndex = 7;
            this.label4.Text = "5 sec Mov. Avg. Msg Rate";
            // 
            // label_msgRate_T1
            // 
            this.label_msgRate_T1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_msgRate_T1.ForeColor = System.Drawing.Color.DimGray;
            this.label_msgRate_T1.Location = new System.Drawing.Point(201, 79);
            this.label_msgRate_T1.Name = "label_msgRate_T1";
            this.label_msgRate_T1.Size = new System.Drawing.Size(66, 24);
            this.label_msgRate_T1.TabIndex = 9;
            this.label_msgRate_T1.Text = "0";
            this.label_msgRate_T1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_msgRate_T2
            // 
            this.label_msgRate_T2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_msgRate_T2.ForeColor = System.Drawing.Color.Gray;
            this.label_msgRate_T2.Location = new System.Drawing.Point(129, 80);
            this.label_msgRate_T2.Name = "label_msgRate_T2";
            this.label_msgRate_T2.Size = new System.Drawing.Size(66, 24);
            this.label_msgRate_T2.TabIndex = 10;
            this.label_msgRate_T2.Text = "0";
            this.label_msgRate_T2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_msgRate_T3
            // 
            this.label_msgRate_T3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_msgRate_T3.ForeColor = System.Drawing.Color.DarkGray;
            this.label_msgRate_T3.Location = new System.Drawing.Point(73, 81);
            this.label_msgRate_T3.Name = "label_msgRate_T3";
            this.label_msgRate_T3.Size = new System.Drawing.Size(50, 24);
            this.label_msgRate_T3.TabIndex = 11;
            this.label_msgRate_T3.Text = "0";
            this.label_msgRate_T3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_msgRate_T4
            // 
            this.label_msgRate_T4.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_msgRate_T4.ForeColor = System.Drawing.Color.Silver;
            this.label_msgRate_T4.Location = new System.Drawing.Point(20, 81);
            this.label_msgRate_T4.Name = "label_msgRate_T4";
            this.label_msgRate_T4.Size = new System.Drawing.Size(47, 24);
            this.label_msgRate_T4.TabIndex = 12;
            this.label_msgRate_T4.Text = "0";
            this.label_msgRate_T4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_pub,
            this.toolStripStatusLabel_sub,
            this.toolStripStatusLabel_root});
            this.statusStrip1.Location = new System.Drawing.Point(0, 206);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(380, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_pub
            // 
            this.toolStripStatusLabel_pub.AutoSize = false;
            this.toolStripStatusLabel_pub.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel_pub.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel_pub.Name = "toolStripStatusLabel_pub";
            this.toolStripStatusLabel_pub.Size = new System.Drawing.Size(124, 17);
            this.toolStripStatusLabel_pub.Text = "Pub : N/A";
            this.toolStripStatusLabel_pub.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel_sub
            // 
            this.toolStripStatusLabel_sub.AutoSize = false;
            this.toolStripStatusLabel_sub.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel_sub.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel_sub.Name = "toolStripStatusLabel_sub";
            this.toolStripStatusLabel_sub.Size = new System.Drawing.Size(124, 17);
            this.toolStripStatusLabel_sub.Text = "Sub : N/A";
            this.toolStripStatusLabel_sub.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel_root
            // 
            this.toolStripStatusLabel_root.AutoSize = false;
            this.toolStripStatusLabel_root.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel_root.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel_root.Name = "toolStripStatusLabel_root";
            this.toolStripStatusLabel_root.Size = new System.Drawing.Size(124, 17);
            this.toolStripStatusLabel_root.Text = "Root : N/A";
            this.toolStripStatusLabel_root.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Form_main
            // 
            this.ClientSize = new System.Drawing.Size(380, 228);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label_msgRate_T4);
            this.Controls.Add(this.label_msgRate_T3);
            this.Controls.Add(this.label_msgRate_T2);
            this.Controls.Add(this.label_msgRate_T1);
            this.Controls.Add(this.label_msgRateAvg);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label_Time);
            this.Controls.Add(this.label_peakMsgRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_msgRate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_numMsgRx);
            this.Controls.Add(this.label_1);
            this.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_main";
            this.Text = "DDS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_main_FormClosing);
            this.Load += new System.EventHandler(this.Form_main_Load);
            this.Resize += new System.EventHandler(this.Form_server_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion        
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label label_1;
        private System.Windows.Forms.Label label_numMsgRx;
        private System.Windows.Forms.Label label_msgRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_peakMsgRate;
        private System.Windows.Forms.Label label_Time;
        private System.Windows.Forms.Label label_msgRateAvg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_msgRate_T1;
        private System.Windows.Forms.Label label_msgRate_T2;
        private System.Windows.Forms.Label label_msgRate_T3;
        private System.Windows.Forms.Label label_msgRate_T4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_pub;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_sub;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_root;
    }
}