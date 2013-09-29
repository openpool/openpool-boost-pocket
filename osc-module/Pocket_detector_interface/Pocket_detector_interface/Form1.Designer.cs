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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.output_box = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.comport_box = new System.Windows.Forms.ComboBox();
            this.status_textbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // output_box
            // 
            this.output_box.Location = new System.Drawing.Point(33, 115);
            this.output_box.Multiline = true;
            this.output_box.Name = "output_box";
            this.output_box.ReadOnly = true;
            this.output_box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.output_box.Size = new System.Drawing.Size(392, 160);
            this.output_box.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1.Location = new System.Drawing.Point(33, 21);
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
            this.textBox2.Location = new System.Drawing.Point(33, 91);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(267, 18);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "Message log";
            // 
            // comport_box
            // 
            this.comport_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comport_box.FormattingEnabled = true;
            this.comport_box.Items.AddRange(new object[] {
            "No COM port"});
            this.comport_box.Location = new System.Drawing.Point(33, 45);
            this.comport_box.Name = "comport_box";
            this.comport_box.Size = new System.Drawing.Size(392, 26);
            this.comport_box.TabIndex = 7;
            this.comport_box.SelectedIndexChanged += new System.EventHandler(this.comport_box_SelectedIndexChanged);
            // 
            // status_textbox
            // 
            this.status_textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.status_textbox.Location = new System.Drawing.Point(12, 318);
            this.status_textbox.Name = "status_textbox";
            this.status_textbox.ReadOnly = true;
            this.status_textbox.Size = new System.Drawing.Size(427, 18);
            this.status_textbox.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(451, 337);
            this.Controls.Add(this.status_textbox);
            this.Controls.Add(this.comport_box);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.output_box);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox output_box;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ComboBox comport_box;
        private System.Windows.Forms.TextBox status_textbox;
    }
}

