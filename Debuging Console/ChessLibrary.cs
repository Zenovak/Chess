﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Chess {
    public class ChessGame {
        public ChessGameState GameState { get; set; }


        // Moves
        public Dictionary<int, List<int>> moves { get; }


        public ChessGame() {
            this.GameState = new ChessGameState();
            this.moves = new Dictionary<int, List<int>>();


            this.StartingPlacement();
        }

        public void StartingPlacement() {

            GameState = ImportFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
            GameState = ImportFEN("r1bk3r/p2pBpNp/n4n2/1p1NP2P/6P1/3P4/P1P1K3/q5b1");
            GameState = ImportFEN("8/8/8/4p1K1/2k1P3/8/8/8 b - - 0 1");
            GameState = ImportFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e3 0 1");
        }


        public ChessGameState ImportFEN(string fenSequence) {
            return FENManager.ReadFEN(fenSequence);
        }


        public void CalculateMoves() {
            for (int i = 0; i < 64; i++) {
                var currentPiece = GameState.board[i];

                if (currentPiece == 0)
                    continue;

                // Turn Check (bitwise)
                if ((currentPiece & 8) != (GameState.turn & 8))
                    continue;


                // pawn (ID Checks)
                if (currentPiece == 9 || currentPiece == 1) {
                    AddMoves(i, Pawn(i, GameState.turn, GameState.board, false));
                }


                // knight (ID checks)
                if (currentPiece == 2 || currentPiece == 10) {
                    AddMoves(i, knight(i, GameState.board));
                }

                // Long Diagonal Checks (Bitwise for Q and B)
                if ((currentPiece & 3) == 3) {
                    AddMoves(i, Diagonal(i, GameState.board, true));
                }

                // Long Cross Checks (Bitwise for Q and R)
                if ((currentPiece & 5) == 5) {
                    AddMoves(i, Cross(i, GameState.board, true));
                }

                // King 
                if (currentPiece == 6 || currentPiece == 14)
                    AddMoves(i, King(i, GameState.board));
            }
            Console.WriteLine("Checked all moves");
        }


        private void CalculateAttackSquares(byte[] board, byte attacker) {
            for (int i = 0; i < 64; i++) {
                var currentPiece = board[i];

                if (currentPiece == 0) continue;

                if (isEnemy(currentPiece, attacker)) continue;

                if (currentPiece == 9 || currentPiece == 10) {

                }

                // knight (ID checks)
                if (currentPiece == 2 || currentPiece == 10) {
                    knight(i, GameState.board);
                }

                // Long Diagonal Checks (Bitwise for Q and B)
                if ((currentPiece & 3) == 3) {
                    Diagonal(i, GameState.board, true);
                }

                // Long Cross Checks (Bitwise for Q and R)
                if ((currentPiece & 5) == 5) {
                    Cross(i, GameState.board, true);
                }

                // King 
                if (currentPiece == 6 || currentPiece == 14)
                    King(i, GameState.board);


            }
        }




// ------------- Movement Calculations -----------Private Methods-----------


        private void Pawn(int Index, byte turn, byte[] board) {

            if (turn == 0) {
                moves[Index].Add(Index - 8);

                if (startingPawnPositions.Contains(Index))
                    moves[Index].Add(Index - 16);

                // Attacks. If destination not same side and not on edge and destination not empty: 
                if ((board[Index] & 8) != (board[Index - 9] & 8) && (Index % 8 != 0) && (board[Index - 9] != 0))
                    moves[Index].Add(Index - 9);

                if ((board[Index] & 8) != (board[Index - 7] & 8) && (Index % 8 != 7) && (board[Index - 7] != 0))
                    moves[Index].Add(Index - 7);
            }
            else {
                moves[Index].Add(Index + 8);

                if (startingPawnPositions.Contains(Index))
                    moves[Index].Add(Index + 16);

                if ((board[Index] & 8) != (board[Index + 9] & 8) && (Index % 8 != 0) && (board[Index + 9] != 0))
                    moves[Index].Add(Index + 9);

                if ((board[Index] & 8) != (board[Index + 7] & 8) && (Index % 8 != 7) && (board[Index + 7] != 0))
                    moves[Index].Add(Index + 7);
            }
        }

        private static int[] startingPawnPositions = new int[16] {
            8, 9, 10, 11, 12, 13, 14, 15, 48, 49, 50, 51, 52, 53, 54, 55
        };

        private List<int> Pawn(int index, byte turn, byte[] board, bool attackMovesOnly) {
            var row = index / 8;
            var col = index % 8;

            var pos = new Vector2(col, row);
            var calculatedMoves = new List<int>();

            var displacements = new Vector2[8] {
                new Vector2(0, -1), new Vector2(0, -2),
                new Vector2(1, -1), new Vector2(-1, -1),

                new Vector2(0, 1), new Vector2(0, 2),
                new Vector2(1, 1), new Vector2(-1, 1)
            };


            if (turn == 0) {

                for (int i = 2; i < 4; i++) {
                    var attackSquares = pos + displacements[i];
                    var moveIndex = (int)(attackSquares.Y * 8 + attackSquares.X);

                    if (!withinBounds(attackSquares)) continue;
                    if (board[moveIndex] == 0) continue;

                    calculatedMoves.Add(moveIndex);
                    Console.WriteLine("Calculated attack moves for pawn");
                }


                if (attackMovesOnly) return calculatedMoves;


                for (int i = 0; i < 2; i++) {
                    var potentialMove = pos + displacements[i];
                    var moveIndex = (int)(potentialMove.Y * 8 + potentialMove.X);

                    // Check Edge bounds
                    if (!withinBounds(potentialMove))
                        continue;

                    // Check for blocks.
                    if (board[moveIndex] != 0)
                        continue;

                    if (!startingPawnPositions.Contains(index)) {
                        calculatedMoves.Add(moveIndex);
                        break;
                    }
                    calculatedMoves.Add(moveIndex);
                }
            }
            else {

                for (int i = 2; i < 4; i++) {
                    var attackSquares = pos + displacements[i];
                    var moveIndex = (int)(attackSquares.Y * 8 + attackSquares.X);

                    if (!withinBounds(attackSquares)) continue;
                    if (board[moveIndex] == 0) continue;

                    calculatedMoves.Add(moveIndex);
                }

                if (attackMovesOnly) return calculatedMoves;

                for (int i = 4; i < 6; i++) {
                    var potentialMove = pos + displacements[i];
                    var moveIndex = (int)(potentialMove.Y * 8 + potentialMove.X);

                    // Check Edge bounds
                    if (!withinBounds(potentialMove))
                        continue;

                    // Check for blocks.
                    if (board[moveIndex] != 0)
                        continue;

                    if (!startingPawnPositions.Contains(index)) {
                        calculatedMoves.Add(moveIndex);
                        break;
                    }
                    calculatedMoves.Add(moveIndex);
                }
            }
            return calculatedMoves;
        }



        private List<int> knight(int index, byte[] board) {

            var row = index / 8;
            var col = index % 8;

            var pos = new Vector2(col, row);

            var calculatedMoves = new List<int>();

            var displacements = new Vector2[8] {
                new Vector2(-1, -2), new Vector2(1, -2),
                new Vector2(2, -1),new Vector2(2, 1),
                new Vector2(1, 2),new Vector2(-1, 2),
                new Vector2(-2, 1), new Vector2(-2, -1),
            };


            foreach (var displacement in displacements) {
                var potentialMove = pos + displacement;
                var moveIndex = (int)(potentialMove.Y * 8 + potentialMove.X);

                // Check Edge bounds
                if (!withinBounds(potentialMove))
                    continue;

                // Check same sides.
                if (board[moveIndex] != 0)
                    if (!isEnemy(board[index], board[moveIndex]))
                        continue;

                calculatedMoves.Add(moveIndex);
            }
            return calculatedMoves;
        }


        private List<int> Diagonal(int index, byte[] board, bool isAllAxis) {

            var displacements = new Vector2[4] {
                new Vector2(1, -1), new Vector2(1, 1),
                new Vector2(-1, 1), new Vector2(-1, -1)
            };

            return calculateAxisMoves(index, displacements, board, isAllAxis);
        }


        private List<int> Cross(int index, byte[] board, bool isAllAxis) {
            Console.WriteLine("Calculating for cross moves");
            var displacements = new Vector2[4] {
                new Vector2(0, -1), new Vector2(1, 0),
                new Vector2(0, 1), new Vector2(-1, 0)
            };

            return calculateAxisMoves(index, displacements, board, isAllAxis);
        }

        private List<int> King(int index, byte[] board) {
            var moves = Cross(index, board, false);
            moves.Concat(Diagonal(index, board, false));

            return moves;
        }






//------------------------ Private Useful Methods ---------------------------------------

        /// <summary>
        /// Calculates vertical, horizontal and diagonal axis moves. 
        /// </summary>
        /// <param name="index">The Index of the piece based on its position in the byteboard</param>
        /// <param name="displacements">Movement Patterns for the piece in directional Vector2</param>
        /// <param name="board">The byteboard reference</param>
        /// <param name="isLong">Whether this piece movement walks the whole board</param>

        private List<int> calculateAxisMoves(int index, Vector2[] displacements, byte[] board, bool isLong) {
            var row = index / 8;
            var col = index % 8;

            var pos = new Vector2(col, row);

            var moveIndexes = new List<int>();

            var increment = 1;
            foreach (var displacement in displacements) {
                while (true) {
                    var potentialMove = pos + displacement * increment;
                    var moveIndex = (int)(potentialMove.Y * 8 + potentialMove.X);

                    // Check Edge bounds
                    if (!withinBounds(potentialMove))
                        break;

                    // Check same sides or enemy.
                    if (board[moveIndex] != 0) {
                        if (!isEnemy(board[index], board[moveIndex]))
                            break;
                        else {
                            moveIndexes.Add(moveIndex);
                            break;
                        }
                    }

                    moveIndexes.Add(moveIndex);

                    increment += 1;
                    if (!isLong) {
                        break;
                    }
                }
                increment = 1;
            }
            return moveIndexes;
        }


        private void AddMoves(int pieceIndex, int moveIndex) {
            if (!moves.ContainsKey(pieceIndex))
                moves[pieceIndex] = new List<int>();

            moves[pieceIndex].Add(moveIndex);
        }

        private void AddMoves(int pieceIndex, List<int> moveIndexs) {
            if (moveIndexs.Count < 1)
                return;

            if (!moves.ContainsKey(pieceIndex))
                moves[pieceIndex] = moveIndexs;

            else
                moves[pieceIndex].Concat(moveIndexs);
        }


        private static bool withinBounds(Vector2 potentialMove) {
            if (potentialMove.X < 0 || potentialMove.Y < 0)
                return false;
            if (potentialMove.X > 7 || potentialMove.Y > 7)
                return false;
            return true;
        }


        private static bool isEnemy(byte selfPieceTypeID, byte targetPieceTypeID) {
            return (selfPieceTypeID & 8) != (targetPieceTypeID & 8) && targetPieceTypeID != 0;
        }
    }
}