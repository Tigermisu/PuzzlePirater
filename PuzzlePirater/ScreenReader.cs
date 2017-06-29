using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzlePirater {
    class ScreenReader {
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref WindowPosition rectangle);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        public struct WindowPosition {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        };

        public WindowPosition PuzzlePiratesWindowPostion { get { return windowPosition; } }

        public Rectangle puzzleRect;

        private IntPtr ppWindow = IntPtr.Zero;
        private WindowPosition windowPosition;


        /// <summary>
        /// Creates a new ScreenReader, initializing by finding an active PP window.
        /// </summary>
        public ScreenReader() {
            assertWindow();
        }

        /// <summary>
        /// Creates a new SR, with an active PP window and a pre-defined rectangle size for the desired puzzle
        /// </summary>
        /// <param name="puzzleRect">a Rectangle defining the size and position of the puzzle relative to the window.</param>
        public ScreenReader(Rectangle puzzleRect) {
            assertWindow();
            this.puzzleRect = puzzleRect;
        }

        /// <summary>
        /// Returns an screenshot of the active puzzle.
        /// </summary>
        /// <returns></returns>
        public Bitmap getPuzzlePreview() {
            Bitmap ss = getScreenshot();
            if (puzzleRect == null) return ss;
            return ss.Clone(puzzleRect, ss.PixelFormat);
        }

        /// <summary>
        /// Takes a screenshot of PP and saves it to the .exe folder.
        /// </summary>
        public void saveScreenshot() {
            Bitmap ss = getScreenshot();

            // croppedImage = ss.Clone(new Rectangle(50, 50, 200, 200), ss.PixelFormat);


            ss.Save((Int32)((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds) + ".png", ImageFormat.Png);
        }        

        /// <summary>
        /// Finds the position of the Puzzle Pirates window, and stores it in a member variable for further reference.
        /// </summary>
        private void getWindowPosition() {
            Process[] javaProcesses = Process.GetProcessesByName("javaw");
            foreach (Process p in javaProcesses) {
                if (p.MainWindowTitle.StartsWith("Puzzle Pirates")) {
                    ppWindow = p.MainWindowHandle;
                    GetWindowRect(ppWindow, ref windowPosition);
                    return;
                }
            }
            // Not a single process matches Puzzle Pirates
            throw new WindowNotFoundException("Puzzle Pirates window could not be found.");
            
        }

        /// <summary>
        /// Gets a screenshot of the active PP window.
        /// </summary>
        /// <returns>A bitmap containing the screenshot data.</returns>
        private Bitmap getScreenshot() {
            if(!assertWindow()) throw new WindowNotFoundException("Puzzle Pirates window could not be found.");

            Bitmap bmp = new Bitmap(
                windowPosition.Right - windowPosition.Left, // Width
                windowPosition.Bottom - windowPosition.Top, // Height
                PixelFormat.Format32bppArgb);

            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(ppWindow, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return bmp;
        }

        /// <summary>
        /// Asserts that we have an active Puzzle Pirates Window, tries to fetch it if not.
        /// </summary>
        private bool assertWindow() {
            if (ppWindow == IntPtr.Zero) {
                try {
                    getWindowPosition();
                } catch (Exception e) {
                    MessageBox.Show("Could not find Puzzle Pirates Instance.\nPlease launch the game and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(e);
                    Console.WriteLine(e.StackTrace);
                    return false;
                }
            }
            return true;
        }
    }
    
}
