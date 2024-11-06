using MiniGames_con;
using System;

namespace MiniGames_con {

	internal partial class Program {
		private static readonly int WindowHeight = 4 * (1 + (games.Count + 2) / 3) + 1;
		private static readonly int WindowWidth = 56;
		private static bool selectedRendered;

		private static void Initialization() {
			Console.Title = "Mini Console Games";
			Console.CursorVisible = false;
			Console.SetWindowSize(WindowWidth, WindowHeight);
			//Console.SetBufferSize(WindowWidth, WindowHeight);
			Console.Clear();
			Console.ResetColor();
			Thread.Sleep(400);
		}

		private static void RenderMenu() {
			Console.Clear();
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;

			for (int i = 0; i < 3; i++) {
				Console.SetCursorPosition(2, 1 + i);
				if (i == 1) Console.Write(Sources.CentereString("Choose a game:", 52));
				else Console.Write(Sources.CentereString("", 52));
			}

			for (int g = 0; g < games.Count; g++) {
				for (int i = 0; i < 3; i++) {
					Console.SetCursorPosition(2 + g % 3 * 18, 5 + g / 3 * 4 + i);
					//Console.Write(i == 1 ? Sources.СentereString(games[g], 16) : "                ");
					Console.Write(i switch {
						0 => "╔══════════════╗",
						1 => $"║{Sources.CentereString(games[g], 14)}║",
						2 => "╚══════════════╝",
					});

					//  ╔ ═ ╗ ║ ╚ ╝
				}
			}
			Console.ResetColor();
		}
		private static void RenderSelected(int g) {
			selectedRendered = true;
			Console.BackgroundColor = ConsoleColor.DarkGray;
			Console.ForegroundColor = ConsoleColor.Gray;
			for (int i = 0; i < 3; i++) {
				Console.SetCursorPosition(2 + g % 3 * 18, 5 + g / 3 * 4 + i);
				//Console.Write(i == 1 ? Sources.СentereString(games[g], 16) : "                ");
				Console.Write(i switch {
					0 => "╔══════════════╗",
					1 => $"║{Sources.CentereString(games[g], 14)}║",
					2 => "╚══════════════╝",
				});
			}
			Console.ResetColor();
		}
		private static void RenderUnSelected(int g) {
			selectedRendered = false;
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;
			for (int i = 0; i < 3; i++) {
				Console.SetCursorPosition(2 + g % 3 * 18, 5 + g / 3 * 4 + i);
				//Console.Write(i == 1 ? Sources.СentereString(games[g], 16) : "                ");
				Console.Write(i switch {
					0 => "╔══════════════╗",
					1 => $"║{Sources.CentereString(games[g], 14)}║",
					2 => "╚══════════════╝",
				});
			}
			Console.ResetColor();
		}

	}



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



	internal static partial class MineSweeper {
		private static readonly int boxX = 7;
		private static readonly int boxY = 31;

		private static void Initialization() {
			Console.Title = "MineSweeper";
			Console.CursorVisible = false;
			Console.SetWindowSize(Sidebar + FieldCols * 6, FieldRows * 3);
			//Console.SetBufferSize(WindowWidth, WindowHeight);
			Console.Clear();
			Console.ResetColor();
			Thread.Sleep(400);
			RenderSidebar();
		}

		private static void RenderBoard(Cell[,] cells, int flagCount) {
			RenderFlagsCount(flagCount);
			for (int i = 0; i < FieldRows; i++)
				for (int j = 0; j < FieldCols; j++)
					RenderCell(cells[i, j], i, j);
		}
		private static void RenderEndMessage(bool status) {
			Thread.Sleep(status ? 500 : 500);
			for (int i = 0; i < boxX; i++) {
				Console.SetCursorPosition(Sidebar + (FieldCols + boxY) / 2, (FieldRows + boxX) / 2 + i);
				for (int j = 0; j < boxY; j++) {
					Console.BackgroundColor = status ? (win[i, j] == 1 ? ConsoleColor.Green : ConsoleColor.DarkGray) : (loss[i, j] == 1 ? ConsoleColor.Red : ConsoleColor.DarkGray);
					Console.Write(" ");
				}
			}
			Console.ResetColor();
			Thread.Sleep(1000);
		}

