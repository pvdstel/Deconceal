using Deconceal.Native;

namespace Deconceal.Api
{
    public class InputState
    {
        public static bool LeftMousePressed()
        {
            bool leftRightSwapped = Methods.GetSystemMetrics(SystemMetric.SM_SWAPBUTTON) > 0;
            VirtualKeys query = leftRightSwapped ? VirtualKeys.RightButton : VirtualKeys.LeftButton;
            // The most significant bit (the sign) of the short is set if the button is currently down.
            return Methods.GetAsyncKeyState(query) < 0;
        }
    }
}
