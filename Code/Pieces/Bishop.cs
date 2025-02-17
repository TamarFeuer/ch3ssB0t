using Raylib_cs;
using System.Numerics;

namespace Chess.Pieces;

class Bishop : Piece
{
	public Bishop(Vector2 posInWorld, bool color)
	{
		this.posInWorld = posInWorld;
		this.color = color;

		if (color)
			texture = Raylib.LoadTexture("./Textures/Pieces/White/Bishop_White.png");
		else
			texture = Raylib.LoadTexture("./Textures/Pieces/Black/Bishop_Black.png");
	}

	public override void CalculatePossibleMoves()
	{
		possibleMoves.Clear();

		Vector2 posOnBoard = Board.WorldToBoard(posInWorld);
		List<Vector2> directions = new List<Vector2>
		{
			new Vector2(-1, -1), new Vector2(1, 1),
			new Vector2(-1, 1), new Vector2(1, -1)
		};

		foreach (Vector2 direction in directions)
		{
			Vector2 target = posOnBoard + direction;

			while (target.X >= 0 && target.X < 8 && target.Y >= 0 && target.Y < 8)
			{
				string targetKey = Board.GetCoordinatesFromPosition((int)target.X, (int)target.Y);

				if (Board.board.ContainsKey(targetKey))
				{
					if (Board.board[targetKey].color != color)
						possibleMoves.Add(target);

					break;
				}

				possibleMoves.Add(target);
				target += direction;
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