		private static void RenderCell(Cell cell, int x, int y) {
			if (!cell.IsRevealed) {
				for (int i = 0; i < 3; i++) {
					Console.SetCursorPosition(Sidebar + y * 6, x * 3 + i);
					for (int j = 0; j < 3; j++) {
						Console.BackgroundColor = (x + y) % 2 == 0 ? ConsoleColor.DarkGreen : ConsoleColor.Green;
						if (cell.IsFlagged && i == 1 && j == 1)
							Console.BackgroundColor = ConsoleColor.DarkRed;
						Console.Write("  ");
					}
				}
			}
			else {
				for (int i = 0; i < 3; i++) {
					Console.SetCursorPosition(Sidebar + y * 6, x * 3 + i);
					for (int j = 0; j < 3; j++) {
						Console.BackgroundColor = (x + y) % 2 == 0 ? ConsoleColor.Gray : ConsoleColor.White;
						if (cell.AdjacentMines != 0 && i == 1 && j == 1) {
							Console.ForegroundColor = cell.AdjacentMines switch {
								1 => ConsoleColor.DarkBlue,
								2 => ConsoleColor.DarkGreen,
								3 => ConsoleColor.DarkRed,
								4 => ConsoleColor.Magenta,
								5 => ConsoleColor.Yellow,
								6 => ConsoleColor.DarkCyan,
								_ => ConsoleColor.DarkMagenta,
							};
							Console.Write(cell.AdjacentMines.ToString(" 0"));
						}
						else if (cell.IsMine) {
							Console.BackgroundColor = ConsoleColor.Red;
							if (i == 1 && j == 1)
								Console.BackgroundColor = ConsoleColor.DarkRed;
							Console.Write("  ");
						}
						else Console.Write("  ");
					}
				}
			}
			Console.ResetColor();
		}
		private static void RenderSidebar() {
			for (int i = 0; i < FieldRows * 3; i++) {
				Console.SetCursorPosition(0, i);
				if (i >= 0 && i <= 2) Console.BackgroundColor = ConsoleColor.DarkGray;
				else Console.BackgroundColor = ConsoleColor.Gray;

				if (i == 1) {
					Console.ForegroundColor = ConsoleColor.Black;
					Console.Write("  B A C K  ");
				}
				else if (i == 6) {
					Console.ForegroundColor = ConsoleColor.DarkRed;
					Console.Write("   FLAGS   ");
				}
				else Console.Write("           ");
			}

			Console.BackgroundColor = ConsoleColor.Black;
			for (int i = 0; i < FieldRows * 3; i++) {
				Console.SetCursorPosition(Sidebar - 1, i);
				Console.Write(" ");
			}
			Console.ResetColor();
		}
		private static void RenderFlagsCount(int flagCount) {
			Sources.SetColor(ConsoleColor.Gray, ConsoleColor.DarkRed);
			Console.SetCursorPosition(0, 8);
			Console.Write(flagCount.ToString("    0 0    "));
		}

