namespace WindowsFormsApp1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gISAIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFastaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePrimersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkFeaturesInStringsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkFeaturesInStringsWithNsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getSeqyunMutationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkFeaturesInStringsALLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFAssEAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gECCOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gECOO2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gISAIDToolStripMenuItem,
            this.openFAssEAToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(312, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gISAIDToolStripMenuItem
            // 
            this.gISAIDToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFastaToolStripMenuItem,
            this.changePrimersToolStripMenuItem,
            this.checkFeaturesInStringsToolStripMenuItem,
            this.checkFeaturesInStringsWithNsToolStripMenuItem,
            this.getSeqyunMutationsToolStripMenuItem,
            this.checkFeaturesInStringsALLToolStripMenuItem});
            this.gISAIDToolStripMenuItem.Name = "gISAIDToolStripMenuItem";
            this.gISAIDToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.gISAIDToolStripMenuItem.Text = "GISAID";
            // 
            // openFastaToolStripMenuItem
            // 
            this.openFastaToolStripMenuItem.Name = "openFastaToolStripMenuItem";
            this.openFastaToolStripMenuItem.Size = new System.Drawing.Size(308, 26);
            this.openFastaToolStripMenuItem.Text = "OpenFasta";
            this.openFastaToolStripMenuItem.Click += new System.EventHandler(this.OpenFastaToolStripMenuItem_Click);
            // 
            // changePrimersToolStripMenuItem
            // 
            this.changePrimersToolStripMenuItem.Name = "changePrimersToolStripMenuItem";
            this.changePrimersToolStripMenuItem.Size = new System.Drawing.Size(308, 26);
            this.changePrimersToolStripMenuItem.Text = "ChangePrimers";
            this.changePrimersToolStripMenuItem.Click += new System.EventHandler(this.ChangePrimersToolStripMenuItem_Click);
            // 
            // checkFeaturesInStringsToolStripMenuItem
            // 
            this.checkFeaturesInStringsToolStripMenuItem.Name = "checkFeaturesInStringsToolStripMenuItem";
            this.checkFeaturesInStringsToolStripMenuItem.Size = new System.Drawing.Size(308, 26);
            this.checkFeaturesInStringsToolStripMenuItem.Text = "Check Features in Strings";
            this.checkFeaturesInStringsToolStripMenuItem.Click += new System.EventHandler(this.CheckFeaturesInStringsToolStripMenuItem_Click);
            // 
            // checkFeaturesInStringsWithNsToolStripMenuItem
            // 
            this.checkFeaturesInStringsWithNsToolStripMenuItem.Name = "checkFeaturesInStringsWithNsToolStripMenuItem";
            this.checkFeaturesInStringsWithNsToolStripMenuItem.Size = new System.Drawing.Size(308, 26);
            this.checkFeaturesInStringsWithNsToolStripMenuItem.Text = "Check Features in Strings with Ns";
            this.checkFeaturesInStringsWithNsToolStripMenuItem.Click += new System.EventHandler(this.CheckFeaturesInStringsWithNsToolStripMenuItem_Click);
            // 
            // getSeqyunMutationsToolStripMenuItem
            // 
            this.getSeqyunMutationsToolStripMenuItem.Name = "getSeqyunMutationsToolStripMenuItem";
            this.getSeqyunMutationsToolStripMenuItem.Size = new System.Drawing.Size(308, 26);
            this.getSeqyunMutationsToolStripMenuItem.Text = "Get seqMutations";
            this.getSeqyunMutationsToolStripMenuItem.Click += new System.EventHandler(this.GetSeqyunMutationsToolStripMenuItem_Click);
            // 
            // checkFeaturesInStringsALLToolStripMenuItem
            // 
            this.checkFeaturesInStringsALLToolStripMenuItem.Name = "checkFeaturesInStringsALLToolStripMenuItem";
            this.checkFeaturesInStringsALLToolStripMenuItem.Size = new System.Drawing.Size(308, 26);
            this.checkFeaturesInStringsALLToolStripMenuItem.Text = "Check Features in Strings ALL";
            this.checkFeaturesInStringsALLToolStripMenuItem.Click += new System.EventHandler(this.CheckFeaturesInStringsALLToolStripMenuItem_Click);
            // 
            // openFAssEAToolStripMenuItem
            // 
            this.openFAssEAToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gECCOToolStripMenuItem,
            this.gECOO2ToolStripMenuItem});
            this.openFAssEAToolStripMenuItem.Name = "openFAssEAToolStripMenuItem";
            this.openFAssEAToolStripMenuItem.Size = new System.Drawing.Size(41, 24);
            this.openFAssEAToolStripMenuItem.Text = "EA";
            // 
            // gECCOToolStripMenuItem
            // 
            this.gECCOToolStripMenuItem.Name = "gECCOToolStripMenuItem";
            this.gECCOToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.gECCOToolStripMenuItem.Text = "GECCO";
            this.gECCOToolStripMenuItem.Click += new System.EventHandler(this.GECCOToolStripMenuItem_Click);
            // 
            // gECOO2ToolStripMenuItem
            // 
            this.gECOO2ToolStripMenuItem.Name = "gECOO2ToolStripMenuItem";
            this.gECOO2ToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.gECOO2ToolStripMenuItem.Text = "GECOO 2 Restricted Section";
            this.gECOO2ToolStripMenuItem.Click += new System.EventHandler(this.GECOO2ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 228);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gISAIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFastaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePrimersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkFeaturesInStringsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem checkFeaturesInStringsWithNsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFAssEAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getSeqyunMutationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gECCOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gECOO2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkFeaturesInStringsALLToolStripMenuItem;
    }
}

