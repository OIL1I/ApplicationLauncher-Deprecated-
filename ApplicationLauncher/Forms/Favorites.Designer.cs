﻿namespace ApplicationLauncher.Forms
{
    partial class Favorites
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
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.flpannel_items = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "App Launcher";
            this.notifyIcon1.Visible = true;
            // 
            // flpannel_items
            // 
            this.flpannel_items.Location = new System.Drawing.Point(13, 12);
            this.flpannel_items.Name = "flpannel_items";
            this.flpannel_items.Size = new System.Drawing.Size(425, 126);
            this.flpannel_items.TabIndex = 1;
            // 
            // Favorites
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 150);
            this.Controls.Add(this.flpannel_items);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Favorites";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Favorites";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Favorites_FormClosing);
            this.Load += new System.EventHandler(this.Favorites_Load);
            this.Move += new System.EventHandler(this.Favorites_Move);
            this.Resize += new System.EventHandler(this.Favorites_Resize);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.FlowLayoutPanel flpannel_items;
    }
}