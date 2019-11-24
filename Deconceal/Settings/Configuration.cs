using Deconceal.Api;

namespace Deconceal.Settings
{
    public class Configuration
    {
        public static Configuration Instance = Default();

        public static Configuration Default() => new Configuration()
        {
            SearchDistance = new int[] { 100, 10, 1 },
            SpanDisplays = false,
            SearchDepth = 8,
            PriorityPenalty = 1.25,
            ResizeBorder = new Rectangle(0, 0, 0, 0),
        };

        /// <summary>
        /// Whether all displays should be taken into account when positioning the window.
        /// If false, only the display the window is on will be used.
        /// </summary>
        public bool SpanDisplays { get; set; }

        /// <summary>
        /// The search approximation distance.
        /// </summary>
        public int[] SearchDistance { get; set; }

        /// <summary>
        /// The number of windows to evaluate.
        /// </summary>
        public int SearchDepth { get; set; }

        /// <summary>
        /// How much the priority of back windows is reduced.
        /// </summary>
        public double PriorityPenalty { get; set; }

        public Rectangle ResizeBorder { get; set; }
    }
}
