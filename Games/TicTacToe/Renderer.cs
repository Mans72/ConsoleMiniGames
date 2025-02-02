namespace ConsoleMiniGames.Games.TicTacToe {

	internal static partial class TicTacToe {
		private const int size = 21;
		private const int sidebar = 17;
		private const int boxX = 10;
		private const int boxY = 7;

		private static void Initialization() {
			Console.Title = "TicTacToe";
			Console.CursorVisible = false;
			Console.SetWindowSize(size * 2 + sidebar, size);
			//Console.SetBufferSize(WindowWidth * 2, WindowHeight);
			Console.Clear();
			Console.ResetColor();
			Thread.Sleep(400);
			RenderSidebar();
			RenderScore(0, 0, 0);
			RenderOutline();
		}

		private static void DrawElement(char elem, int x, int y) {
			for (int i = 0; i < 5; i++) {
				Console.SetCursorPosition(sidebar + x * 12 + 4, y * 6 + 2 + i);
				for (int j = 0; j < 5; j++) {
					Console.BackgroundColor = ConsoleColor.Black;
					if (elem == 'X' && (i == j || (i + j) == 4))
						Console.BackgroundColor = ConsoleColor.Red;
					else if (elem == 'O' && ((i == 0 || i == 4 || j == 0 || j == 4) && (i != j) && (i + j != 4)))
						Console.BackgroundColor = ConsoleColor.Blue;
					Console.Write("  ");
				}
			}
			Console.ResetColor();
		}
		private static void RenderClear() {
			for (int i = 1; i < size - 1; i++) {
				Console.SetCursorPosition(sidebar + 2, i);
				for (int j = 1; j < size - 1; j++) {
					Console.Write("  ");
				}
			}
			Console.BackgroundColor = ConsoleColor.DarkGray;
			for (int i = 0; i < boxY; i++) {
				Console.SetCursorPosition(0, size - boxY + i);
				for (int j = 0; j < sidebar; j++) {
					Console.Write(" ");
				}
			}
			Console.ResetColor();
		}
		private static void RenderEndMessage(bool win, bool player) {
			Console.BackgroundColor = ConsoleColor.DarkGray;
			for (int i = 0; i < boxY; i++) {
				Console.SetCursorPosition(0, size - boxY + i);
				for (int j = 0; j < sidebar; j++) {
					if (j is >= 3 and < 13) {
						if (win)
							Console.BackgroundColor = player ? (winBox[i, j - 3] == 1 ? ConsoleColor.DarkGreen : ConsoleColor.DarkGray) : (lossBox[i, j - 3] == 1 ? ConsoleColor.DarkRed : ConsoleColor.DarkGray);
						else
							Console.BackgroundColor = drawBox[i, j - 3] == 1 ? ConsoleColor.Gray : ConsoleColor.DarkGray;
					}
					Console.Write(" ");
				}
			}
			Console.ResetColor();
		}
		private static void RenderWinLine(int winCase) {
			Console.BackgroundColor = ConsoleColor.Green;
			for (int i = 1; i < 20; i++) {
				switch (winCase) {
					case 1:
					case 2:
					case 3:
						Console.SetCursorPosition(sidebar + i * 2, 6 * winCase - 2);
						break;
					case 4:
					case 5:
					case 6:
						Console.SetCursorPosition(sidebar + 12 * winCase - 40, i);
						break;
					case 7:
						Console.SetCursorPosition(sidebar + i * 2, i);
						break;
					case 8:
						Console.SetCursorPosition(sidebar + (20 - i) * 2, i);
						break;
					default: break;
				}
				Console.Write("  ");
			}
			Console.ResetColor();
		}
		private static void RenderScore(int wins, int losses, int draws) {
			Console.BackgroundColor = ConsoleColor.Gray;
			for (int i = 0; i < 9; i++) {
				Console.SetCursorPosition(0, 5 + i);
				switch (i) {
					case 1:
						Console.ForegroundColor = ConsoleColor.Black;
						Console.Write("  SCORE COUNTER  ");
						break;
					case 3:
						Console.ForegroundColor = ConsoleColor.DarkGreen;
						Console.Write($"   WINS:   {wins,3}   ");
						break;
					case 5:
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write($"   LOSSES: {losses,3}   ");
						break;
					case 7:
						Console.ForegroundColor = ConsoleColor.DarkGray;
						Console.Write($"   DRAWS:  {draws,3}   ");
						break;
					default:
						Console.Write("                 ");
						break;
				}
			}
			Console.ResetColor();
		}

		// Background
		private static void RenderOutline() {
			for (int i = 0; i < size; i++) {
				Console.SetCursorPosition(sidebar, i);
				for (int j = 0; j < size; j++) {
					Console.BackgroundColor = (i == 0 || i == 20 || j == 0 || j == 20) ? ConsoleColor.Gray : ConsoleColor.Black;
					Console.Write("  ");
				}
			}
			Console.ResetColor();
		}
		private static void RenderSidebar() {
			Sources.SetColor(ConsoleColor.DarkGray, ConsoleColor.Black);
			for (int i = 0; i < size; i++) {
				Console.SetCursorPosition(0, i);
				if (i == 2) Console.Write(Sources.CentereString("E X I T", sidebar));
				else Console.Write(new string(' ', sidebar));
			}
			Console.ResetColor();
		}

		private static readonly int[,] winBox = new int[boxY, boxX] {
			{ 0,0,0,0,0,0,0,0,0,0 },
			{ 0,0,1,1,0,0,0,1,1,0 },
			{ 0,0,1,1,0,0,0,1,1,0 },
			{ 0,0,1,1,0,1,0,1,1,0 },
			{ 0,0,1,1,1,1,1,1,1,0 },
			{ 0,0,0,1,1,0,1,1,0,0 },
			{ 0,0,0,0,0,0,0,0,0,0 }
		};
		private static readonly int[,] lossBox = new int[boxY, boxX] {
			{ 0,0,0,0,0,0,0,0,0,0 },
			{ 0,0,1,1,0,0,0,0,0,0 },
			{ 0,0,1,1,0,0,0,0,0,0 },
			{ 0,0,1,1,0,0,0,0,0,0 },
			{ 0,0,1,1,0,0,0,0,0,0 },
			{ 0,0,1,1,1,1,1,1,0,0 },
			{ 0,0,0,0,0,0,0,0,0,0 }
		};
		private static readonly int[,] drawBox = new int[boxY, boxX] {
			{ 0,0,0,0,0,0,0,0,0,0 },
			{ 0,0,1,1,1,1,1,0,0,0 },
			{ 0,0,1,1,0,0,1,1,0,0 },
			{ 0,0,1,1,0,0,1,1,0,0 },
			{ 0,0,1,1,0,0,1,1,0,0 },
			{ 0,0,1,1,1,1,1,0,0,0 },
			{ 0,0,0,0,0,0,0,0,0,0 },
		};
	}
}

