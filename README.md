# Chess
UNDER CONSTRUCTION.

Using reference: https://www.chess.com/terms/chess-notation



# Design patterns used:
This chess game app will be based on typical MVC design, separating the game logics and the UI. 



# Game Mechanics design:
The data structure for the chess board will be a simple 1 dimension array size: 64.
I avoided using 2D array as it may incur performance penalties when doing legal move checks.

## Data structure
Currently the Chess game uses a ChessGameState data class to store the current state of the game. This helps separates the data objects form the methods, for cleaner code. 

## Moves validation methods.
This implementation uses a combination of bitwise operations and "IF id = ???" to calculate the movement of the pieces. Frist, Pseudo legal moves are calculated for the current turn. Then an opposition's attacking squares for each possible psuedo-move's outcome are calculated. IF the attacking squares contains the current turn's king position, it is a check, and this pseudo-legal move is removed from the list.

<br>
<br>

---
# Future Plans.
Mobile release with MAUI.
Web Application release. 
Plug In for chess AI. 
