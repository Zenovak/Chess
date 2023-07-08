namespace Chess {
    
    public class ChessGameState {
        // byte board of 64
        /// <summary>
        /// The game board. Where index 0 is the upper left corner (black's Rook)
        /// </summary>
        public byte[] board;

        // tracts turn. 0 = white, 8 = black.
        public byte turn;

        // flag: wk wq, bk, bq
        public byte castlingRights;

        public ChessGameState() {
            board = new byte[64];
        }
    }
}
