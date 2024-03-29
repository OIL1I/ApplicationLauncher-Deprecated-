﻿using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using ApplicationLauncher.Data;
using ApplicationLauncher.Logic;
using SaveItem = SaveHelper.SaveItem;

namespace ApplicationLauncher.Forms
{
    public partial class Favorites : Form
    {
        Configuration config;
        Rectangle screen;
        bool shouldClose = false;

        public Favorites(string arg)
        {
            config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            if (arg == "Autostart" && config.AppSettings.Settings["Autostart"].Value == "true" || arg == "Start")
                InitializeComponent();
            else
            {
                shouldClose = true;
                this.Close();
            }
        }

        private void Favorites_Load(object sender, EventArgs e)
        {
            screen = Screen.PrimaryScreen.WorkingArea;

            notifyIcon1.Icon = SystemIcons.Application;

            notifyIcon1.ContextMenuStrip = CreateContextMenuStrip();
            notifyIcon1.DoubleClick += ShowFavoriteWindow_Click;

            try
            {
                if (config.AppSettings.Settings["FirstLaunch"].Value == "true")
                {
                    using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                    {
                        var processInfo = new System.Diagnostics.ProcessStartInfo(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Binaries\\Setup.exe");
                        process.StartInfo = processInfo;
                        process.Start();
                    }

                    shouldClose = true;
                    Application.Exit();
                    //SaveManager.SetPathsToDefault();
                    //config.AppSettings.Settings["CurrentSavePath"].Value = SaveManager.CurrentSavePath;
                    //notifyIcon1.BalloonTipText = "First launch: .config setup complete";
                    //notifyIcon1.ShowBalloonTip(150);
                    //this.Location = new Point(screen.Width - this.Width, screen.Height - this.Height);
                }
                else
                {
                    SaveManager.CurrentSavePath = config.AppSettings.Settings["CurrentSavePath"].Value;
                    SaveManager.DefaultSavePath = config.AppSettings.Settings["DefaultSavePath"].Value;

                    int width;
                    int height;
                    int posX;
                    int posY;
                    Int32.TryParse(config.AppSettings.Settings["Width"].Value, out width);
                    Int32.TryParse(config.AppSettings.Settings["Height"].Value, out height);
                    Int32.TryParse(config.AppSettings.Settings["PositionX"].Value, out posX);
                    Int32.TryParse(config.AppSettings.Settings["PositionY"].Value, out posY);

                    this.Location = new Point(posX, posY);
                    this.Size = new Size(width, height);
                    this.flpannel_items.Size = new Size(width - 41, height - 63);

                    SaveManager.PreviousSavePath = SaveManager.CurrentSavePath;
                }

                SaveItem[] items = SaveManager.LoadFromFiles(SaveManager.CurrentSavePath);

                foreach (SaveItem saveItem in items)
                {
                    LauncherData.AddItem(new Controls.LauncherItem(saveItem));
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "Undefined Savepath!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exceptions.SavePathNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Missing savepath directory!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ReloadFavorites();
        }

        private ContextMenuStrip CreateContextMenuStrip()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            cms.Items.Add("Show favorites");
            cms.Items.Add("Show all registered applications");
            cms.Items.Add("Settings");
            cms.Items.Add("Close application");

            cms.Items[0].Click += ShowFavoriteWindow_Click;
            cms.Items[1].Click += ShowAllApllications_Click;
            cms.Items[2].Click += ShowSettingsWindow_Click;
            cms.Items[3].Click += QuitApplication_Click;

            return cms;
        }

        private void ShowAllApllications_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < LauncherData.ItemCount; i++)
            {
                var item = LauncherData.GetItem(i);

                item.RemoveClickToLaunch();
            }

            this.Visible = false;
            new AllApps().ShowDialog();
            this.Visible = true;

            ReloadFavorites();
        }

