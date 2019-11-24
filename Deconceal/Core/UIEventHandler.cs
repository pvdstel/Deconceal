using Deconceal.Api;
using System;
using System.Windows.Automation;

namespace Deconceal.Core
{
    public class UIEventHandler
    {
        private Lazy<WindowPlacer> windowPlacer = new Lazy<WindowPlacer>();

        public void OnWindowOpened(object sender, AutomationEventArgs automationEventArgs)
        {
            try
            {
                var window = sender as AutomationElement;
                if (window != null && !InputState.LeftMousePressed())
                {
                    IntPtr windowHandle = (IntPtr)window.Current.NativeWindowHandle;
                    windowPlacer.Value.PlaceWindow(windowHandle);
                }
            }
            catch (ElementNotAvailableException)
            {
                Console.Error.WriteLine("The UI element was no longer available.");
            }
        }
    }
}
