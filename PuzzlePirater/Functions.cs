using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzlePirater {
    static class Functions {
        /// <summary>
        /// Returns an interpolation following standard bezier curve
        /// </summary>
        /// <param name="t">a number from 0 to 1 indicating the interpolation status</param>
        /// <returns>The interpolated value</returns>
        public static double bezierCurve(double t) {
            t = clamp(t, 1, 0);
            return Math.Pow(t, 2) * (3.0f - 2.0f * t);
        }

        /// <summary>
        /// Returns an interpolation based on a parametric blend
        /// </summary>
        /// <param name="x">The state of the interpolation, ranging from 1 to 0</param>
        /// <param name="a">The parameter of the interpolation, values approaching 0 net a linear interpolation, while infinity results in a step function.</param>
        /// <returns>The interpolated value</returns>
        public static double parametricBlend(double x, double a) {
            x = clamp(x, 1, 0);
            double xa = Math.Pow(x, a);
            return xa / (xa + Math.Pow(1 - x, a));
        }

        /// <summary>
        /// Clamps a value between a minimum and a max value.
        /// </summary>
        /// <param name="v">The value to clamp</param>
        /// <param name="max">The max value</param>
        /// <param name="min">The min value</param>
        /// <returns>the clamped value</returns>
        public static double clamp(double v, double max, double min) {
            return Math.Max(Math.Min(max, v), min);
        }
    }
}
