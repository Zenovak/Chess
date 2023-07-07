using System.Linq;

namespace Chess {
    public static class FENManager {

        private readonly static Dictionary<char, byte> _fenRef = new Dictionary<char, byte>() {
            {'P', 1 },
            {'N', 2 },
            {'R', 5 },
            {'B', 3 },
            {'K', 6 },
            {'Q', 7 },

            {'p', 9 },
            {'n', 10 },
            {'r', 13 },
            {'b', 11 },
            {'k', 14 },
            {'q', 15 },
        };


        public static ChessGameState ReadFEN(string fenSequence) {
            var gameStateExport = new ChessGameState();

            var sequence = fenSequence.Split(" ");

            var piecePos = sequence[0].Split("/");
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    gameStateExport.board[i * 8 + j] = _readPosition(piecePos[i])[j];
                }
            }

            if (sequence.Length > 1)
                gameStateExport.turn = _readTurn(sequence[1]);

            if (sequence.Length > 2)
                gameStateExport.castlingRights = _readCastingRights(sequence[2]);

            return gameStateExport;
        }








        // -------------------------- Private methods------------------------------------

        /// <summary>
        /// Reads a FEN sequence for a row's position and returns the byteboard representation equivalent.
        /// </summary>
        /// <param name="fenRow"></param>
        /// <returns></returns>
        private static byte[] _readPosition(string fenRow) {
            
            byte[] row = new byte[8];
            var Index = 0;
            for (int i = 0; i < fenRow.Length; i++) {

                if (!_fenRef.ContainsKey(fenRow[i])) {
                    Index +=  int.Parse(fenRow[i].ToString());
                }

                else {

                    row[Index] = _fenRef[fenRow[i]];
                    Index += 1;
                }
            }
            return row;
        }


        /// <summary>
        /// Reads the FEN sequence fragment for a turn and returns our proprietary GameState equavalent.
        /// </summary>
        /// <param name="fenSequence"></param>
        /// <returns>White = false, Black = true</returns>
        private static byte _readTurn(string fenSequence) {
            if (fenSequence == null) return 0;
            if (fenSequence == "b") return 8;

            return 0;
        }


        /// <summary>
        /// Reads the FEN sequence fragment for castling, and returns our proprietary GameState equavalent.
        /// </summary>
        /// <param name="fenSequence"></param>
        /// <returns>bitflag: WhiteKing, WhiteQueen, BlackKing, BlackQueen</returns>

        private static byte _readCastingRights(string fenSequence) {
            byte castlingFlag = 0;
            if (fenSequence == null || fenSequence == "-") 
                return 0;
            else {
                if (fenSequence.Contains("K"))
                    castlingFlag |= 8;
                if (fenSequence.Contains("Q"))
                    castlingFlag |= 4;
                if (fenSequence.Contains("k"))
                    castlingFlag |= 2;
                if (fenSequence.Contains("q"))
                    castlingFlag |= 1;

                return castlingFlag;
            }
        }
    }
}
