using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzlePirater {
    class WindowNotFoundException : Exception {
        /// <summary>
        /// This exception is called when a requested window has not been found.
        /// </summary>
        public WindowNotFoundException() { }

        /// <summary>
        /// This exception is called when a requested window has not been found.
        /// </summary>
        /// <param name="message">The exception message</param>
        public WindowNotFoundException(string message) : base(message) { }

        /// <summary>
        /// This exception is called when a requested window has not been found.
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="inner">the inner exception</param>
        public WindowNotFoundException(string message, Exception inner) : base(message) { }
    }
}
