using ConsoleMiniGames.Types;

namespace ConsoleMiniGames.Games.MineSweeper {

	internal static partial class MineSweeper {
		private static Difficulty Difficulty { get; set; } = Difficulty.Easy;
		private static int Sidebar { get; } = 12;

		private static int AmountMines {
			get => Difficulty switch {
				Difficulty.Easy => 12,
				Difficulty.Medium => 35,
				Difficulty.Hard => 60,
				_ => 6,
			};
		}
		private static int FieldRows {
			get => Difficulty switch {
				Difficulty.Easy => 6,
				Difficulty.Medium => 9,
				Difficulty.Hard => 12,
				_ => 6,
			};
		}
		private static int FieldCols {
			get => Difficulty switch {
				Difficulty.Easy => 12,
				Difficulty.Medium => 18,
				Difficulty.Hard => 24,
				_ => 6,
			};
		}

		public static void Start() {

			Initialization();

			do {
				// добавить уровень сложности

				Game game = new();
				game.Start();
				if (Program.ReturnToMenu) break;

				(XY pos, MouseButton button) = MouseClickHandler.GetMouseClick();
				if (pos.X >= 0 && pos.X <= 10 && pos.Y >= 0 && pos.Y <= 2 && button == MouseButton.LeftButton)
					Program.ReturnToMenu = true;
			} while (!Program.ReturnToMenu);
		}



		private class Game {
			private bool isRunning = true;
			private readonly Board board = new();
			private bool firstCell = true;

			public void Start() {

				while (isRunning) {
					board.Render();

					MakeMove(out XY pos, out MouseButton button);
					if (Program.ReturnToMenu) break;

					if (button == MouseButton.LeftButton) {
						if (firstCell) board.Initialize(pos);
						firstCell = false;
						if (board.RevealCell(pos)) EndGame(false);
					}
					else if (!firstCell && button == MouseButton.RightButton) {
						board.ToggleFlag(pos);
					}
					if (board.AllCellsRevealed()) EndGame(true);
				}
			}

			private static void MakeMove(out XY pos, out MouseButton button) {
				do {
					(pos, button) = MouseClickHandler.GetMouseClick();
					if (pos.X >= 0 && pos.X <= 10 && pos.Y >= 0 && pos.Y <= 2) {
						Program.ReturnToMenu = true;
						return;
					}
					Thread.Sleep(32);
				} while (!IsValidPosition(ref pos) && (button != MouseButton.LeftButton || button != MouseButton.RightButton));
			}

			private void EndGame(bool status) {
				isRunning = false;
				board.Render();
				RenderEndMessage(status);
			}
			private static bool IsValidPosition(ref XY pos) {
				if (pos < (0, 0)) return false;
				if (pos.X < Sidebar) return false;
				pos.X -= Sidebar; pos.X /= 6; pos.Y /= 3;
				(pos.X, pos.Y) = (pos.Y, pos.X);
				if (pos.X > FieldRows) return false;
				if (pos.Y > FieldCols) return false;
				return true;
			}
		}



		private class Board {
			private readonly int rows = FieldRows;
			private readonly int cols = FieldCols;
			private readonly Cell[,] Cells = new Cell[FieldRows, FieldCols];
			private int FlagCount { get; set; } = AmountMines;

			// Инициализация поля, расстановка мин, происходит после первого выбора клетки
			public void Initialize(XY pos) {
				List<XY> mines = [];
				Random rnd = new();
				for (int m = 0; m < AmountMines; m++) {
					int x, y;
					do {
						x = rnd.Next(rows);
						y = rnd.Next(cols);
					} while (Sources.IsAround(pos.X, pos.Y, x, y) || Cells[x, y].IsMine);

					Cells[x, y].IsMine = true;
					mines.Add(new XY(x, y));
					for (int i = -1; i <= 1; i++)
						for (int j = -1; j <= 1; j++)
							if ((x + i) >= 0 && (x + i) < rows && (y + j) >= 0 && (y + j) < cols)
								Cells[x + i, y + j].AdjacentMines++;
				}
				foreach (XY m in mines)
					Cells[m.X, m.Y].AdjacentMines = 0;
			}
			private void RevealEmptyCells(int row, int col) {
				// Если ячейка уже раскрыта или это мина, выходим
				if (Cells[row, col].IsRevealed || Cells[row, col].IsFlagged) return;
				// Раскрываем текущую ячейку
				Cells[row, col].IsRevealed = true;
				// Если вокруг ячейки есть мины, останавливаемся (не раскрываем соседей)
				if (Cells[row, col].AdjacentMines > 0 || Cells[row, col].IsMine) return;

				// Рекурсивно раскрываем соседние ячейки
				for (int i = -1; i <= 1; i++) {
					for (int j = -1; j <= 1; j++) {
						// Избегаем рекурсивного вызова для той же ячейки
						if (i == 0 && j == 0) continue;
						// Проверка, чтобы не выйти за границы массива
						if (WithinBoundaries(row + i, col + j))
							RevealEmptyCells(row + i, col + j);
					}
				}
			}
			private bool RevealCellsAround(int row, int col) {
				int flags = 0;
				for (int i = -1; i <= 1; i++)
					for (int j = -1; j <= 1; j++)
						if (WithinBoundaries(row + i, col + j) && Cells[row + i, col + j].IsFlagged)
							flags++;


				if (flags != 0 && flags == Cells[row, col].AdjacentMines)
					for (int i = -1; i <= 1; i++)
						for (int j = -1; j <= 1; j++)
							if (WithinBoundaries(row + i, col + j) && !Cells[row + i, col + j].IsFlagged && !Cells[row + i, col + j].IsRevealed)
								if (RevealCell(new XY(row + i, col + j))) return true;

				return false;
			}

			public void Render() {
				RenderBoard(Cells, FlagCount);
			}

			public bool RevealCell(XY pos) {
				bool mine = false;

				if (Cells[pos.X, pos.Y].IsMine) {
					Cells[pos.X, pos.Y].IsRevealed = true;
					mine = true;
				}
				else if (!Cells[pos.X, pos.Y].IsRevealed && !Cells[pos.X, pos.Y].IsFlagged)
					RevealEmptyCells(pos.X, pos.Y);
				else if (Cells[pos.X, pos.Y].IsRevealed)
					mine = RevealCellsAround(pos.X, pos.Y);

				return mine;
			}
			public void ToggleFlag(XY pos) {
				if (Cells[pos.X, pos.Y].IsFlagged) {
					Cells[pos.X, pos.Y].IsFlagged = false;
					FlagCount++;
				}
				else if (FlagCount > 0 && !Cells[pos.X, pos.Y].IsRevealed) {
					Cells[pos.X, pos.Y].IsFlagged = true;
					FlagCount--;
				}
			}
			public bool AllCellsRevealed() {
				foreach (var cell in Cells) {
					if (!cell.IsMine && !cell.IsRevealed) return false;
				}
				return true;
			}

			private bool WithinBoundaries(int row, int col) {
				return (row >= 0 && row < rows && col >= 0 && col < cols);
			}
		}



		private struct Cell(bool isMine) {
			public bool IsMine = isMine;
			public bool IsRevealed = false;
			public bool IsFlagged = false;
			public short AdjacentMines = 0;
		}
	}
}