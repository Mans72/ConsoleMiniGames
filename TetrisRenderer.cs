using MiniGames_con;
using System;
using System.Drawing;

namespace MiniGames_con {

	internal partial class Program {
		private static readonly int WindowHeight = h + 2;
		private static readonly int WindowWidth = sidebar + w * 2 + 4;
		private const int sidebar = 17;
		private static bool selectedRendered;

		private static void Initialization() {
			Console.Title = "Tetris";
			Console.CursorVisible = false;
			Console.SetWindowSize(WindowWidth, WindowHeight);
			//Console.SetBufferSize(WindowWidth, WindowHeight);
			Console.Clear();
			Thread.Sleep(400);
			RenderSidebar();
			RenderOutline();
		}

		private static void RenderSidebar() {
			for (int i = 0; i < WindowHeight; i++) {
				Console.SetCursorPosition(0, i);

				if (i < 5)
					Sources.SetColor(ConsoleColor.DarkGray, ConsoleColor.Black);
				else
					Sources.SetColor(ConsoleColor.Gray, ConsoleColor.Black);

				if (i == 2) Console.Write(Sources.CentereString("E X I T", sidebar));
				else if (i == 7) Console.Write(Sources.CentereString("NEXT FIGURE", sidebar));
				else Console.Write(new string(' ', sidebar));
			}
			Console.ResetColor();
		}
		private static void RenderOutline() {
			for (int i = 0; i < WindowHeight; i++) {
				Console.SetCursorPosition(sidebar, i);
				for (int j = 0; j < w + 2; j++) {
					Console.BackgroundColor = (i == 0 || i == WindowHeight - 1 || j == 0 || j == w + 1) ? ConsoleColor.Gray : ConsoleColor.Black;
					Console.Write("  ");
				}
			}
			Console.ResetColor();
		}
		private static void RenderNextTetromindo(Tetromino tetromino) {
			for (int i = 0; i < 4; i++) {
				Console.SetCursorPosition(4, 9 + i);
				for (int j = 0; j < 4; j++) {
					if (tetromino.IsIn(j + 5, i))
						Console.BackgroundColor = tetromino.Color;
					else
						Console.BackgroundColor = ConsoleColor.Gray;

					Console.Write("  ");
				}
			}
			Console.ResetColor();
		}

		private static void RenderBoard(ConsoleColor[,] grid) {
			for (int i = 0; i < h; i++) {
				Console.SetCursorPosition(sidebar + 2, i + 1);
				for (int j = 0; j < w; j++) {
					Console.BackgroundColor = grid[i, j];
					Console.Write("  ");
				}
			}
			Console.ResetColor();
		}
		private static void RenderTetramindo(XY[] shape, ConsoleColor color) {
			Console.BackgroundColor = color;
			foreach (XY pos in shape) {
				Console.SetCursorPosition(pos.X * 2, pos.Y);
				Console.Write("  ");
			}
			Console.ResetColor();
		}
		private static void RenderLineRemove(int y) {
			Console.SetCursorPosition(sidebar + 2, y + 1);
			Console.BackgroundColor = ConsoleColor.Black;
			for (int j = 0; j < w; j++)
				Console.Write("  ");
			Console.ResetColor();
		}



	}
}