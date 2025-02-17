using Raylib_cs;
using System.Numerics;

namespace Chess.Pieces;

class Pawn : Piece
{
	bool hasMoved;

	public Pawn(Vector2 posInWorld, bool color)
	{
		this.posInWorld = posInWorld;
		this.color = color;

		hasMoved = false;

		if (color)
			texture = Raylib.LoadTexture("./Textures/Pieces/White/Pawn_White.png");
		else
			texture = Raylib.LoadTexture("./Textures/Pieces/Black/Pawn_Black.png");
	}

	public override void CalculatePossibleMoves()
	{
		possibleMoves.Clear();

		Vector2 boardPos = Board.WorldToBoard(posInWorld);
		int currentX = (int)boardPos.X;
		int currentY = (int)boardPos.Y;

		Vector2 targetPos;

		if (color)
		{
			if (currentY == 0) return;

			CheckForTakes();

			targetPos = new Vector2(currentX, currentY - 1);
			string targetKey1 = Board.GetCoordinatesFromPosition((int)targetPos.X, (int)targetPos.Y);
			if (!Board.board.ContainsKey(targetKey1))
				possibleMoves.Add(targetPos);
			else return;

			if (!hasMoved)
			{
				targetPos = new Vector2(currentX, currentY - 2);
				string targetKey = Board.GetCoordinatesFromPosition((int)targetPos.X, (int)targetPos.Y);
				if (!Board.board.ContainsKey(targetKey)) 
					possibleMoves.Add(targetPos);
			}
		}
		else
		{
			if (currentY == Board.rows - 1) return;

			CheckForTakes();

			targetPos = new Vector2(currentX, currentY + 1);
			string targetKey2 = Board.GetCoordinatesFromPosition((int)targetPos.X, (int)targetPos.Y);
			if (!Board.board.ContainsKey(targetKey2)) 
				possibleMoves.Add(targetPos);
			else return;

			if (!hasMoved)
			{
				targetPos = new Vector2(currentX, currentY + 2);
				string targetKey = Board.GetCoordinatesFromPosition((int)targetPos.X, (int)targetPos.Y);
				if (!Board.board.ContainsKey(targetKey)) 
					possibleMoves.Add(targetPos);
			}
		}
	}

	void CheckForTakes()
	{
		Vector2 boardPos = Board.WorldToBoard(posInWorld);

		List<Vector2> captureOffsets = color
			? new List<Vector2> { new Vector2(-1, -1), new Vector2(1, -1) }
			: new List<Vector2> { new Vector2(-1, 1), new Vector2(1, 1) };

		foreach (Vector2 offset in captureOffsets)
		{
			Vector2 targetPos = boardPos + offset;

			if (targetPos.X >= 0 && targetPos.X < Board.cols &&
				targetPos.Y >= 0 && targetPos.Y < Board.rows)
			{
				string targetKey = Board.GetCoordinatesFromPosition((int)targetPos.X, (int)targetPos.Y);

				if (Board.board.ContainsKey(targetKey))
				{
					Piece targetPiece = Board.board[targetKey];
					if (targetPiece.color != color) 
						possibleMoves.Add(targetPos);
				}
			}
		}
	}

	public override void Draw()
	{
		Vector2 boardPos = Board.WorldToBoard(posInWorld);
		Vector2 worldPos = Board.BoardToWorld(boardPos);

		Raylib.DrawTextureV(texture, new Vector2(worldPos.X - texture.Width / 2, worldPos.Y - texture.Height / 2), Color.White);
	}

	public override void DrawMoves()
	{
		foreach (Vector2 move in possibleMoves)
		{
			Raylib.DrawCircleV(Board.BoardToWorld(move), 10, Color.Black);
		}
	}

	public override void UnloadTexture()
	{
		Raylib.UnloadTexture(texture);
	}

	public void SetHasMoved()
	{
		hasMoved = true;
	}
}