        private void QuitApplication_Click(object sender, EventArgs e)
        {
        save:
            try
            {
                for (int i = 0; i < LauncherData.ItemCount; i++)
                {
                    SaveManager.SaveToFile(LauncherData.GetItem(i).GetSaveItem());
                }

                config.AppSettings.Settings["CurrentSavePath"].Value = SaveManager.CurrentSavePath;
                config.AppSettings.Settings["PositionX"].Value = this.Location.X.ToString();
                config.AppSettings.Settings["PositionY"].Value = this.Location.Y.ToString();
                config.AppSettings.Settings["Width"].Value = this.Size.Width.ToString();
                config.AppSettings.Settings["Height"].Value = this.Size.Height.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                shouldClose = true;
                Application.Exit();
            }
            catch (Exceptions.SavePathWasNotSetException ex)
            {
                var result = MessageBox.Show(ex.Message + Environment.NewLine + "Would you like to set it now?", "Undefined savepath!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (result == DialogResult.Yes)
                {
                    notifyIcon1.BalloonTipText = "Savepath set to default";
                    notifyIcon1.ShowBalloonTip(150);
                    SaveManager.SetPathsToDefault();
                    (new Settings()).ShowDialog();
                }
                return;
            }
            catch (Exceptions.SavePathNotFoundException ex)
            {
                var result = MessageBox.Show(ex.Message + Environment.NewLine + "Would you like to create it?", "Missing savepath directory", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (result == DialogResult.Yes)
                {
                    notifyIcon1.BalloonTipText = "Created directory " + SaveManager.CurrentSavePath;
                    notifyIcon1.ShowBalloonTip(150);
                    SaveManager.CreateDirectory(SaveManager.CurrentSavePath);
                }
                return;
            }
            catch (Exceptions.ChangedSavePathException ex)
            {
                var result = MessageBox.Show(ex.Message + Environment.NewLine + "Are you sure ?", "Changed savepath", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                if (result == DialogResult.Yes)
                {
                    SaveManager.RemoveCurrentSaveFiles();
                    notifyIcon1.BalloonTipText = "Deleted all savefiles";
                    notifyIcon1.ShowBalloonTip(150);
                    SaveManager.PreviousSavePath = SaveManager.CurrentSavePath;
                    goto save;
                }
                notifyIcon1.BalloonTipText = "Savepath set to default";
                notifyIcon1.ShowBalloonTip(150);
                SaveManager.SetPathsToDefault();
            }
        }

        private void ShowSettingsWindow_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            new Settings().ShowDialog();
            ReloadFavorites();
            this.Visible = true;
        }

        public void ShowFavoriteWindow_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void btn_hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void ReloadFavorites()
        {
            foreach (Controls.LauncherItem item in flpannel_items.Controls)
            {
                item.RemoveClickToLaunch();
            }
            foreach (Controls.FolderItem item in flpannel_items.Controls)
            {
                item.RemoveClickToOpen();
            }

            flpannel_items.Controls.Clear();

            for (int i = 0; i < Data.LauncherData.ItemCount; i++)
            {
                if ((Data.LauncherData.GetItem(i)).IsFavorite)
                {
                    Controls.LauncherItem tI = Data.LauncherData.GetItem(i);
                    flpannel_items.Controls.Add(tI);
                    tI.SetClickToLaunch();
                }

            }
            for (int i = 0; i < Data.LauncherData.FolderCount; i++)
            {
                var tF = Data.LauncherData.GetFolder(i);
                flpannel_items.Controls.Add(tF);
                tF.SetClickToOpen();
            }
        }

        private void Favorites_Move(object sender, EventArgs e)
        {
            if (this.Location.X > screen.Width / 2)
                this.Location = new Point(screen.Width - this.Width, this.Location.Y);
            else
                this.Location = new Point(0, this.Location.Y);
        }

        private void Favorites_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!shouldClose)
            {
                this.Visible = false;
                e.Cancel = true;
            }
        }

        private void Favorites_Resize(object sender, EventArgs e)
        {
            this.flpannel_items.Size = new Size(this.Size.Width - 41, this.Size.Height - 63);

            this.flpannel_items.Update();
            this.Update();
        }
    }
}
