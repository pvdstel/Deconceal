using Deconceal.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Deconceal.Api
{
    public class Monitor
    {
        private static Dictionary<IntPtr, Monitor> _handleCache = new Dictionary<IntPtr, Monitor>();

        public Rectangle Bounds { get; private set; }

        public Rectangle WorkingArea { get; private set; }

        public string DeviceName { get; private set; }

        public static List<Monitor> GetMonitors()
        {
            List<Monitor> monitorHandles = new List<Monitor>();
            Methods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (IntPtr handle, IntPtr hdc, RECT rect, IntPtr dwData) =>
            {
                monitorHandles.Add(FromHandle(handle));
                return true;
            }, IntPtr.Zero);
            return monitorHandles;
        }

        public static Monitor GetMonitorFromRect(Rectangle rect)
        {
            RECT nativeRect = rect;
            IntPtr monitorHandle = Methods.MonitorFromRect(ref nativeRect, MonitorDefaultingFlags.MONITOR_DEFAULTTONEAREST);
            return FromHandle(monitorHandle);
        }

        private static Monitor FromHandle(IntPtr handle)
        {
            if (_handleCache.ContainsKey(handle))
            {
                return _handleCache[handle];
            }
            else
            {
                MonitorInfoEx monitorInfo = new MonitorInfoEx();
                monitorInfo.Size = Marshal.SizeOf(monitorInfo);
                Methods.GetMonitorInfo(handle, ref monitorInfo);
                Monitor monitor = new Monitor()
                {
                    Bounds = monitorInfo.Monitor,
                    WorkingArea = monitorInfo.WorkArea,
                    DeviceName = monitorInfo.DeviceName,
                };
                _handleCache[handle] = monitor;
                return monitor;
            }
        }
    }
}
