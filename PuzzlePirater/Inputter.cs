using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzlePirater {
    static class Inputter {
        private static Semaphore mouseMovementSemaphore = new Semaphore(1, 1);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;

        private static Queue<ClickAction> clickQueue = new Queue<ClickAction>();
        private static Thread clickQueueConsumer;

        /// <summary>
        /// Enqueues a ClickAction into the Inputter for further processing.
        /// </summary>
        /// <param name="click">The ClickAction Object containing the action parameters.</param>
        public static void enqueueClick(ClickAction click) {
            clickQueue.Enqueue(click);
        }

        /// <summary>
        /// Clears the current ClickAction Queue
        /// </summary>
        public static void clearQueue() {
            clickQueue = new Queue<ClickAction>();
        }

        /// <summary>
        /// Starts a new thread that constantly executes ClickActions stored in the queue
        /// </summary>
        public static void startQueueConsumption() {
            if (clickQueueConsumer == null) { 
                clickQueueConsumer = new Thread(() => consumeQueue());
                clickQueueConsumer.Start();
            }
        }

        /// <summary>
        /// Aborts the thread executing ClickActions, if present, and optionally clears the queue.
        /// </summary>
        /// <param name="clearClickQueue">Should the ClickAction Queue also be cleared?</param>
        public static void abortQueueConsumption(bool clearClickQueue = false) {
            if (clickQueueConsumer != null) {
                clickQueueConsumer.Abort();
                clickQueueConsumer = null;
            } 
            if (clearClickQueue) clearQueue();
        }

        /// <summary>
        /// Defines the consumption of the clickQueue
        /// </summary>
        private static void consumeQueue() {
            while (true) {
                if (clickQueue.Count > 0) {
                    ClickAction nextClick = clickQueue.Dequeue();

                    mouseMovementSemaphore.WaitOne();
                    smoothenMouseMovement(nextClick.DesiredPosition, nextClick.GonnaClick);
                    mouseMovementSemaphore.Release();
                }

                Thread.Sleep(new Random().Next(17, 372));
            }            
        }

        /// <summary>
        /// Moves the mouse pointer smoothly from its actual position to a target position, following a pseudo-random parametrized trajectory
        /// </summary>
        /// <param name="targetPos">The target final position of the pointer</param>
        /// <param name="click">Should the mouse be clicked at the end of the movement?</param>
        private static void smoothenMouseMovement(Point targetPos, bool click) {
            const int REFRESH_RATE = 120,
                AVERAGE_MOVEMENT_SIZE = 20000; // 100^2 + 100^2 - assumes average movement of 100px by 100px;
            
            Random rng = new Random();
            Point initialPosition = Cursor.Position;

            int timeToMove = (int)(Math.Log10((Math.Pow(targetPos.X - initialPosition.X, 2) + Math.Pow(targetPos.Y - initialPosition.Y, 2) / AVERAGE_MOVEMENT_SIZE + 1)) * (rng.NextDouble() * 0.5 + 0.75)) * REFRESH_RATE / 10;
            double xA = rng.NextDouble() * 0.5 + 1.75,
                yA = xA + rng.NextDouble() * 0.5 - 0.25;

            for (int i = 0; i <= timeToMove; i++) {
                double targetXPos = initialPosition.X + Functions.parametricBlend(i / (double)timeToMove, xA) * (targetPos.X - initialPosition.X),
                    targetYPos = initialPosition.Y + Functions.parametricBlend(i / (double)timeToMove, yA) * (targetPos.Y - initialPosition.Y);

                Cursor.Position = new Point((int)targetXPos, (int)targetYPos);
                Thread.Sleep(1000 / REFRESH_RATE);
            }

            if (click) {
                Thread.Sleep(rng.Next(0, 150));
                doClick();
            }            
        }

        /// <summary>
        /// Sends a click event at the current cursor position.
        /// </summary>
        private static void doClick() {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new UIntPtr());
            Thread.Sleep(new Random().Next(40, 120));
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new UIntPtr());
        }

    }
}
