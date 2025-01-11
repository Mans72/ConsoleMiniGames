using ConsoleMiniGames.Types;

namespace ConsoleMiniGames {

	internal static partial class SeaBattle {
		private static int WindowHeight = 30;
		private static int WindowWidth = 43;
		private static XY leftFP = new(2, 1);
		private static XY rightFP = new(44, 1);
		private const string msg1 = "Select Placing Method:";
		private const string msg2 = "Auto";
		private const string msg3 = "Manual";

		private static void Initialization() {
			Console.Title = "Sea Battle";
			Console.CursorVisible = false;
			Console.SetWindowSize(WindowWidth * 2, WindowHeight);
			//Console.SetBufferSize(WindowWidth * 2, WindowHeight);
			Console.Clear();
			Console.ResetColor();
			Thread.Sleep(400);
		}

		private static void RenderBoard(int[,] grid1, int[,] grid2) {
			RenderField(grid1, true);
			RenderField(grid2, false);
		}
		private static void RenderField(int[,] grid, bool left) {
			for (int i = 0; i < 20; i++) {
				Console.SetCursorPosition(left ? leftFP.X : rightFP.X, (left ? leftFP.Y : rightFP.Y) + i);
				for (int j = 0; j < 10; j++) {
					switch ((Cell)grid[i / 2, j]) {
						case Cell.Sea:
							Sources.SetColor((i / 2 + j) % 2 == 0 ? ConsoleColor.DarkCyan : ConsoleColor.Blue); break;
						case Cell.Miss:
							Sources.SetColor((i / 2 + j) % 2 == 0 ? ConsoleColor.DarkCyan : ConsoleColor.Blue, ConsoleColor.Cyan); break;
						case Cell.Boat:
							Sources.SetColor(ConsoleColor.Gray); break;
						case Cell.Hit:
							Sources.SetColor(ConsoleColor.DarkGray, ConsoleColor.DarkRed); break;
						default:
							Sources.SetColor(ConsoleColor.DarkCyan); break;
					}
					Console.Write(i % 2 == 0 ? " \\/ " : " /\\ ");
				}
			}
			Console.ResetColor();
		}
		private static void RenderEndMessage(bool status) {
			Thread.Sleep(300);
			for (int i = 0; i < 7; i++) {
				Console.SetCursorPosition(6, 7 + i);
				for (int j = 0; j < 32; j++) {
					Console.BackgroundColor = status ? (win[i, j] == 1 ? ConsoleColor.Green : ConsoleColor.Gray) : (loss[i, j] == 1 ? ConsoleColor.Red : ConsoleColor.Gray);
					Console.Write(" ");
				}
				Console.SetCursorPosition(48, 7 + i);
				for (int j = 0; j < 32; j++) {
					Console.BackgroundColor = status ? (loss[i, j] == 1 ? ConsoleColor.Red : ConsoleColor.Gray) : (win[i, j] == 1 ? ConsoleColor.Green : ConsoleColor.Gray);
					Console.Write(" ");
				}
				Console.SetCursorPosition(44, 22 + i);
				for (int j = 0; j < 40; j++) {
					Console.BackgroundColor = again[i, j] == 1 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
					Console.Write(" ");
				}
			}
			Console.ResetColor();
		}

		private static void RenderPlacing(XY pos, int length, bool horizontal, bool[] place) {
			for (int i = 0; i < length; i++) {
				for (int j = 0; j < 2; j++) {
					if (horizontal)
						Console.SetCursorPosition(leftFP.X + (pos.Y + i) * 4, leftFP.Y + pos.X * 2 + j);
					else
						Console.SetCursorPosition(leftFP.X + pos.Y * 4, leftFP.Y + (pos.X + i) * 2 + j);

					Sources.SetColor(place[i] ? ConsoleColor.DarkGray : ConsoleColor.Red, place[i] ? ConsoleColor.Gray : ConsoleColor.DarkRed);
					Console.Write(j == 0 ? " \\/ " : " /\\ ");
				}
			}
		}
		private static void RenderSelectingCoords(int[,] enemyGrid, XY pos) {
			switch ((Cell)enemyGrid[pos.X, pos.Y]) {
				case Cell.Sea:
					Sources.SetColor(ConsoleColor.Red, ConsoleColor.Red); break;
				case Cell.Miss:
					Sources.SetColor(ConsoleColor.DarkRed, ConsoleColor.Red); break;
				case Cell.Hit:
					Sources.SetColor(ConsoleColor.DarkRed, ConsoleColor.Red); break;
			}
			for (int i = 0; i < 2; i++) {
				Console.SetCursorPosition(rightFP.X + pos.Y * 4, rightFP.Y + pos.X * 2 + i);
				Console.Write(i == 0 ? " \\/ " : " /\\ ");
			}
		}

