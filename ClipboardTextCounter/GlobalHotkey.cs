using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClipboardTextCounter
{
    //Code based on http://www.dreamincode.net/forums/topic/180436-global-hotkeys/
    class GlobalHotkey : IDisposable
    {
        //modifiers
        // ReSharper disable UnusedMember.Global
        public const int NOMOD = 0x0000;
        public const int ALT = 0x0001;
        public const int CTRL = 0x0002;
        public const int SHIFT = 0x0004;
        public const int WIN = 0x0008;
        // ReSharper restore UnusedMember.Global

        //windows message id for hotkey
        public const int WM_HOTKEY_MSG_ID = 0x0312;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("kernel32.dll")]
        private static extern short GlobalAddAtom(string atomName);
        [DllImport("kernel32.dll")]
        private static extern short GlobalDeleteAtom(short atom);

        private readonly IntPtr hWnd;
        private readonly short id;

        public GlobalHotkey(int modifier, Keys keys, IWin32Window form)
        {
            hWnd = form.Handle;
            id = GlobalAddAtom("Hotkey" + modifier + keys + form.Handle);
            RegisterHotKey(form.Handle, id, modifier, (int)keys);
        }

        private bool disposed;
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                GlobalDeleteAtom(id);
                UnregisterHotKey(hWnd, id);
            }
        }

        ~GlobalHotkey()
        {
            Dispose();
        }
    }
}
