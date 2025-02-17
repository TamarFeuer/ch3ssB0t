using Raylib_cs;
using System.Numerics;

namespace Chess.Pieces;

abstract class Piece
{
	public Texture2D texture;
	public Vector2 posInWorld;
	public List<Vector2> possibleMoves = new List<Vector2>();

	public bool color;

	public abstract void CalculatePossibleMoves();
	public abstract void Draw();
	public abstract void DrawMoves();
	public abstract void UnloadTexture();
}