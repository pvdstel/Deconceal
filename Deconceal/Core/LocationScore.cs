using Deconceal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deconceal.Core
{
    internal class LocationScore
    {
        public LocationScore(Point point)
        {
            Point = point;
            Score = 0;
        }

        public Point Point { get; private set; }

        public double Score { get; set; }

        public static IEnumerable<Point> CreatePointList(int xStart, int xEnd, int yStart, int yEnd, int step)
        {
            for (int x = xStart; x < xEnd; x += step)
            {
                for (int y = yStart; y < yEnd; y += step)
                {
                    yield return new Point(x, y);
                }
            }
        }

        public override string ToString()
        {
            return $"{{Score={Score}, Point={Point}}}";
        }

        public static implicit operator Point(LocationScore locationScore)
        {
            return locationScore.Point;
        }
    }
}
