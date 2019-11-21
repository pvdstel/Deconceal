using Deconceal.Native;
using System;

namespace Deconceal.Api
{
    public class Window
    {
        public IntPtr Handle { get; set; }

        public string Text { get; set; }

        public Rectangle Rect { get; set; }

        public Monitor Monitor { get; set; }

        public bool IsMaximized { get; set; }

        public void MoveTo(Point p)
        {
            WINDOWPLACEMENT currentPlacement = WINDOWPLACEMENT.Default;
            Methods.GetWindowPlacement(Handle, ref currentPlacement);
            RECT cpNp = ((Rectangle)currentPlacement.NormalPosition).ShrinkByResizeBorder();

            WINDOWPLACEMENT nextPlacement = WINDOWPLACEMENT.Default;
            nextPlacement.Flags = currentPlacement.Flags;
            nextPlacement.MaxPosition = currentPlacement.MaxPosition;
            nextPlacement.MinPosition = currentPlacement.MinPosition;
            nextPlacement.NormalPosition = new Rectangle(p.X, p.Y, cpNp.Right - cpNp.Left, cpNp.Bottom - cpNp.Top).GrowByResizeBorder();
            nextPlacement.ShowCmd = currentPlacement.ShowCmd;
            Methods.SetWindowPlacement(Handle, ref nextPlacement);
        }

        public override string ToString()
        {
            return $"{Handle}: {Text} @ {Rect} @ {Monitor.DeviceName}";
        }
    }
}
