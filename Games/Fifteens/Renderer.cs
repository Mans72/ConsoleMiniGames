namespace ConsoleMiniGames.Games.Fifteens {

	internal static class Renderer {
		private static readonly int WindowHeight = 19;
		private static readonly int WindowWidth = 51;
		public const int sidebar = 13;

		public static void Initialization() {
			Console.Title = "Fifteens";
			Console.CursorVisible = false;
			Console.SetWindowSize(WindowWidth, WindowHeight);
			//Console.SetBufferSize(WindowWidth, WindowHeight);
			Console.Clear();
			Console.ResetColor();
			Thread.Sleep(400);
			RenderSidebar();
			RenderTime(0);
			RenderOutline();
		}


		public static void RenderBoard(Board b) {
			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++)
					RenderElement(b, false, i, j);
		}
		public static void RenderOutline() {
			for (int i = 0; i < WindowHeight; i++) {
				Console.SetCursorPosition(sidebar, i);
				for (int j = 0; j < WindowHeight; j++) {
					Console.BackgroundColor = i == 0 || i == WindowHeight - 1 || j == 0 || j == WindowHeight - 1 ? ConsoleColor.Gray : ConsoleColor.Black;
					Console.Write("  ");
				}
			}
			Console.ResetColor();
		}
		public static void RenderSidebar() {
			for (int i = 0; i < WindowHeight; i++) {
				Console.SetCursorPosition(0, i);
				if (i is < 5 or > 11)
					Sources.SetColor(ConsoleColor.DarkGray, ConsoleColor.Black);
				else
					Sources.SetColor(ConsoleColor.Gray, ConsoleColor.Black);

				if (i == 2) Console.Write(Sources.CentereString("E X I T", sidebar));
				else if (i == 7) Console.Write(Sources.CentereString("T i m e", sidebar));
				else Console.Write(new string(' ', sidebar));
			}
			Console.ResetColor();
		}
		public static void RenderTime(int seconds) {
			Sources.SetColor(ConsoleColor.Gray, ConsoleColor.Black);
			Console.SetCursorPosition(0, 9);
			Console.Write(Sources.CentereString(Sources.FormatTime(seconds), sidebar));
			Console.ResetColor();
		}
		public static void RenderEndMessage() {
			Console.BackgroundColor = ConsoleColor.DarkGray;
			for (int i = 0; i < 7; i++) {
				Console.SetCursorPosition(0, WindowHeight - 7 + i);
				for (int j = 0; j < sidebar; j++) {
					Console.BackgroundColor = winBox[i, j] == 1 ? ConsoleColor.DarkGreen : ConsoleColor.DarkGray;
					Console.Write(" ");
				}
			}
			Console.ResetColor();
		}

		public static void RenderElement(Board b, bool C, int y, int x) {
			int N = b.Grid[y, x];
			bool M = b.IsMoveable(y, x);
			x = x * 8 + sidebar + 4;
			y = y * 4 + 2;

			if (N != 0) {
				for (int i = 0; i < 3; i++) {
					Console.SetCursorPosition(x, y + i);
					for (int j = 0; j < 3; j++) {
						if (M && C) Console.BackgroundColor = ConsoleColor.Green;
						else if (M) Console.BackgroundColor = ConsoleColor.DarkGreen;
						else if (C) Console.BackgroundColor = ConsoleColor.Red;
						else Console.BackgroundColor = ConsoleColor.DarkRed;
						Console.Write("  ");
					}
				}
				Console.BackgroundColor = ConsoleColor.Blue;
				Console.SetCursorPosition(x + 2, y + 1);
				Console.Write($"{N,2}");
				Console.BackgroundColor = ConsoleColor.Black;
			}
			else if (N == 0 && C) {
				UnRenderElement(x, y);
				Console.BackgroundColor = ConsoleColor.Blue;
				Console.SetCursorPosition(x + 2, y + 1);
				Console.Write("  ");
				Console.BackgroundColor = ConsoleColor.Black;
			}
			else UnRenderElement(x, y);
			Console.ResetColor();
		}
		private static void UnRenderElement(int x, int y) {
			for (int i = 0; i < 3; i++) {
				Console.SetCursorPosition(x, y + i);
				for (int j = 0; j < 3; j++) {
					Console.BackgroundColor = ConsoleColor.Black;
					Console.Write("  ");
				}
			}
			Console.ResetColor();
		}


		private static readonly int[,] winBox = new int[7, sidebar] {
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0 },
			{ 0,0,0,1,1,0,0,0,1,1,0,0,0 },
			{ 0,0,0,1,1,0,0,0,1,1,0,0,0 },
			{ 0,0,0,1,1,0,1,0,1,1,0,0,0 },
			{ 0,0,0,1,1,1,1,1,1,1,0,0,0 },
			{ 0,0,0,0,1,1,0,1,1,0,0,0,0 },
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0 }
		};
	}
}

