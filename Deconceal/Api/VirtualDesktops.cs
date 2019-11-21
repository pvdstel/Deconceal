using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Deconceal.Api
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b")]
    [System.Security.SuppressUnmanagedCodeSecurity]
    public interface IVirtualDesktopManager
    {
        [PreserveSig]
        int IsWindowOnCurrentVirtualDesktop([In] IntPtr TopLevelWindow, [Out] out int OnCurrentDesktop);

        [PreserveSig]
        int GetWindowDesktopId([In] IntPtr TopLevelWindow, [Out] out Guid CurrentDesktop);

        [PreserveSig]
        int MoveWindowToDesktop([In] IntPtr TopLevelWindow, [MarshalAs(UnmanagedType.LPStruct)] [In]Guid CurrentDesktop);
    }

    [ComImport, Guid("aa509086-5ca9-4c25-8f95-589d3c07b48a")]
    internal class CVirtualDesktopManager { }

    public class VirtualDesktopManager
    {
        private CVirtualDesktopManager cmanager = null;
        private IVirtualDesktopManager manager;

        public VirtualDesktopManager()
        {
            cmanager = new CVirtualDesktopManager();
            manager = (IVirtualDesktopManager)cmanager;
        }

        ~VirtualDesktopManager()
        {
            manager = null;
            cmanager = null;
        }

        public bool IsWindowOnCurrentVirtualDesktop(IntPtr TopLevelWindow)
        {
            int result;
            int hr = manager.IsWindowOnCurrentVirtualDesktop(TopLevelWindow, out result);

            if (hr != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            return result != 0;
        }

        public Guid GetWindowDesktopId(IntPtr TopLevelWindow)
        {
            Guid result;
            int hr= manager.GetWindowDesktopId(TopLevelWindow, out result);

            if (hr != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            return result;
        }

        public void MoveWindowToDesktop(IntPtr TopLevelWindow, Guid CurrentDesktop)
        {
            int hr = manager.MoveWindowToDesktop(TopLevelWindow, CurrentDesktop);

            if (hr != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }
    }
}