		private static readonly int[,] win = new int[7, 31] {
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,0,0,0,1,1,0,1,1,1,1,1,1,0,1,1,0,0,1,1,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,0,0,0,1,1,0,0,0,1,1,0,0,0,1,1,1,0,1,1,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,0,1,0,1,1,0,0,0,1,1,0,0,0,1,1,1,1,1,1,0,0,0,0,0 },
			{ 0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,1,1,0,0,0,1,1,0,1,1,1,0,0,0,0,0 },
			{ 0,0,0,0,0,0,1,1,0,1,1,0,0,1,1,1,1,1,1,0,1,1,0,0,1,1,0,0,0,0,0 },
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
		};
		private static readonly int[,] loss = new int[7, 31] {
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
			{ 0,0,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0 },
			{ 0,0,1,1,0,0,0,0,0,1,1,0,0,1,1,0,1,1,0,0,0,0,0,1,1,0,0,0,0,0,0 },
			{ 0,0,1,1,0,0,0,0,0,1,1,0,0,1,1,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0 },
			{ 0,0,1,1,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,1,1,0,0,0,0,0,1,1,0,0 },
			{ 0,0,1,1,1,1,1,1,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0 },
			{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
		};
	}



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
					if (j >= 3 && j < 13) {
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



	internal static partial class Fifteens {
		private static readonly int WindowHeight = 19;
		private static readonly int WindowWidth = 51;
		private const int sidebar = 13;

		private static void Initialization() {
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


		private static void RenderBoard(Board b) {
			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++)
					RenderElement(b, false, i, j);
		}
		private static void RenderOutline() {
			for (int i = 0; i < WindowHeight; i++) {
				Console.SetCursorPosition(sidebar, i);
				for (int j = 0; j < WindowHeight; j++) {
					Console.BackgroundColor = (i == 0 || i == WindowHeight - 1 || j == 0 || j == WindowHeight - 1) ? ConsoleColor.Gray : ConsoleColor.Black;
					Console.Write("  ");
				}
			}
			Console.ResetColor();
		}
		private static void RenderSidebar() {
			for (int i = 0; i < WindowHeight; i++) {
				Console.SetCursorPosition(0, i);
				if (i < 5 || i > 11) 
					Sources.SetColor(ConsoleColor.DarkGray, ConsoleColor.Black);
				else
					Sources.SetColor(ConsoleColor.Gray, ConsoleColor.Black);

				if (i == 2) Console.Write(Sources.CentereString("E X I T", sidebar));
				else if (i == 7) Console.Write(Sources.CentereString("T i m e", sidebar));
				else Console.Write(new string(' ', sidebar));
			}
			Console.ResetColor();
		}
		private static void RenderTime(int seconds) {
			Sources.SetColor(ConsoleColor.Gray, ConsoleColor.Black);
			Console.SetCursorPosition(0, 9);
			Console.Write(Sources.CentereString(Sources.FormatTime(seconds), sidebar));
			Console.ResetColor();
		}
		private static void RenderEndMessage() {
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

		private static void RenderElement(Board b, bool C, int y, int x) {
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



	internal static partial class CowsBulls {
		private static readonly int WindowHeight = 25;
		private static readonly int WindowWidth = 44;
		private static bool selectedRendered;

		private static void Initialization() {
			Console.Title = "CowsBulls";
			Console.CursorVisible = false;
			Console.SetWindowSize(WindowWidth, WindowHeight);
			//Console.SetBufferSize(WindowWidth, WindowHeight);
			Console.Clear();
			Console.ResetColor();
			Thread.Sleep(400);
		}


		private static void RenderMenu() {
			Console.Clear();
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;
			for (int i = 0; i < 3; i++) {
				Console.SetCursorPosition(5, 2 + i);
				if (i == 1) Console.Write(Sources.CentereString("Choose a mode:", 34));
				else Console.Write(Sources.CentereString("", 34));
			}

			for (int o = 0; o < options.Count; o++) {
				for (int i = 0; i < 3; i++) {
					Console.SetCursorPosition(5, 6 + o * 4 + i);
					Console.Write(i switch {
						0 => "╔════════════════════════════════╗",
						1 => $"║{Sources.CentereString(options[o], 32)}║",
						2 => "╚════════════════════════════════╝",
					});
				}
			}
			Console.ResetColor();
		}
		private static void RenderSelected(int o) {
			selectedRendered = true;
			Console.BackgroundColor = ConsoleColor.DarkGray;
			Console.ForegroundColor = ConsoleColor.Gray;
			for (int i = 0; i < 3; i++) {
				Console.SetCursorPosition(5, 6 + o * 4 + i);
				Console.Write(i switch {
					0 => "╔════════════════════════════════╗",
					1 => $"║{Sources.CentereString(options[o], 32)}║",
					2 => "╚════════════════════════════════╝",
				});
			}
			Console.ResetColor();
		}
		private static void RenderUnSelected(int o) {
			selectedRendered = false;
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;
			for (int i = 0; i < 3; i++) {
				Console.SetCursorPosition(5, 6 + o * 4 + i);
				Console.Write(i switch {
					0 => "╔════════════════════════════════╗",
					1 => $"║{Sources.CentereString(options[o], 32)}║",
					2 => "╚════════════════════════════════╝",
				});
			}
			Console.ResetColor();
		}

		private static void RenderStart(string message) {
			Console.Clear();
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;
			for (int i = 2; i < WindowHeight - 2; i++) {
				Console.SetCursorPosition(3, i);
				if (i == 3) Console.Write(Sources.CentereString(message, 38));
				else Console.Write(new string(' ', 38));
			}
		}
		private static void RenderTryMessage(int tryes) {
			Console.SetCursorPosition(6, 5 + tryes);
			Console.WriteLine("Try: {0}", tryes);
		}
		private static string RenderRead(int tryes) {
			char dig;
			string number = "";
			Console.SetCursorPosition(16, 5 + tryes);
			do {
				dig = Console.ReadKey(true).KeyChar;
				if (Sources.Digits.Contains(dig) && !number.Contains(dig)) {
					number += dig;
					Console.Write(dig);
				}
			} while (number.Length != 4);
			return number;
		}
		private static void RenderResult(int tryes, XY result) {
			Console.SetCursorPosition(24, 5 + tryes);
			Console.WriteLine(string.Format("{0}-{1}", result.X, result.Y));
		}

		private static string RenderReadForBot() {
			char dig;
			string number = "";
			Console.SetCursorPosition(28, 3);
			do {
				dig = Console.ReadKey(true).KeyChar;
				if (Sources.Digits.Contains(dig) && !number.Contains(dig)) {
					number += dig;
					Console.Write(dig);
				}
			} while (number.Length != 4);
			return number;
		}
		private static void RenderBotTryMessage(int tryes, string input, XY result, int vars) {
			Console.SetCursorPosition(6, 5 + tryes);
			Console.WriteLine("Try: {0}    {1}    {2}-{3}    V: {4}", tryes, input, result.X, result.Y, vars);
		}
		private static void RenderBotTryMessage(int tryes, string input, int vars) {
			Console.SetCursorPosition(6, 5 + tryes);
			Console.WriteLine("Try: {0}    {1}     -     V: {2}", tryes, input, vars);
		}
		private static XY RenderResultForBot(int tryes) {
			XY result = new();
			do {
				Console.SetCursorPosition(24, 5 + tryes);
				char dig = Console.ReadKey(true).KeyChar;
				if (Sources.Digits.Contains(dig)) {
					result.X = Sources.CharToInt(dig);
					Console.Write(dig);
				}
			} while (result.X == -1);
			Console.Write("-");
			do {
				Console.SetCursorPosition(26, 5 + tryes);
				char dig = Console.ReadKey(true).KeyChar;
				if (Sources.Digits.Contains(dig)) {
					result.Y = Sources.CharToInt(dig);
					Console.Write(dig);
				}
			} while (result.Y == -1);
			return result;
		}

		private static void RenderWin(int tryes, string message) {
			Console.SetCursorPosition(3, 7 + tryes);
			Console.WriteLine(Sources.CentereString(message, 38));
			Console.ResetColor();
		}
	}
}

