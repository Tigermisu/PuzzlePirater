using System.Collections.Generic;
using System.Linq;

namespace PuzzlePirater {
    public class BilgePiece {

        public enum PieceNames : byte {
            Mint,
            Swamp,
            Ocean,
            Sea,
            Water,
            Fish,
            SunkenMint,
            SunkenSwamp,
            SunkenOcean,
            SunkenSea,
            SunkenWater,
            SunkenFish,
            Unknown
        }

        public PieceNames pieceName;
        public int pieceCode;

        private Dictionary<double, PieceNames> pieceValues = new Dictionary<double, PieceNames>() {
            { 7219, PieceNames.Mint },
            { 4727, PieceNames.Swamp },
            { 4458, PieceNames.Ocean },
            { 6475, PieceNames.Sea },
            { 5474, PieceNames.Water },
            { 5941, PieceNames.Fish },
            { 4529, PieceNames.Sea }, // Sunken Sea
            { 3716, PieceNames.Ocean }, // Sunken Ocean
            { 4133, PieceNames.Water }, // Sunken Water
            { 4305, PieceNames.Fish }, // Sunken Fish
            { 3828, PieceNames.Swamp }, // Sunken Swamp
            { 4825, PieceNames.Mint }, // Sunken Mint
        };

        public BilgePiece(PieceNames pn) {
            pieceName = pn;
            pieceCode = (int)pieceValues.FirstOrDefault(x => x.Value == pn).Key;
        }

        public BilgePiece(int pieceCode) {
            this.pieceCode = pieceCode;
            if (pieceValues.ContainsKey(pieceCode)) {
                pieceName = pieceValues[pieceCode];
            } else {
                pieceName = PieceNames.Unknown;    
            }
        }
    }
}