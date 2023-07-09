namespace Chess {
    public static class ChessDebugTools {
        /// <summary>
        /// Debug Extension Tools used to print the board to console.
        /// </summary>
        /// <param name="chessGame"></param>
        /// <returns></returns>
        public static ChessGame DebugPrintBoard(this ChessGame chessGame) {
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    if (chessGame.GameState.board[i * 8 + j] == 0)
                        Console.Write(new string(" "));
                    else {
                        Console.Write(chessGame.GameState.board[i * 8 + j]);
                    }

                    Console.Write(new string(", "));
                }
                Console.WriteLine();
            }
            Console.WriteLine("Now its " + chessGame.GameState.turn + "'s turn");
            Console.WriteLine("Castling Rights: " + chessGame.GameState.castlingRights);
            Console.WriteLine("Moves available: ");
            
            DebugPrintMoves(chessGame);

            return chessGame;
        }
        private static void DebugPrintRow(byte[] row) {
            Console.WriteLine();
            foreach (var num in row) {
                Console.Write(num.ToString() + ", ");
            }
            Console.WriteLine();
        }

        private static ChessGame DebugPrintMoves(this ChessGame chessGame) {
            foreach (var kvp in chessGame.moves) {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, string.Join(", ", kvp.Value));
            }
            return chessGame;
        }

        private readonly static Dictionary<char, byte> Reference = new Dictionary<char, byte>() {
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


        /// <summary>
        /// Prints the attacking Squares of the desired side.
        /// </summary>
        /// <param name="attackerSide">0 = White, 8 = Black.</param>
        public static void DebugPrintAttackSquares(this ChessGame chess, byte attackerSide) {
            var results = chess.CalculateAttackSquares(chess.GameState.board, attackerSide);

            Console.WriteLine(string.Join(", ", results));
        }
    }

}
