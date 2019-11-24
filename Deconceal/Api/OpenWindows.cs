using Deconceal.Native;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deconceal.Api
{
    public class OpenWindows
    {
        private const int WindowTextCapacity = 256;
        private static VirtualDesktopManager virtualDesktopManager = new VirtualDesktopManager();

        public static List<Window> GetOpenWindowsInOrder()
        {
            List<Window> windows = new List<Window>();

            IntPtr current = IntPtr.Zero;
            IntPtr searchStart = Methods.GetDesktopWindow();
            StringBuilder windowTextBuffer = new StringBuilder(WindowTextCapacity);

            current = Methods.GetWindow(searchStart, GetWindowCommand.GW_CHILD);
            while (current != IntPtr.Zero)
            {
                WindowStyles currentStyles = (WindowStyles)Methods.GetWindowLong(current, WindowLongs.GWL_STYLE);
                WindowStylesEx currentStylesEx = (WindowStylesEx)Methods.GetWindowLong(current, WindowLongs.GWL_EXSTYLE);
                bool isStyleOk =
                    currentStyles.HasFlag(WindowStyles.WS_VISIBLE) &&
                    !currentStylesEx.HasFlag(WindowStylesEx.WS_EX_TOOLWINDOW) &&
                    (
                        !currentStyles.HasFlag(WindowStyles.WS_POPUP)
                        || currentStylesEx.HasFlag(WindowStylesEx.WS_EX_APPWINDOW)
                    ) &&
                    (
                        currentStyles.HasFlag(WindowStyles.WS_BORDER)
                        || currentStyles.HasFlag(WindowStyles.WS_CAPTION)
                        || currentStyles.HasFlag(WindowStyles.WS_SYSMENU)
                    );


                if (isStyleOk && !Methods.IsIconic(current) && virtualDesktopManager.IsWindowOnCurrentVirtualDesktop(current))
                {
                    Methods.GetWindowText(current, windowTextBuffer, WindowTextCapacity);
                    WINDOWPLACEMENT placement = WINDOWPLACEMENT.Default;
                    Methods.GetWindowPlacement(current, ref placement);

                    windows.Add(new Window()
                    {
                        Handle = current,
                        Text = windowTextBuffer.ToString(),
                        Rect = ((Rectangle)placement.NormalPosition).ShrinkByResizeBorder(),
                        Monitor = Monitor.GetMonitorFromRect(placement.NormalPosition),
                        IsMaximized = placement.ShowCmd == ShowWindowCommands.Maximize,
                    });
                    windowTextBuffer.Clear();
                }

                current = Methods.GetWindow(current, GetWindowCommand.GW_HWNDNEXT);
                if (current == searchStart)
                {
                    current = Methods.GetWindow(current, GetWindowCommand.GW_HWNDNEXT);
                }
            }

            return windows;
        }
    }
}
