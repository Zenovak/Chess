using Chess;

namespace Debuging_Console {
    internal class Program {
        static void Main(string[] args) {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            watch.Start();
            Console.WriteLine("Hello, World!");
            var chess = new ChessGame();
            chess.CalculateMoves();
            chess.DebugPrintBoard();
            chess.DebugPrintAttackSquares(0);
            watch.Stop();
            Console.WriteLine("Executed In: " + watch.ElapsedMilliseconds);
        }
    }
}