using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WindowResizer
{
    class Class1
    {
        #region Dll Import
        protected delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        protected static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        protected static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")]
        protected static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        [DllImport("user32.dll")]
        protected static extern bool IsWindowVisible(IntPtr hWnd);

        //Define API functions.
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPlacement(IntPtr hWnd,
           [In] ref WINDOWPLACEMENT lpwndpl);
        #endregion

        #region Structures
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPLACEMENT
        {
            public int Length;
            public int Flags;
            public ShowWindowCommands ShowCmd;
            public POINT MinPosition;
            public POINT MaxPosition;
            public RECT NormalPosition;
            public static WINDOWPLACEMENT Default
            {
                get
                {
                    WINDOWPLACEMENT result = new WINDOWPLACEMENT();
                    result.Length = Marshal.SizeOf(result);
                    return result;
                }
            }
        }

        internal enum ShowWindowCommands : int
        {
            Hide = 0,
            Normal = 1,
            ShowMinimized = 2,
            Maximize = 3, // is this the right value?
            ShowMaximized = 3,
            ShowNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActive = 7,
            ShowNA = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimize = 11
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int _Left;
            public int _Top;
            public int _Right;
            public int _Bottom;
        }
        #endregion

        static List<string> windows_title; 

        /// <summary>
        /// Get the target window's handle by thir title or part of it
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Minimized"></param>
        /// <param name="Maximized"></param>
        /// <param name="All"></param>
        public static void FindWindow(string title, string position, bool All)
        {
            List<IntPtr> target_hwnds = findwindow(title);

            if (target_hwnds.Count == 0)
            {
                MessageBox.Show("Could not find a window with the title \"" +
                    title + "\"");
                return;
            }

            if (All)
            {
                foreach (IntPtr hwnd in target_hwnds)
                {
                    SetWindowLocation(position, hwnd);
                }
            }
            else SetWindowLocation(position, target_hwnds[0]);
        }


        /// <summary>
        /// Set the target's placement.
        /// </summary>
        /// <param name="Minimized">true if want to Minimized</param>
        /// <param name="Maximized">true if want to Maximized</param>
        /// <param name="hwnd">the target window</param>
        private static void SetWindowLocation(string position, IntPtr hwnd)
        {
            if (hwnd != IntPtr.Zero)
            {
                // Prepare the WINDOWPLACEMENT structure.
                WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
                placement.Length = Marshal.SizeOf(placement);

                // Get the window's current placement.
                GetWindowPlacement(hwnd, out placement);

                placement.ShowCmd = ShowWindowCommands.Normal;

                switch (position.ToLower())
                {
                    case "min":
                        placement.ShowCmd = ShowWindowCommands.ShowMinimized;
                        break;
                    case "max":
                        placement.ShowCmd = ShowWindowCommands.ShowMaximized;
                        break;
                    case "normal":
                        break;
                    case "l":
                        placement.NormalPosition._Left = 0;
                        placement.NormalPosition._Right = Screen.PrimaryScreen.WorkingArea.Width / 2;
                        placement.NormalPosition._Top = 0;
                        placement.NormalPosition._Bottom = Screen.PrimaryScreen.WorkingArea.Height;
                        break;
                    case "r":
                        placement.NormalPosition._Left = Screen.PrimaryScreen.WorkingArea.Width / 2;
                        placement.NormalPosition._Right = Screen.PrimaryScreen.WorkingArea.Width;
                        placement.NormalPosition._Top = 0;
                        placement.NormalPosition._Bottom = Screen.PrimaryScreen.WorkingArea.Height;
                        break;
                    case "t":
                        placement.NormalPosition._Left = 0;
                        placement.NormalPosition._Right = Screen.PrimaryScreen.WorkingArea.Width;
                        placement.NormalPosition._Top = 0;
                        placement.NormalPosition._Bottom = Screen.PrimaryScreen.WorkingArea.Height/2;
                        break;
                    case "b":
                        placement.NormalPosition._Left = 0;
                        placement.NormalPosition._Right = Screen.PrimaryScreen.WorkingArea.Width;
                        placement.NormalPosition._Top = Screen.PrimaryScreen.WorkingArea.Height / 2;
                        placement.NormalPosition._Bottom = Screen.PrimaryScreen.WorkingArea.Height;
                        break;
                    case "tl":
                        placement.NormalPosition._Left = 0;
                        placement.NormalPosition._Right = Screen.PrimaryScreen.WorkingArea.Width/2;
                        placement.NormalPosition._Top = 0;
                        placement.NormalPosition._Bottom = Screen.PrimaryScreen.WorkingArea.Height/2;
                        break;
                    case "bl":
                        placement.NormalPosition._Left = 0;
                        placement.NormalPosition._Right = Screen.PrimaryScreen.WorkingArea.Width / 2;
                        placement.NormalPosition._Top = Screen.PrimaryScreen.WorkingArea.Height / 2;
                        placement.NormalPosition._Bottom = Screen.PrimaryScreen.WorkingArea.Height;
                        break;
                    case "tr":
                        placement.NormalPosition._Left = Screen.PrimaryScreen.WorkingArea.Width / 2;
                        placement.NormalPosition._Right = Screen.PrimaryScreen.WorkingArea.Width;
                        placement.NormalPosition._Top = 0;
                        placement.NormalPosition._Bottom = Screen.PrimaryScreen.WorkingArea.Height/2;
                        break;
                    case "br":
                        placement.NormalPosition._Left = Screen.PrimaryScreen.WorkingArea.Width / 2;
                        placement.NormalPosition._Right = Screen.PrimaryScreen.WorkingArea.Width;
                        placement.NormalPosition._Top = Screen.PrimaryScreen.WorkingArea.Height / 2;
                        placement.NormalPosition._Bottom = Screen.PrimaryScreen.WorkingArea.Height;
                        break;
                    default:    
                        break;
                }
                // Perform the action.
                SetWindowPlacement(hwnd, ref placement);
            }
        }

        private static List<IntPtr> findwindow(string title)
        {     
            //Process[] processlist = Process.GetProcesses();
            List<IntPtr> windows_list = new List<IntPtr>();
            windows_title = new List<string>();
            EnumWindows(new EnumWindowsProc(EnumTheWindows), IntPtr.Zero);

            foreach (string str in windows_title)
            {
                if (str != null && str.ToLower().Contains(title.ToLower()))
                {
                    windows_list.Add(FindWindowByCaption(IntPtr.Zero, str));
                }
            }

            return windows_list;
        }

        protected static bool EnumTheWindows(IntPtr hWnd, IntPtr lParam)
        {
            int size = GetWindowTextLength(hWnd);
            if (size++ > 0 && IsWindowVisible(hWnd))
            {
                StringBuilder sb = new StringBuilder(size);
                GetWindowText(hWnd, sb, size);
                windows_title.Add(sb.ToString());
            }
            return true;
        }
    }   
}
