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

        public BilgeMove getBestMove(BilgePiece[] boardState) {
            List<BilgeMove> bilgeMoves = new List<BilgeMove>();
            for(int i = 0; i < boardState.Length; i++) { // For every piece 
                BilgePiece p = boardState[i];
                if(p.pieceName == BilgePiece.PieceNames.Unknown) continue; // Skip unknown pieces
                int xPos = i % 6,
                    yPos = i / 6,
                    moveScore = 0,
                    horizontalPieces = 0,
                    uniqueVerticalPieces = 0,
                    xMean = xPos;
                
                int[] piecesPerVerticalRow = {0, 0, 1, 0, 0};

                // Check for horizontal moves
                for(int j = 0; j < 6; j++) {
                    if(p.pieceName == boardState[i + j - xPos] && j != xPos) {
                        horizontalPieces++;
                    }
                }

                if(horizontalPieces >= 2) { // Calculate possible move horizontally
                    List<Point> clickPoints = new List<Point>();
                    BilgeMove horizontalMove = new BilgeMove();
                    int leftPieces = 0, rightPieces = 0;
                    for(int j = 0; j < 6 && leftPieces + rightPieces < 2; j++) {
                        if(p.pieceName == boardState[i + j - xPos] &&
                         ((j + leftPieces < xPos && j < xPos ) || (j - rightPieces > xPos && j > xPos))) {
                            if( j < xPos ) {
                                for(int k = j + 1; k + leftPieces < xPos; k++) {
                                    clickPoints.Add(new Point(k, yPos));
                                }
                                leftPieces++;
                            } else {
                                for(int k = j - 1; k - rightPieces > xPos; k--) {
                                    clickPoints.Add(new Point(k, yPos));
                                }
                                rightPieces++;
                            }
                        }
                    }
                    horizontalMove.Score = 100 - clickPoints.Count * 20;
                    horizontalMove.Clicks = clickPoints.ToArray();
                    bilgeMoves.Add(horizontalMove);
                }

                // Check for vertical moves
                for(int j = yPos - 2; j <= yPos + 2; j++) {
                    if(j < 0) j = 0; // Prevent going out of the board
                    if(j == yPos) continue; // Ignore current row
                    for(int k = 0; k < 6; k++) {
                        targetIndex = (i - xPos + k) + 6 * (j - yPos);
                        if(p.pieceName == boardState[targetIndex] && j != yPos) {
                            if(piecesPerVerticalRow[j - yPos + 2] == 0) uniqueVerticalPieces++;
                            // TODO: Implement pseudo:
                            /*
                             xMean = xMean * sum(piecesPerVerticalRow) + k;
                             piecesPerVerticalRow[j - yPos + 2]++;
                             xMean /= sum(piecesPerVerticalRow);                             
                            */
                            piecesPerVerticalRow[j - yPos + 2]++;
                        }
                    }
                }

                if(uniqueVerticalPieces >= 2) { // Calculate best move vertically
                    List<Point> clickPoints = new List<Point>();
                    BilgeMove verticalMove = new verticalMove();

                    int targetPos = Math.Round(xMean),
                        pieces = 0;
                    if(piecesPerVerticalRow[0] > 0 && piecesPerVerticalRow[1] > 0) {
                        // Place upper piece
                        moveBestPieceHorizontally(yPos - 2, targetPos, boardState, p.pieceName, clickPoints);
                        pieces++;
                    }
                    if(piecesPerVerticalRow[4] > 0 && piecesPerVerticalRow[3] > 0) {
                        // Place lower piece
                        moveBestPieceHorizontally(yPos + 2, targetPos, boardState, p.pieceName, clickPoints);
                        pieces++;                        
                    }
                    if(piecesPerVerticalRow[1] > 0) {
                        //Check if movement would trigger break
                        if(piecesPerVerticalRow[0] > 0 && targetPos == xPos) {
                            moveBestPieceHorizontally(yPos, xPos + 1);
                        }                       
                        moveBestPieceHorizontally(yPos - 1, targetPos, boardState, p.pieceName, clickPoints);
                        pieces++; 
                    }
                    if(piecesPerVerticalRow[3] > 0) {
                        // Place piece below
                        moveBestPieceHorizontally(yPos + 1, targetPos, boardState, p.pieceName, clickPoints);
                        pieces++; 
                    }
                    moveBestPieceHorizontally(yPos, targetPos, boardState, p.pieceName, clickPoints);

                    verticalMove.Score = 100 - clickPoints.Count * 20 + pieces * 20;
                    verticalMove.Clicks = clickPoints.ToArray();
                    bilgeMoves.Add(verticalMove);
                }
            }
            
            /* TODO: Test it works before making it recursive
            // For each possible move
            foreach (BilgeMove move in bilgeMoves) {
                // Simulate it and then recursively find the next best move
                BilgePiece[] futureState = simulateMove(boardState, move);
                move.Score +=  getBestMove(futureState).Score;
            }
             */

            // Obtain and return the bilge move with the highest score
            return bilgeMoves.Aggregate((max, i) => (i.Score > max.Score ? i : max));
        }

        private void moveBestPieceHorizontally(int row, int targetPos, BilgePiece[] boardState, BilgePiece.PieceNames pieceName, ref List<Point> clickPoints) {
            int closestPos = 0,
                closestDistance = Math.Infinity;
            for(int i = row * 6, limit = i + 6; i < limit; i++) {
                if(boardState[i].pieceName == pieceName && Math.Abs(i - targetPos) < closestDistance) {
                    closestPos = i;
                    closestDistance = Math.Abs(i - targetPos);
                }
            }
            for(int i = 1, direction = closestPos < targetPos? 1:-1; i <= closestDistance; i++) {
                clickPoints.Add(closestPos + i * direction, row);
            }
        }

        private BilgePiece[] simulateMove(BilgePiece[] boardState, BilgeMove move) {

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
