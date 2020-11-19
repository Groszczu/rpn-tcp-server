namespace Client
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
            this.rpnTextBox = new System.Windows.Forms.RichTextBox();
            this.rpnLabel = new System.Windows.Forms.Label();
            this.calculateButton = new System.Windows.Forms.Button();
            this.reportTextBox = new System.Windows.Forms.TextBox();
            this.reportLabel = new System.Windows.Forms.Label();
            this.sendReportButton = new System.Windows.Forms.Button();
            this.rpnResultTextBox = new System.Windows.Forms.TextBox();
            this.equalityLabel = new System.Windows.Forms.Label();
            this.logOutButton = new System.Windows.Forms.Button();
            this.historyButton = new System.Windows.Forms.Button();
            this.reportViewButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rpnTextBox
            // 
            this.rpnTextBox.Location = new System.Drawing.Point(33, 59);
            this.rpnTextBox.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.rpnTextBox.Multiline = false;
            this.rpnTextBox.Name = "rpnTextBox";
            this.rpnTextBox.Size = new System.Drawing.Size(489, 28);
            this.rpnTextBox.TabIndex = 0;
            this.rpnTextBox.Text = "";
            this.rpnTextBox.TextChanged += new System.EventHandler(this.rpnTextBox_TextChanged);
            // 
            // rpnLabel
            // 
            this.rpnLabel.AutoSize = true;
            this.rpnLabel.Location = new System.Drawing.Point(29, 36);
            this.rpnLabel.Name = "rpnLabel";
            this.rpnLabel.Size = new System.Drawing.Size(298, 19);
            this.rpnLabel.TabIndex = 1;
            this.rpnLabel.Text = "Enter RPN expression to calculate";
            // 
            // calculateButton
            // 
            this.calculateButton.Location = new System.Drawing.Point(33, 94);
            this.calculateButton.Name = "calculateButton";
            this.calculateButton.Size = new System.Drawing.Size(333, 32);
            this.calculateButton.TabIndex = 2;
            this.calculateButton.Text = "Calculate";
            this.calculateButton.UseVisualStyleBackColor = true;
            this.calculateButton.Click += new System.EventHandler(this.calculateButton_Click);
            // 
            // reportTextBox
            // 
            this.reportTextBox.Location = new System.Drawing.Point(406, 154);
            this.reportTextBox.Multiline = true;
            this.reportTextBox.Name = "reportTextBox";
            this.reportTextBox.Size = new System.Drawing.Size(419, 218);
            this.reportTextBox.TabIndex = 3;
            // 
            // reportLabel
            // 
            this.reportLabel.AutoSize = true;
            this.reportLabel.Location = new System.Drawing.Point(402, 132);
            this.reportLabel.Name = "reportLabel";
            this.reportLabel.Size = new System.Drawing.Size(120, 19);
            this.reportLabel.TabIndex = 4;
            this.reportLabel.Text = "Report a bug";
            // 
            // sendReportButton
            // 
            this.sendReportButton.Location = new System.Drawing.Point(406, 378);
            this.sendReportButton.Name = "sendReportButton";
            this.sendReportButton.Size = new System.Drawing.Size(188, 32);
            this.sendReportButton.TabIndex = 5;
            this.sendReportButton.Text = "Report";
            this.sendReportButton.UseVisualStyleBackColor = true;
            // 
            // rpnResultTextBox
            // 
            this.rpnResultTextBox.Location = new System.Drawing.Point(563, 59);
            this.rpnResultTextBox.Name = "rpnResultTextBox";
            this.rpnResultTextBox.ReadOnly = true;
            this.rpnResultTextBox.Size = new System.Drawing.Size(262, 26);
            this.rpnResultTextBox.TabIndex = 6;
            // 
            // equalityLabel
            // 
            this.equalityLabel.AutoSize = true;
            this.equalityLabel.Location = new System.Drawing.Point(531, 62);
            this.equalityLabel.Name = "equalityLabel";
            this.equalityLabel.Size = new System.Drawing.Size(22, 19);
            this.equalityLabel.TabIndex = 7;
            this.equalityLabel.Text = "=";
            // 
            // logOutButton
            // 
            this.logOutButton.Location = new System.Drawing.Point(33, 367);
            this.logOutButton.Name = "logOutButton";
            this.logOutButton.Size = new System.Drawing.Size(333, 43);
            this.logOutButton.TabIndex = 8;
            this.logOutButton.Text = "Log out";
            this.logOutButton.UseVisualStyleBackColor = true;
            this.logOutButton.Click += new System.EventHandler(this.logOutButton_Click);
            // 
            // historyButton
            // 
            this.historyButton.Location = new System.Drawing.Point(33, 154);
            this.historyButton.Name = "historyButton";
            this.historyButton.Size = new System.Drawing.Size(333, 43);
            this.historyButton.TabIndex = 9;
            this.historyButton.Text = "Show my calculation history";
            this.historyButton.UseVisualStyleBackColor = true;
            this.historyButton.Click += new System.EventHandler(this.historyButton_Click);
            // 
            // reportViewButton
            // 
            this.reportViewButton.Location = new System.Drawing.Point(33, 203);
            this.reportViewButton.Name = "reportViewButton";
            this.reportViewButton.Size = new System.Drawing.Size(333, 43);
            this.reportViewButton.TabIndex = 10;
            this.reportViewButton.Text = "View bug reports";
            this.reportViewButton.UseVisualStyleBackColor = true;
            this.reportViewButton.Visible = false;
            // 
            // MainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 453);
            this.Controls.Add(this.reportViewButton);
            this.Controls.Add(this.historyButton);
            this.Controls.Add(this.logOutButton);
            this.Controls.Add(this.equalityLabel);
            this.Controls.Add(this.rpnResultTextBox);
            this.Controls.Add(this.sendReportButton);
            this.Controls.Add(this.reportLabel);
            this.Controls.Add(this.reportTextBox);
            this.Controls.Add(this.calculateButton);
            this.Controls.Add(this.rpnLabel);
            this.Controls.Add(this.rpnTextBox);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("DejaVu Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.MaximizeBox = false;
            this.Name = "MainScreen";
            this.Text = "RPN Calculator - Logged in as";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

        }

        #endregion

        private System.Windows.Forms.RichTextBox rpnTextBox;
        private System.Windows.Forms.Label rpnLabel;
        private System.Windows.Forms.Button calculateButton;
        private System.Windows.Forms.TextBox reportTextBox;
        private System.Windows.Forms.Label reportLabel;
        private System.Windows.Forms.Button sendReportButton;
        private System.Windows.Forms.TextBox rpnResultTextBox;
        private System.Windows.Forms.Label equalityLabel;
        private System.Windows.Forms.Button logOutButton;
        private System.Windows.Forms.Button historyButton;
        private System.Windows.Forms.Button reportViewButton;
    }
}