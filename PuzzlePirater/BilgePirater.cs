using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzlePirater {
    class BilgePirater {
        private Rectangle bilgeRect = new Rectangle(91, 71, 272, 542);
        private ScreenReader bilgeReader;

        public BilgePirater() {
            bilgeReader = new ScreenReader(bilgeRect);
        }

        /// <summary>
        /// Gets an unprocessed image of the puzzle.
        /// </summary>
        /// <returns>a Bitmap containing a raw screenshot of the puzzle</returns>
        public Bitmap getRaw() {
            return bilgeReader.getPuzzlePreview();
        }

        /// <summary>
        /// Gets the grayscale representation of the puzzle
        /// </summary>
        /// <returns>The grayscale bitmap of the puzzle</returns>
        public Bitmap getGrayScale() {
            return processToGrayScale();
        }

        private Bitmap processToGrayScale() {
            Bitmap rawImage = getRaw();
            Rectangle rect = new Rectangle(0, 0, rawImage.Width, rawImage.Height);
            IntPtr ptr;
            int bytesNumber;
            byte[] rgbValues;

            System.Drawing.Imaging.BitmapData bmpData =
                rawImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                rawImage.PixelFormat);

            ptr = bmpData.Scan0;

            bytesNumber = rawImage.Width * rawImage.Height * 4;

            rgbValues = new byte[bytesNumber];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytesNumber);

            for (int i = 0; i < rgbValues.Length; i += 4 ) {
                byte weightedAverage = (byte)(0.3 * rgbValues[i] + 0.59 * rgbValues[i + 1] + 0.11 * rgbValues[i + 2]);
                rgbValues[i] = rgbValues[i + 1] = rgbValues[i + 2] = weightedAverage;
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytesNumber);

            rawImage.UnlockBits(bmpData);

            return rawImage;
        }

        /// <summary>
        /// Processes a raw bitmap into an array of pieces
        /// </summary>
        /// <returns>an Array of BilgePieces</returns>
        public BilgePiece[] processBoard() {
            List<BilgePiece> bilgePieces = new List<BilgePiece>();
            Bitmap grayScaleImage = processToGrayScale();
            Rectangle rect = new Rectangle(0, 0, grayScaleImage.Width, grayScaleImage.Height);
            IntPtr ptr;
            int bytesNumber;
            byte[] rgbValues;

            System.Drawing.Imaging.BitmapData bmpData =
                grayScaleImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                grayScaleImage.PixelFormat);

            ptr = bmpData.Scan0;

            bytesNumber = grayScaleImage.Width * grayScaleImage.Height * 4;

            rgbValues = new byte[bytesNumber];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytesNumber);

            // for every row
            for (int i = 21760; i < rgbValues.Length; i += 48960) {
                // for every piece
                for (int j = 20; j < 1088; j += 180) {
                    int pieceCode = 0;
                    // read the data of 35 pixels
                    for (int k = 0; k < 140; k += 4) {
                        pieceCode += rgbValues[i+j+k];
                    }
                    bilgePieces.Add(new BilgePiece(pieceCode));
                }
            }

            grayScaleImage.UnlockBits(bmpData);

            return bilgePieces.ToArray();
        }

        /// <summary>
        /// Returns a byte array consiting of the board's pixels
        /// </summary>
        /// <returns>An RGBA byte array for every op</returns>
        private byte[] getBoardPixels() {
            throw new NotImplementedException();
        }
    }
}
