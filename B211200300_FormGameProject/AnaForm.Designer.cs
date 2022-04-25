namespace B211200300_FormGameProject
{
    partial class AnaForm
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
            this.bilgiPanel = new System.Windows.Forms.Panel();
            this.oyunPanel = new System.Windows.Forms.Panel();
            this.kalansureLabel = new System.Windows.Forms.Label();
            this.kalansure = new System.Windows.Forms.Label();
            this.bilgiPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // bilgiPanel
            // 
            this.bilgiPanel.BackColor = System.Drawing.SystemColors.GrayText;
            this.bilgiPanel.Controls.Add(this.kalansure);
            this.bilgiPanel.Controls.Add(this.kalansureLabel);
            this.bilgiPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.bilgiPanel.Location = new System.Drawing.Point(843, 0);
            this.bilgiPanel.Name = "bilgiPanel";
            this.bilgiPanel.Size = new System.Drawing.Size(219, 712);
            this.bilgiPanel.TabIndex = 0;
            // 
            // oyunPanel
            // 
            this.oyunPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.oyunPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oyunPanel.Location = new System.Drawing.Point(0, 0);
            this.oyunPanel.Name = "oyunPanel";
            this.oyunPanel.Size = new System.Drawing.Size(843, 712);
            this.oyunPanel.TabIndex = 1;
            // 
            // kalansureLabel
            // 
            this.kalansureLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kalansureLabel.AutoSize = true;
            this.kalansureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kalansureLabel.Location = new System.Drawing.Point(35, 34);
            this.kalansureLabel.Name = "kalansureLabel";
            this.kalansureLabel.Size = new System.Drawing.Size(172, 36);
            this.kalansureLabel.TabIndex = 0;
            this.kalansureLabel.Text = "Kalan Süre";
            // 
            // kalansure
            // 
            this.kalansure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kalansure.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.kalansure.Location = new System.Drawing.Point(49, 79);
            this.kalansure.Name = "kalansure";
            this.kalansure.Size = new System.Drawing.Size(114, 55);
            this.kalansure.TabIndex = 1;
            this.kalansure.Text = "000";
            this.kalansure.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AnaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 712);
            this.Controls.Add(this.oyunPanel);
            this.Controls.Add(this.bilgiPanel);
            this.Name = "AnaForm";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.bilgiPanel.ResumeLayout(false);
            this.bilgiPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel bilgiPanel;
        private System.Windows.Forms.Label kalansure;
        private System.Windows.Forms.Label kalansureLabel;
        private System.Windows.Forms.Panel oyunPanel;
    }
}

