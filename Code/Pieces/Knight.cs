using Raylib_cs;
using System.Numerics;

namespace Chess.Pieces;

class Knight : Piece
{
	public Knight(Vector2 posInWorld, bool color)
	{
		this.posInWorld = posInWorld;
		this.color = color;

		if (color)
			texture = Raylib.LoadTexture("./Textures/Pieces/White/Knight_White.png");
		else
			texture = Raylib.LoadTexture("./Textures/Pieces/Black/Knight_Black.png");
	}

	public override void CalculatePossibleMoves()
	{
		possibleMoves.Clear();
		Vector2 posOnBoard = Board.WorldToBoard(posInWorld);

		List<Vector2> moves = new List<Vector2>
		{
			new Vector2(-2, -1), new Vector2(-2, 1),
			new Vector2(2, -1), new Vector2(2, 1),
			new Vector2(-1, -2), new Vector2(1, -2),
			new Vector2(-1, 2), new Vector2(1, 2)
		};

		foreach (Vector2 move in moves)
		{
			Vector2 target = posOnBoard + move;
			if (target.X >= 0 && target.X < 8 && target.Y >= 0 && target.Y < 8)
			{
				string targetKey = Board.GetCoordinatesFromPosition((int)target.X, (int)target.Y);

				if (!Board.board.ContainsKey(targetKey) || Board.board[targetKey].color != color)
					possibleMoves.Add(target);
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
}
