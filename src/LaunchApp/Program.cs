using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Main;
using DBManager;
using System.Windows.Forms.Integration;
using Microsoft.Win32;

namespace LaunchApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BackuperContext());
        }
    }

    class BackuperContext : ApplicationContext
    {
        MainWindow mw = new MainWindow();
        NotifyIcon ni = new NotifyIcon();

        public BackuperContext()
        {
            AddToAutostart();

            PlanHandler.HandleAllPlans();

            System.Drawing.Image imageBackuper = LaunchApp.Properties.Resources.logo;
            System.Drawing.Image imageExit = LaunchApp.Properties.Resources.exit;

            ToolStripItem showApp = new ToolStripMenuItem("Backuper", imageBackuper, new EventHandler(ShowApp));
            ToolStripMenuItem exitApp = new ToolStripMenuItem("Exit", imageExit, new EventHandler(ExitApp));

            ni.Icon = LaunchApp.Properties.Resources.appIcon;
            ni.DoubleClick += new EventHandler(ShowApp);

            ni.ContextMenuStrip = new ContextMenuStrip();
            ni.ContextMenuStrip.Items.AddRange(new ToolStripItem[] { showApp, exitApp });
            ni.Visible = true;
        }

        private void AddToAutostart()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rk.GetValue("Backuper") == null)
                rk.SetValue("Backuper", Application.ExecutablePath);
        }

        private void ShowApp(object sender, EventArgs e)
        {
            mw.InitializeComponent();

            mw.Closing += mw_ClosingEventHandler;

            if (mw.IsVisible == true)
                mw.Activate();
            else
            {
                ElementHost.EnableModelessKeyboardInterop(mw);
                mw.Show();
            }
        }

        private void ExitApp(object sender, EventArgs e)
        {
            ni.Visible = false;
            System.Windows.Forms.Application.Exit();
        }

        private void mw_ClosingEventHandler(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            mw.Hide();
        }
    }
}
