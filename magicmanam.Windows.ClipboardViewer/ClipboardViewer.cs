using System;

namespace magicmanam.Windows.ClipboardViewer
{
    public class ClipboardViewer
    {
        private IntPtr _handle;
        private IntPtr _nextViewer;

        public ClipboardViewer(IntPtr handle)
        {
            this._handle = handle;
            this._nextViewer = WindowsFunctions.SetClipboardViewer(this._handle);
        }

        /// <summary>
        /// Clipboard updated event.
        /// </summary>
        public event EventHandler ClipboardUpdated;

        /// <summary>
        /// Handles clipboard-related Windows events. 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        public void HandleWindowsMessage(int msg, IntPtr wParam, IntPtr lParam)
        {
            switch(msg)
            {
                case Messages.WM_DRAWCLIPBOARD:
                    if (this._nextViewer != IntPtr.Zero)
                    {
                        WindowsFunctions.SendMessage(this._nextViewer, msg, IntPtr.Zero, IntPtr.Zero);
                    }
                    ClipboardUpdated?.Invoke(this, EventArgs.Empty);
                    break;
                case Messages.WM_DESTROY:
                    WindowsFunctions.ChangeClipboardChain(this._handle, this._nextViewer);
                    break;
                case Messages.WM_CHANGECBCHAIN:
                    if (this._nextViewer == wParam)
                    {
                        this._nextViewer = lParam;
                    }
                    else
                    {
                        WindowsFunctions.SendMessage(this._nextViewer, msg, wParam, lParam);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Call this method to recreate you clipboard viewer in the clipboard chain.
        /// That is need because not all viewers work correctly with clipboard chain and you need to ensure that your viewer is still in chain.
        /// </summary>
        public void RefreshViewer()
        {
            var currentViewer = WindowsFunctions.GetClipboardViewer();
            
            if (currentViewer != this._handle)
            {
                WindowsFunctions.ChangeClipboardChain(this._handle, this._nextViewer);
                this._nextViewer = WindowsFunctions.SetClipboardViewer(this._handle);
            }
        }
    }
}
