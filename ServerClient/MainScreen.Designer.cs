namespace ServerClient
{
    partial class MainScreen
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
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.roleButton = new System.Windows.Forms.Button();
            this.reportButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ipBox = new System.Windows.Forms.RichTextBox();
            this.portBox = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(12, 106);
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(563, 287);
            this.logBox.TabIndex = 0;
            this.logBox.Text = "";
            // 
            // startButton
            // 
            this.startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.startButton.Location = new System.Drawing.Point(12, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(95, 31);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.stopButton.Location = new System.Drawing.Point(12, 49);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(95, 31);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // roleButton
            // 
            this.roleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.roleButton.Location = new System.Drawing.Point(480, 12);
            this.roleButton.Name = "roleButton";
            this.roleButton.Size = new System.Drawing.Size(95, 30);
            this.roleButton.TabIndex = 3;
            this.roleButton.Text = "Give a role";
            this.roleButton.UseVisualStyleBackColor = true;
            this.roleButton.Click += new System.EventHandler(this.roleButton_Click);
            // 
            // reportButton
            // 
            this.reportButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.reportButton.Location = new System.Drawing.Point(480, 49);
            this.reportButton.Name = "reportButton";
            this.reportButton.Size = new System.Drawing.Size(95, 31);
            this.reportButton.TabIndex = 4;
            this.reportButton.Text = "Reports";
            this.reportButton.UseVisualStyleBackColor = true;
            this.reportButton.Click += new System.EventHandler(this.reportButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(12, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Logs:";
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(310, 17);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(164, 26);
            this.ipBox.TabIndex = 6;
            this.ipBox.Text = "";
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(376, 55);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(98, 26);
            this.portBox.TabIndex = 7;
            this.portBox.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(276, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(328, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Port:";
            // 
            // MainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 405);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.ipBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reportButton);
            this.Controls.Add(this.roleButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.logBox);
            this.Name = "MainScreen";
            this.Text = "MainScreen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox logBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button roleButton;
        private System.Windows.Forms.Button reportButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox ipBox;
        private System.Windows.Forms.RichTextBox portBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}