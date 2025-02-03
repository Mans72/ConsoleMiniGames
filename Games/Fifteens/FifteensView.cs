using System.ComponentModel;

namespace ConsoleMiniGames.Games.Fifteens {

	internal class FifteensView : BaseView {
		public const int sidebar = 13;
		public int Sidebar { get => sidebar; }

		public FifteensView() {
			Title = "Fifteens";
			windowHeight = 19;
			windowWidth = 51;
			Initialization();
		}


		protected override void Initialization() {
			ConfigureWindow();
			RenderSidebar();
			RenderTime(0);
			RenderOutline();
		}


		public void RenderBoard(Board b) {
			lock (consoleLock) {
				for (int i = 0; i < 4; i++)
					for (int j = 0; j < 4; j++)
						RenderElement(b, false, i, j);
			}
		}

		public void RenderTime(int seconds) {
			lock (consoleLock) {
				Sources.SetColor(ConsoleColor.Gray, ConsoleColor.Black);
				Console.SetCursorPosition(0, 9);
				Console.Write(Sources.CentereString(Sources.FormatTime(seconds), sidebar));
				Console.ResetColor();
			}
		}

		public void RenderEndMessage() {
			Console.BackgroundColor = ConsoleColor.DarkGray;
			for (int i = 0; i < 7; i++) {
				Console.SetCursorPosition(0, windowHeight - 7 + i);
				for (int j = 0; j < sidebar; j++) {
					Console.BackgroundColor = winBox[i, j] == 1 ? ConsoleColor.DarkGreen : ConsoleColor.DarkGray;
					Console.Write(" ");
				}
			}
			Console.ResetColor();
		}

		public void RenderElement(Board b, bool C, int y, int x) {
			lock (consoleLock) {
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
		}
		public void RenderSelected(object? sender, PropertyChangedEventArgs e) {
			if (sender is Game game && e.PropertyName == nameof(game.Current)) {
				RenderElement(game.Current, true);
			}
		}

		private void RenderElement(Cell cell, bool selected) {
			lock (consoleLock) {
				int x = cell.X * 8 + sidebar + 4;
				int y = cell.Y * 4 + 2;

				if (cell.Number != 0) {
					for (int i = 0; i < 3; i++) {
						Console.SetCursorPosition(x, y + i);
						for (int j = 0; j < 3; j++) {
							if (cell.IsMoveable && selected) Console.BackgroundColor = ConsoleColor.Green;
							else if (cell.IsMoveable) Console.BackgroundColor = ConsoleColor.DarkGreen;
							else if (selected) Console.BackgroundColor = ConsoleColor.Red;
							else Console.BackgroundColor = ConsoleColor.DarkRed;
							Console.Write("  ");
						}
					}
					Console.BackgroundColor = ConsoleColor.Blue;
					Console.SetCursorPosition(x + 2, y + 1);
					Console.Write($"{cell.Number,2}");
					Console.BackgroundColor = ConsoleColor.Black;
				}
				else if (cell.Number == 0 && selected) {
					UnRenderElement(x, y);
					Console.BackgroundColor = ConsoleColor.Blue;
					Console.SetCursorPosition(x + 2, y + 1);
					Console.Write("  ");
					Console.BackgroundColor = ConsoleColor.Black;
				}
				else UnRenderElement(x, y);
				Console.ResetColor();
			}
		}

		private void RenderOutline() {
			for (int i = 0; i < windowHeight; i++) {
				Console.SetCursorPosition(sidebar, i);
				for (int j = 0; j < windowHeight; j++) {
					Console.BackgroundColor = i == 0 || i == windowHeight - 1 || j == 0 || j == windowHeight - 1 ? ConsoleColor.Gray : ConsoleColor.Black;
					Console.Write("  ");
				}
			}
			Console.ResetColor();
		}

		private void RenderSidebar() {
			for (int i = 0; i < windowHeight; i++) {
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

		private void UnRenderElement(int x, int y) {
			lock (consoleLock) {
				for (int i = 0; i < 3; i++) {
					Console.SetCursorPosition(x, y + i);
					for (int j = 0; j < 3; j++) {
						Console.BackgroundColor = ConsoleColor.Black;
						Console.Write("  ");
					}
				}
				Console.ResetColor();
			}
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

