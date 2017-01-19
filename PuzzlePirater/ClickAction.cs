using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PuzzlePirater {
    class ClickAction {
        public Point DesiredPosition { get; set; }
        public bool GonnaClick { get; set; }

        public ClickAction() {
            GonnaClick = false;
            DesiredPosition = new Point(0, 0);
        }

        public ClickAction(Point p) {
            GonnaClick = false;
            DesiredPosition = p;
        }

        public ClickAction(bool gc) {
            GonnaClick = gc;
            DesiredPosition = new Point(0, 0);
        }

        public ClickAction(Point p, bool gc) {
            GonnaClick = gc;
            DesiredPosition = p;
        }
    }
}
