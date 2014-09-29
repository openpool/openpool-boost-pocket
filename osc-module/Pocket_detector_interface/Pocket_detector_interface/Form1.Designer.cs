namespace Pocket_detector_interface
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pocketdetector_worker = new System.ComponentModel.BackgroundWorker();
            this.message_box_PD = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.comport_box_PD = new System.Windows.Forms.ComboBox();
            this.status_box_PD = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.status_box_SC = new System.Windows.Forms.TextBox();
            this.comport_box_SC = new System.Windows.Forms.ComboBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.message_box_SC = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.smartcue_worker = new System.ComponentModel.BackgroundWorker();
            this.status_box_all = new System.Windows.Forms.TextBox();
            this.comport_scan_worker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // pocketdetector_worker
            // 
            this.pocketdetector_worker.WorkerSupportsCancellation = true;
            this.pocketdetector_worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.pocketdetector_worker_DoWork);
            // 
            // message_box_PD
            // 
            this.message_box_PD.Location = new System.Drawing.Point(44, 145);
            this.message_box_PD.Multiline = true;
            this.message_box_PD.Name = "message_box_PD";
            this.message_box_PD.ReadOnly = true;
            this.message_box_PD.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.message_box_PD.Size = new System.Drawing.Size(295, 160);
            this.message_box_PD.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1.Location = new System.Drawing.Point(44, 51);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(267, 18);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "Select COM port";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2.Location = new System.Drawing.Point(44, 121);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(267, 18);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "Message log";
            // 
            // comport_box_PD
            // 
            this.comport_box_PD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comport_box_PD.FormattingEnabled = true;
            this.comport_box_PD.Items.AddRange(new object[] {
            "No COM port"});
            this.comport_box_PD.Location = new System.Drawing.Point(44, 75);
            this.comport_box_PD.Name = "comport_box_PD";
            this.comport_box_PD.Size = new System.Drawing.Size(295, 26);
            this.comport_box_PD.TabIndex = 7;
            this.comport_box_PD.SelectedIndexChanged += new System.EventHandler(this.comport_box_PD_SelectedIndexChanged);
            // 
            // status_box_PD
            // 
            this.status_box_PD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.status_box_PD.Location = new System.Drawing.Point(44, 311);
            this.status_box_PD.Name = "status_box_PD";
            this.status_box_PD.ReadOnly = true;
            this.status_box_PD.Size = new System.Drawing.Size(295, 18);
            this.status_box_PD.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(23, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(332, 308);
            this.label1.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "Pocket Detector";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(396, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 18);
            this.label3.TabIndex = 17;
            this.label3.Text = "Smart Cue";
            // 
            // status_box_SC
            // 
            this.status_box_SC.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.status_box_SC.Location = new System.Drawing.Point(399, 311);
            this.status_box_SC.Name = "status_box_SC";
            this.status_box_SC.ReadOnly = true;
            this.status_box_SC.Size = new System.Drawing.Size(295, 18);
            this.status_box_SC.TabIndex = 15;
            // 
            // comport_box_SC
            // 
            this.comport_box_SC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comport_box_SC.FormattingEnabled = true;
            this.comport_box_SC.Items.AddRange(new object[] {
            "test1",
            "test2",
            "test3"});
            this.comport_box_SC.Location = new System.Drawing.Point(399, 75);
            this.comport_box_SC.Name = "comport_box_SC";
            this.comport_box_SC.Size = new System.Drawing.Size(295, 26);
            this.comport_box_SC.TabIndex = 14;
            this.comport_box_SC.SelectedIndexChanged += new System.EventHandler(this.comport_box_SC_SelectedIndexChanged);
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox4.Location = new System.Drawing.Point(399, 121);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(267, 18);
            this.textBox4.TabIndex = 13;
            this.textBox4.Text = "Message log";
            // 
            // textBox5
            // 
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox5.Location = new System.Drawing.Point(399, 51);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(267, 18);
            this.textBox5.TabIndex = 12;
            this.textBox5.Text = "Select COM port";
            // 
            // message_box_SC
            // 
            this.message_box_SC.Location = new System.Drawing.Point(399, 145);
            this.message_box_SC.Multiline = true;
            this.message_box_SC.Name = "message_box_SC";
            this.message_box_SC.ReadOnly = true;
            this.message_box_SC.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.message_box_SC.Size = new System.Drawing.Size(295, 160);
            this.message_box_SC.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(378, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(332, 308);
            this.label4.TabIndex = 16;
            // 
            // smartcue_worker
            // 
            this.smartcue_worker.WorkerSupportsCancellation = true;
            this.smartcue_worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Smartcue_worker_DoWork);
            // 
            // status_box_all
            // 
            this.status_box_all.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.status_box_all.Location = new System.Drawing.Point(23, 355);
            this.status_box_all.Name = "status_box_all";
            this.status_box_all.ReadOnly = true;
            this.status_box_all.Size = new System.Drawing.Size(687, 18);
            this.status_box_all.TabIndex = 18;
            // 
            // comport_scan_worker
            // 
            this.comport_scan_worker.WorkerSupportsCancellation = true;
            this.comport_scan_worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.comport_scan_worker_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(731, 385);
            this.Controls.Add(this.status_box_all);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.status_box_SC);
            this.Controls.Add(this.comport_box_SC);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.message_box_SC);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.status_box_PD);
            this.Controls.Add(this.comport_box_PD);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.message_box_PD);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Openpool boost interface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker pocketdetector_worker;
        private System.Windows.Forms.TextBox message_box_PD;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ComboBox comport_box_PD;
        private System.Windows.Forms.TextBox status_box_PD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox status_box_SC;
        private System.Windows.Forms.ComboBox comport_box_SC;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox message_box_SC;
        private System.Windows.Forms.Label label4;
        private System.ComponentModel.BackgroundWorker smartcue_worker;
        private System.Windows.Forms.TextBox status_box_all;
        private System.ComponentModel.BackgroundWorker comport_scan_worker;
    }
}

