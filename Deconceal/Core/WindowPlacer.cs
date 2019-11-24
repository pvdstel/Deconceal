using Deconceal.Api;
using Deconceal.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deconceal.Core
{
    public class WindowPlacer
    {
        public void PlaceWindow(IntPtr handle)
        {
            var windows = OpenWindows.GetOpenWindowsInOrder();
            Window targetWindow = windows.Find(w => w.Handle == handle);
            if (targetWindow == null)
            {
                Console.WriteLine($"Window with handle {handle} not found in open windows, skipping placement.");
                return;
            }
            if (targetWindow.IsMaximized)
            {
                return;
            }

            var candidatePositions = CreateSearchPoints(targetWindow);

            var (bestPoint, targetMonitor) = FindBestPoint(candidatePositions, targetWindow, windows);
            for (int searchStep = 1; searchStep < Configuration.Instance.SearchDistance.Length; ++searchStep)
            {
                int searchDistance = Configuration.Instance.SearchDistance[searchStep];
                int previousDistance = Configuration.Instance.SearchDistance[searchStep - 1];

                var optimizingCandidates = LocationScore.CreatePointList(
                    Math.Max(targetMonitor.WorkingArea.Left, bestPoint.Point.X - previousDistance + 1),
                    Math.Min(targetMonitor.WorkingArea.Right - targetWindow.Rect.Width, bestPoint.Point.X + previousDistance - 1),
                    Math.Max(targetMonitor.WorkingArea.Top, bestPoint.Point.Y - previousDistance + 1),
                    Math.Min(targetMonitor.WorkingArea.Bottom - targetWindow.Rect.Height, bestPoint.Point.Y + previousDistance - 1),
                    searchDistance);

                bestPoint = FindBestPoint(optimizingCandidates, targetMonitor, targetWindow, windows);
            }

            targetWindow.MoveTo(bestPoint);
        }

        private Dictionary<Monitor, IEnumerable<Point>> CreateSearchPoints(Window targetWindow)
        {
            Dictionary<Monitor, IEnumerable<Point>> result = new Dictionary<Monitor, IEnumerable<Point>>();
            Monitor targetMonitor = Monitor.GetMonitorFromRect(targetWindow.Rect);

            List<Monitor> monitorsToExamine = Configuration.Instance.SpanDisplays
                ? Monitor.GetMonitors()
                : new List<Monitor>() { targetMonitor };

            foreach (Monitor monitor in monitorsToExamine)
            {
                int endX = monitor.WorkingArea.Right - targetWindow.Rect.Width;
                int endY = monitor.WorkingArea.Bottom - targetWindow.Rect.Height;

                result[monitor] = LocationScore.CreatePointList(monitor.WorkingArea.Left, endX, monitor.WorkingArea.Top, endY, Configuration.Instance.SearchDistance[0]);
            }

            return result;
        }

        private (LocationScore BestPoint, Monitor Monitor) FindBestPoint(Dictionary<Monitor, IEnumerable<Point>> points, Window targetWindow, List<Window> windows)
        {
            (LocationScore bestPoint, Monitor monitor) result = (new LocationScore(new Point()) { Score = int.MaxValue }, null);
            foreach (var monitorPoints in points)
            {
                LocationScore monitorResult = FindBestPoint(monitorPoints.Value, monitorPoints.Key, targetWindow, windows);
                if (monitorResult.Score < result.bestPoint.Score)
                {
                    result = (monitorResult, monitorPoints.Key);
                }
            }

            return result;
        }

        private LocationScore FindBestPoint(IEnumerable<Point> points, Monitor monitor, Window targetWindow, List<Window> windows)
        {
            List<Window> windowsToExamine = windows
                .Where(w => w.Monitor == monitor && w != targetWindow)
                .Take(Configuration.Instance.SearchDepth)
                .ToList();

            List<LocationScore> locationScores = points.Select(p => new LocationScore(p)).ToList();
            for (int i = 0; i < windowsToExamine.Count; ++i)
            {
                foreach (LocationScore ls in locationScores)
                {
                    int penalty = RectangleIntersectionArea(
                        windowsToExamine[i].Rect,
                        ls.Point.X,
                        ls.Point.Y,
                        ls.Point.X + targetWindow.Rect.Width,
                        ls.Point.Y + targetWindow.Rect.Height);
                    ls.Score += penalty / Math.Pow(Configuration.Instance.PriorityPenalty, i);
                }
            }

            return locationScores.OrderBy(ls => ls.Score).ThenBy(ls => ls.Point.Y).ThenBy(ls => ls.Point.X).First();
        }

        private static int RectangleIntersectionArea(in Rectangle r, int left, int top, int right, int bottom)
        {
            int maxLeft = Math.Max(r.Left, left);
            int maxTop = Math.Max(r.Top, top);
            int minRight = Math.Min(r.Right, right);
            int minBottom = Math.Min(r.Bottom, bottom);

            int dx = minRight - maxLeft;
            int dy = minBottom - maxTop;

            return Math.Max(dx, 0) * Math.Max(dy, 0);
        }
    }
}