		private static void RenderBotHits(XY pos, bool hit) {
			Sources.SetColor(hit ? ConsoleColor.Red : ConsoleColor.Yellow, hit ? ConsoleColor.DarkRed : ConsoleColor.DarkYellow);
			for (int i = 0; i < 2; i++) {
				Console.SetCursorPosition(leftFP.X + pos.Y * 4, leftFP.Y + pos.X * 2 + i);
				Console.Write(i == 0 ? " \\/ " : " /\\ ");
			}
			Thread.Sleep(500);
		}
		private static void RenderBotHits(bool rightBot, XY pos) {
			for (int i = 0; i < 2; i++) {
				if (rightBot) Console.SetCursorPosition(leftFP.X + pos.Y * 4, leftFP.Y + pos.X * 2 + i);
				else Console.SetCursorPosition(rightFP.X + pos.Y * 4, rightFP.Y + pos.X * 2 + i);
				Sources.SetColor(ConsoleColor.Yellow, ConsoleColor.DarkYellow);
				Console.Write(i == 0 ? " \\/ " : " /\\ ");
			}
			Thread.Sleep(500);
		}

		private static void RenderOutline() {
			for (int i = 0; i < WindowHeight; i++) {
				Console.SetCursorPosition(0, i);
				for (int j = 0; j < WindowWidth; j++) {
					if (i == 0 || i == 21 || i == WindowHeight - 1 || j == 0 || j == WindowWidth / 2 || j == WindowWidth - 1)
						Console.BackgroundColor = ConsoleColor.DarkBlue;
					else Console.BackgroundColor = ConsoleColor.Black;
					Console.Write("  ");
				}
			}
		}
		private static void RenderEmptyFields() {
			RenderOutline();

			int[,] grid = new int[10, 10];
			for (int i = 0; i < 10; i++)
				for (int j = 0; j < 10; j++)
					grid[i, j] = 0;
			RenderField(grid, true);
			RenderField(grid, false);

			for (int i = 0; i < 7; i++) {
				Console.SetCursorPosition(2, 22 + i);
				for (int j = 0; j < 40; j++) {
					Console.BackgroundColor = exit[i, j] == 1 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
					Console.Write(" ");
				}
			}

			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;
			for (int i = 0; i < 7; i++) {
				Console.SetCursorPosition(44, 22 + i);
				if (i == 1) Console.Write(Sources.CentereString(msg1, 40));
				else if (i == 4) {
					Console.Write(Sources.CentereString(msg2, 20));
					Console.Write(Sources.CentereString(msg3, 20));
				}
				else Console.Write(new string(' ', 40));
			}

			Console.ResetColor();
		}
		private static void RenderSelectionSelected(bool s, int o) {
			Console.BackgroundColor = s ? ConsoleColor.DarkGray : ConsoleColor.Gray;
			Console.ForegroundColor = s ? ConsoleColor.Gray : ConsoleColor.Black;
			for (int i = 0; i < 3; i++) {
				Console.SetCursorPosition(o == 0 ? 47 : 67, 25 + i);
				if (i == 1) Console.Write(Sources.CentereString(o == 0 ? msg2 : msg3, 14));
				else Console.Write(new string(' ', 14));
			}
			Console.ResetColor();
		}

		private static readonly int[,] win = new int[7, 32] {
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,0,0,0,1,1,0,1,1,1,1,1,1,0,1,1,0,0,1,1,0,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,0,0,0,1,1,0,0,0,1,1,0,0,0,1,1,1,0,1,1,0,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,0,1,0,1,1,0,0,0,1,1,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,1,1,0,0,0,1,1,0,1,1,1,0,0,0,0,0,0 },
			{ 0,0,0,0,0,0,1,1,0,1,1,0,0,1,1,1,1,1,1,0,1,1,0,0,1,1,0,0,0,0,0,0 },
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
		};
		private static readonly int[,] loss = new int[7, 32] {
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
			{ 0,0,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0,0 },
			{ 0,0,1,1,0,0,0,0,0,1,1,0,0,1,1,0,1,1,0,0,0,0,0,1,1,0,0,0,0,0,0,0 },
			{ 0,0,1,1,0,0,0,0,0,1,1,0,0,1,1,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0,0 },
			{ 0,0,1,1,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,1,1,0,0,0,0,0,1,1,0,0,0 },
			{ 0,0,1,1,1,1,1,1,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0,0 },
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
		};
		private static readonly int[,] exit = new int[7, 40] {
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,1,1,1,1,0,0,1,1,0,0,1,1,0,0,1,1,1,1,1,1,0,0,1,1,1,1,1,1,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,1,1,1,1,0,0,1,1,0,0,1,1,0,0,1,1,1,1,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0 },
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
		};
		private static readonly int[,] again = new int[7, 40] {
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
			{ 0,0,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,1,1,1,1,1,1,0,1,1,0,0,1,1,0,0,0 },
			{ 0,0,0,1,1,0,0,1,1,0,1,1,0,0,0,0,0,1,1,0,0,1,1,0,0,0,1,1,0,0,0,1,1,1,0,1,1,0,0,0 },
			{ 0,0,0,1,1,1,1,1,1,0,1,1,0,1,1,1,0,1,1,1,1,1,1,0,0,0,1,1,0,0,0,1,1,0,1,1,1,0,0,0 },
			{ 0,0,0,1,1,0,0,1,1,0,1,1,0,0,1,1,0,1,1,0,0,1,1,0,0,0,1,1,0,0,0,1,1,0,0,1,1,0,0,0 },
			{ 0,0,0,1,1,0,0,1,1,0,0,1,1,1,1,0,0,1,1,0,0,1,1,0,1,1,1,1,1,1,0,1,1,0,0,1,1,0,0,0 },
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
		};
	}
}

