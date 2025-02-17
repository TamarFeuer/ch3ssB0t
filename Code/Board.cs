using Raylib_cs;
using System.Numerics;
using Chess.Pieces;

namespace Chess;

class Board
{
	public static Dictionary<string, Piece> board = new Dictionary<string, Piece>();
	Piece selectedPiece;

	public static int rows = 8, cols = 8;
	static int tileSize = 100;

	public Board()
	{
		LoadFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
	}

	public void HandleMouseInput()
	{
		if (Raylib.IsMouseButtonPressed(MouseButton.Left))
		{
			Vector2 mousePos = Raylib.GetMousePosition();
			Vector2 clickedBoardPos = WorldToBoard(mousePos);
			string clickedPosition = GetCoordinatesFromPosition((int)clickedBoardPos.X, (int)clickedBoardPos.Y);

			if (selectedPiece != null)
			{
				foreach (Vector2 move in selectedPiece.possibleMoves)
				{
					string movePosition = GetCoordinatesFromPosition((int)move.X, (int)move.Y);
					if (movePosition == clickedPosition)
					{
						MovePiece(selectedPiece, move);
						selectedPiece = null;
						return;
					}
				}
			}

			if (board.TryGetValue(clickedPosition, out Piece clickedPiece))
			{
				if (selectedPiece == clickedPiece)
					selectedPiece = null;
				else
				{
					selectedPiece = clickedPiece;
					selectedPiece.CalculatePossibleMoves();
				}
			}
			else
				selectedPiece = null;
		}
	}

	void MovePiece(Piece piece, Vector2 newBoardPos)
	{
		string oldPosition = GetCoordinatesFromPosition((int)WorldToBoard(piece.posInWorld).X, (int)WorldToBoard(piece.posInWorld).Y);
		string newPosition = GetCoordinatesFromPosition((int)newBoardPos.X, (int)newBoardPos.Y);

		if (board.TryGetValue(newPosition, out Piece capturedPiece))
			board.Remove(newPosition);

		board.Remove(oldPosition);
		board[newPosition] = piece;

		piece.posInWorld = BoardToWorld(newBoardPos);

		foreach (Piece pieceToUpdate in board.Values)
		{
			pieceToUpdate.CalculatePossibleMoves();
		}

		if (piece is Pawn movedPawn)
			movedPawn.SetHasMoved();
	}

	public static Vector2 WorldToBoard(Vector2 worldPosition)
	{
		int col = (int)(worldPosition.X / tileSize);
		int row = (int)(worldPosition.Y / tileSize);
		return new Vector2(col, row);
	}

	public static Vector2 BoardToWorld(Vector2 boardPosition)
	{
		float worldX = boardPosition.X * tileSize + tileSize / 2;
		float worldY = boardPosition.Y * tileSize + tileSize / 2;
		return new Vector2(worldX, worldY);
	}

	public static string GetCoordinatesFromPosition(int x, int y)
	{
		return $"{(char)('A' + x)}{8 - y}";
	}

	public string GetFEN()
	{
		string fen = "";
		for (int row = 0; row < rows; row++)
		{
			int emptyCount = 0;
			for (int col = 0; col < cols; col++)
			{
				string position = GetCoordinatesFromPosition(col, row);
				if (board.TryGetValue(position, out Piece piece))
				{
					if (emptyCount > 0)
					{
						fen += emptyCount.ToString();
						emptyCount = 0;
					}
					fen += GetFENChar(piece);
				}
				else
					emptyCount++;
			}
			if (emptyCount > 0)
				fen += emptyCount.ToString();
			
			if (row < rows - 1)
				fen += "/";
		}
		return fen;
	}

	void LoadFromFEN(string fen)
	{
		board.Clear();
		
		string[] ranks = fen.Split('/');
		for (int row = 0; row < rows; row++)
		{
			int col = 0;
			foreach (char symbol in ranks[row])
			{
				if (char.IsDigit(symbol))
					col += symbol - '0';
				else
				{
					bool isWhite = char.IsUpper(symbol);
					Piece piece = CreatePiece(symbol, BoardToWorld(new Vector2(col, row)), isWhite);
					string position = GetCoordinatesFromPosition(col, row);
					board[position] = piece;
					col++;
				}
			}
		}
	}

	Piece CreatePiece(char symbol, Vector2 position, bool isWhite)
	{
		return symbol.ToString().ToLower() switch
		{
			"k" => new King(position, isWhite),
			"p" => new Pawn(position, isWhite),
			"n" => new Knight(position, isWhite),
			"b" => new Bishop(position, isWhite),
			"r" => new Rook(position, isWhite),
			"q" => new Queen(position, isWhite),
			_ => throw new ArgumentException($"Invalid piece symbol: {symbol}")
		};
	}

	char GetFENChar(Piece piece)
	{
		char c = piece switch
		{
			King => 'k',
			Pawn => 'p',
			Knight => 'n',
			Bishop => 'b',
			Rook => 'r',
			Queen => 'q',
			_ => '?'
		};
		return piece.color ? char.ToUpper(c) : c;
	}

	void DrawPieces()
	{
		foreach (Piece piece in board.Values)
		{
			piece.Draw();
			if (piece == selectedPiece)
				piece.DrawMoves();
		}
	}

	void DrawBoard()
	{
		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < cols; col++)
			{
				Color color = (row + col) % 2 == 0 ? new Color(243, 228, 213, 255) : new Color(12, 107, 148, 255);
				Raylib.DrawRectangle(col * tileSize, row * tileSize, tileSize, tileSize, color);
			}
		}
	}

	public void Draw()
	{
		DrawBoard();
		DrawPieces();
	}
}
