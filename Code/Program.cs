using Raylib_cs;
using Chess.Pieces;

namespace Chess;

class MainClass
{
	static void Main()
	{
		Raylib.InitWindow(800, 800, "Chess");
		Board board = new Board();

		while (!Raylib.WindowShouldClose())
		{
			board.HandleMouseInput();

			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.Black);
			board.Draw();
			Raylib.EndDrawing();
		}

		foreach (Piece piece in Board.board.Values)
		{
			piece.UnloadTexture();
		}
	}
}
