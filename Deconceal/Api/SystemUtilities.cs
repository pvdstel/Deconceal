using Deconceal.Settings;

namespace Deconceal.Api
{
    public static class SystemUtilities
    {
        public static Rectangle ShrinkByResizeBorder(this Rectangle rect)
        {
            Rectangle resizeBorder = Configuration.Instance.ResizeBorder;
            return new Rectangle(
                rect.X + resizeBorder.X,
                rect.Y + resizeBorder.Y,
                rect.Width - resizeBorder.Right,
                rect.Height - resizeBorder.Bottom);
        }

        public static Rectangle GrowByResizeBorder(this Rectangle rect)
        {
            Rectangle resizeBorder = Configuration.Instance.ResizeBorder;
            return new Rectangle(
                rect.X - resizeBorder.X,
                rect.Y - resizeBorder.Y,
                rect.Width + resizeBorder.Right,
                rect.Height + resizeBorder.Bottom);
        }
    }
}
