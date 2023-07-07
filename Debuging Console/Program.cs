using Chess;

namespace Debuging_Console {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
            var chess = new ChessGame();
            chess.CalculateMoves();
            chess.PrintBoard();
        }
    }
}