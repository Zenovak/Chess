using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Chess {
    public class ChessGame {
        public ChessPiece[] board = new ChessPiece[64];

        enum pieceRef {
            // big letter is white
            Pawn = 0,
            Knight = 1,
            Rook = 2,
            Bishop = 4,
            King = 6,
            Queen = 7,
            // small is black
            pawn = 8,
            knight = 9,
            rook = 10,
            bishop = 12,
            king = 14,
            queen = 15
        }


        public ChessGame() {
            
        }

        public void StartingPlacement() {
            for (int i = 8; i < 16; i++) {
                board[i] = new ChessPiece((byte)pieceRef.Pawn);
                board[40 + i] = new ChessPiece((byte)pieceRef.pawn);
            }
        }

        public void CalculateMoves() {
            for (int i = 0; i < 64; i++) {
                if (board[i] == null)
                    continue;
                if (0)
            }
        }
    }


    public class ChessPiece {
        public byte pieceType;
        public List<Vector2> availableMoves;

        public ChessPiece(byte flagType) {
            pieceType = flagType;
        }
    }
}
