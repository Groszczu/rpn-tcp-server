using System.ComponentModel;

namespace Client
{
    partial class AdminApprovalScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.applicationListBox = new System.Windows.Forms.CheckedListBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.declineButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // applicationListBox
            // 
            this.applicationListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (238)));
            this.applicationListBox.FormattingEnabled = true;
            this.applicationListBox.Location = new System.Drawing.Point(12, 12);
            this.applicationListBox.Name = "applicationListBox";
            this.applicationListBox.Size = new System.Drawing.Size(776, 361);
            this.applicationListBox.TabIndex = 0;
            // 
            // acceptButton
            // 
            this.acceptButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (238)));
            this.acceptButton.Location = new System.Drawing.Point(35, 382);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(232, 56);
            this.acceptButton.TabIndex = 1;
            this.acceptButton.Text = "Accept selected applications";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // declineButton
            // 
            this.declineButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (238)));
            this.declineButton.Location = new System.Drawing.Point(529, 382);
            this.declineButton.Name = "declineButton";
            this.declineButton.Size = new System.Drawing.Size(232, 56);
            this.declineButton.TabIndex = 2;
            this.declineButton.Text = "Decline selected applications";
            this.declineButton.UseVisualStyleBackColor = true;
            this.declineButton.Click += new System.EventHandler(this.declineButton_Click);
            // 
            // AdminApprovalScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.declineButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.applicationListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "AdminApprovalScreen";
            this.Text = "Review issued admin applications";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button declineButton;

        private System.Windows.Forms.CheckedListBox applicationListBox;

        #endregion
    }
}