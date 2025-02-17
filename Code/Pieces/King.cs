using Raylib_cs;
using System.Numerics;

namespace Chess.Pieces;

class King : Piece
{
	public King(Vector2 posInWorld, bool color)
	{
		this.posInWorld = posInWorld;
		this.color = color;

		if (color)
			texture = Raylib.LoadTexture("./Textures/Pieces/White/King_White.png");
		else
			texture = Raylib.LoadTexture("./Textures/Pieces/Black/King_Black.png");
	}

	public override void CalculatePossibleMoves()
	{
		possibleMoves.Clear();

		Vector2 posOnBoard = Board.WorldToBoard(posInWorld);
		List<Vector2> offsets = new List<Vector2>
		{
			new Vector2(-1, 0), new Vector2(1, 0),
			new Vector2(0, -1), new Vector2(0, 1),
			new Vector2(-1, -1), new Vector2(1, 1),
			new Vector2(-1, 1), new Vector2(1, -1)
		};

		foreach (Vector2 offset in offsets)
		{
			Vector2 target = posOnBoard + offset;

			if (target.X >= 0 && target.X < 8 && target.Y >= 0 && target.Y < 8)
			{
				string targetKey = Board.GetCoordinatesFromPosition((int)target.X, (int)target.Y);

				if (!Board.board.ContainsKey(targetKey))
					possibleMoves.Add(target);
				else if (Board.board.TryGetValue(targetKey, out Piece targetPiece) && targetPiece.color != color)
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