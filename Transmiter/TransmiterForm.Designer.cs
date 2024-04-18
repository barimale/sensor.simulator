namespace Transmiter
{
    partial class TransmiterForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            sTARTToolStripMenuItem = new ToolStripMenuItem();
            sTOPToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { sTARTToolStripMenuItem, sTOPToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // sTARTToolStripMenuItem
            // 
            sTARTToolStripMenuItem.Name = "sTARTToolStripMenuItem";
            sTARTToolStripMenuItem.Size = new Size(64, 24);
            sTARTToolStripMenuItem.Text = "START";
            sTARTToolStripMenuItem.Click += sTARTToolStripMenuItem_Click;
            // 
            // sTOPToolStripMenuItem
            // 
            sTOPToolStripMenuItem.Name = "sTOPToolStripMenuItem";
            sTOPToolStripMenuItem.Size = new Size(57, 24);
            sTOPToolStripMenuItem.Text = "STOP";
            sTOPToolStripMenuItem.Click += sTOPToolStripMenuItem_Click;
            // 
            // TransmiterForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "TransmiterForm";
            Text = "Form1";
            Load += TransmiterForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem sTARTToolStripMenuItem;
        private ToolStripMenuItem sTOPToolStripMenuItem;
    }
}
