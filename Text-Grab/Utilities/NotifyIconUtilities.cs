using System;
using System.Windows.Forms;
using Text_Grab.Views;

namespace Text_Grab.Utilities
{
    public static class NotifyIconUtilities
    {
        public static void SetupNotifyIcon()
        {
            App app = (App)App.Current;
            if (app.TextGrabIcon != null
                || app.NumberOfRunningInstances > 1)
            {
                return;
            }

            NotifyIcon icon = new NotifyIcon();
            icon.Text = "Text Grab";
            icon.Icon = new System.Drawing.Icon(System.Windows.Application.GetResourceStream(new Uri("/t_ICON2.ico", UriKind.Relative)).Stream);
            icon.Visible = true;

            ContextMenuStrip? contextMenu = new();

            ToolStripMenuItem? settingsItem = new("&Settings");
            settingsItem.Click += (s, e) => { SettingsWindow sw = new(); sw.Show(); };
            ToolStripMenuItem? fullscreenGrabItem = new("&Fullscreen Grab");
            fullscreenGrabItem.Click += (s, e) => { WindowUtilities.LaunchFullScreenGrab(true); };
            ToolStripMenuItem? grabFrameItem = new("&Grab Frame");
            grabFrameItem.Click += (s, e) => { GrabFrame gf = new(); gf.Show(); };
            ToolStripMenuItem? editTextWindowItem = new("&Edit Text Window");
            editTextWindowItem.Click += (s, e) => { EditTextWindow etw = new(); etw.Show(); };

            ToolStripMenuItem? exitItem = new("&Close");
            exitItem.Click += (s, e) => { icon.Dispose(); System.Windows.Application.Current.Shutdown(); };

            contextMenu.Items.AddRange(
                new ToolStripMenuItem[] {
                    settingsItem,
                    fullscreenGrabItem,
                    grabFrameItem,
                    editTextWindowItem,
                    exitItem
                }
            );
            icon.ContextMenuStrip = contextMenu;

            icon.MouseClick += (s, e) =>
            {
                // TODO Add a setting to customize click behavior
                if (e.Button == MouseButtons.Left)
                    WindowUtilities.LaunchFullScreenGrab(true);
            };

            // Double click just triggers the single click
            // icon.DoubleClick += (s, e) =>
            // {
            //     // TODO Add a setting to customize doubleclick behavior
            //     EditTextWindow etw = new(); etw.Show();
            // };

            HotKeyManager.RegisterHotKey(Keys.F, KeyModifiers.Windows | KeyModifiers.Shift);
            HotKeyManager.RegisterHotKey(Keys.E, KeyModifiers.Windows | KeyModifiers.Shift);
            HotKeyManager.RegisterHotKey(Keys.G, KeyModifiers.Windows | KeyModifiers.Shift);
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);

            app.TextGrabIcon = icon;
        }


        static void HotKeyManager_HotKeyPressed(object? sender, HotKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.E:
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => { EditTextWindow etw = new(); etw.Show(); }));
                    break;
                case Keys.F:
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => { WindowUtilities.LaunchFullScreenGrab(true); }));
                    break;
                case Keys.G:
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => { GrabFrame gf = new(); gf.Show(); }));
                    break;
                default:
                    break;
            }
        }
    }
}