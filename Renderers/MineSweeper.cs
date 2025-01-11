namespace ConsoleMiniGames {

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
				if (i is >= 0 and <= 2) Console.BackgroundColor = ConsoleColor.DarkGray;
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
}

