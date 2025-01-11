using ConsoleMiniGames.Types;

namespace ConsoleMiniGames {

	internal static partial class Fifteens {
		private static readonly object consoleLock = new();

		public static void Start() {

			Initialization();
			do {
				Game game = new();
				game.Start();

				(XY pos, MouseButton button) = MouseClickHandler.GetMouseClick();
				if (pos >= (0, 0) && pos.X <= sidebar && pos.Y <= 4 && button == MouseButton.LeftButton)
					Program.ReturnToMenu = true;
			} while (!Program.ReturnToMenu);
		}

		private static async Task StartStopwatch(CancellationToken token) {
			int seconds = 0;

			while (!token.IsCancellationRequested) {
				seconds++;
				lock (consoleLock)
					RenderTime(seconds);
				await Task.Delay(1000);
			}
		}


		private class Game {
			private bool isRunning = true;
			private readonly Board board = new();

			public void Start() {
				CancellationTokenSource cancellationTokenSource = new();
				Task.Run(() => StartStopwatch(cancellationTokenSource.Token));
				Thread.Sleep(100);
				lock (consoleLock)
					RenderBoard(board);

				while (isRunning && !Program.ReturnToMenu) {
					XY pos;
					MouseButton button;
					int x = -1, y = -1;
					XY current = new(-1, -1);
					XY tempcurrent = new(-1, -1);
					do {
						(pos, button) = MouseClickHandler.GetMouse();

						if (pos.Y is >= 2 and <= 4) y = 0;
						else if (pos.Y is >= 6 and <= 8) y = 1;
						else if (pos.Y is >= 10 and <= 12) y = 2;
						else if (pos.Y is >= 14 and <= 16) y = 3;
						else y = -1;
						if (pos.X is >= 17 and <= 22) x = 0;
						else if (pos.X is >= 25 and <= 30) x = 1;
						else if (pos.X is >= 33 and <= 38) x = 2;
						else if (pos.X is >= 41 and <= 46) x = 3;
						else x = -1;

						if (x != -1 && y != -1) {
							current = new(y, x);
							if (tempcurrent != current && current != (-1, -1))
								lock (consoleLock)
									RenderElement(board, true, y, x);
							tempcurrent = current;
						}
						else current = new(-1, -1);

						if (tempcurrent != current && tempcurrent != (-1, -1)) {
							lock (consoleLock)
								RenderElement(board, false, tempcurrent.X, tempcurrent.Y);
							tempcurrent = current;
						}
						Thread.Sleep(16);
					} while (button != MouseButton.LeftButton);

					if (pos >= (0, 0) && pos.X <= sidebar && pos.Y <= 4) {
						Program.ReturnToMenu = true;
						cancellationTokenSource.Cancel();
						throw new ReturnToMenu();
					}
					else if (pos.XInBtwn(0, sidebar) && pos.YInBtwn(12, 18)) {
						board.Cheat();
						lock (consoleLock)
							RenderBoard(board);
					}
					else if (current != (-1, -1)) {
						board.Swap(current);
						lock (consoleLock)
							RenderBoard(board);
						if (board.CheckWin()) {
							cancellationTokenSource.Cancel();
							RenderBoard(board);
							RenderElement(board, true, 3, 3);
							RenderEndMessage();
							isRunning = false;
						}
					}
				}
			}
		}


		private class Board {
			public int[,] Grid { get; private set; }

			public Board() {
				Grid = new int[4, 4];
				for (int i = 0; i < 4; i++)
					for (int j = 0; j < 4; j++)
						Grid[i, j] = i * 4 + j;
				Sources.ArrayShuffle<int>(Grid);
			}

			public void Swap(XY pos) {
				if (pos.X != 3 && Grid[pos.X + 1, pos.Y] == 0)
					(Grid[pos.X, pos.Y], Grid[pos.X + 1, pos.Y]) = (Grid[pos.X + 1, pos.Y], Grid[pos.X, pos.Y]);
				else if (pos.X != 0 && Grid[pos.X - 1, pos.Y] == 0)
					(Grid[pos.X, pos.Y], Grid[pos.X - 1, pos.Y]) = (Grid[pos.X - 1, pos.Y], Grid[pos.X, pos.Y]);
				else if (pos.Y != 3 && Grid[pos.X, pos.Y + 1] == 0)
					(Grid[pos.X, pos.Y], Grid[pos.X, pos.Y + 1]) = (Grid[pos.X, pos.Y + 1], Grid[pos.X, pos.Y]);
				else if (pos.Y != 0 && Grid[pos.X, pos.Y - 1] == 0)
					(Grid[pos.X, pos.Y], Grid[pos.X, pos.Y - 1]) = (Grid[pos.X, pos.Y - 1], Grid[pos.X, pos.Y]);
			}
			public bool IsMoveable(int x, int y) {
				if (x != 3 && Grid[x + 1, y] == 0) return true;
				else if (x != 0 && Grid[x - 1, y] == 0) return true;
				else if (y != 3 && Grid[x, y + 1] == 0) return true;
				else if (y != 0 && Grid[x, y - 1] == 0) return true;
				else return false;
			}
			public bool CheckWin() {
				for (int i = 0; i < 15; i++)
					if (Grid[i / 4, i % 4] != i + 1)
						return false;
				return true;
			}
			public void Cheat() {
				Grid = new int[,] {
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 10, 11, 12 },
				{ 13, 14, 0, 15 }
			};
			}
		}
	}
}