using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardTextCounter
{
    //Code based on http://alanbondo.wordpress.com/2008/06/22/creating-a-system-tray-app-with-c/
    public class CounterIcon : Form
    {
        private readonly NotifyIcon notifyIcon = new NotifyIcon();
        private readonly ContextMenu menu = new ContextMenu();

        private GlobalHotkey hotKey;

        public CounterIcon()
        {
            menu.MenuItems.Add("Count", delegate { CountText(); });
            menu.MenuItems.Add("-");
            menu.MenuItems.Add("About", delegate { System.Diagnostics.Process.Start(@"https://github.com/andrewducker/ClipboardTextCounter"); });
            menu.MenuItems.Add("Exit", delegate { Application.Exit(); });
            notifyIcon.Text = "Clipboard Text Counter";
            notifyIcon.Icon = new Icon(SystemIcons.Question, 40, 40);
            notifyIcon.ContextMenu = menu;
            notifyIcon.Click += NotifyIconClick;
            notifyIcon.Visible = true;
        }

        static void NotifyIconClick(object sender, EventArgs e)
        {
            CountText();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == GlobalHotkey.WM_HOTKEY_MSG_ID)
            {
                CountText();
            }
            base.WndProc(ref m);
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            //This must be set under ShowInTaskbar, as that changes the hWnd of the form.
            hotKey = new GlobalHotkey(GlobalHotkey.WIN, Keys.C, this);
            base.OnLoad(e);
        }

        private static void CountText()
        {
            string clipText = Clipboard.ContainsText() ? Clipboard.GetText().Length.ToString() : "Clipboard does not contain text.";
            MessageBox.Show(clipText);
        }

        private bool disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            disposed = true;
            if (disposing)
            {
                notifyIcon.Dispose();
                hotKey.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}