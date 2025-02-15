﻿using ConsoleMiniGames.Interfaces;
using ConsoleMiniGames.Types;

namespace ConsoleMiniGames.Games.Fifteens {

	internal delegate XY GetMouseInput();

	internal class Game {
		private bool isRunning = true;
		private readonly Board board = new();
		
		public event GetMouseInput? GetMouseInput;

		private Cell current;
		public Cell Current {
			get => current;
			set {
				if (current != value) {

					if (current.X != -1 && current.Y != -1) { }
				}
			}
		}

		public void Start() {
			Renderer.RenderBoard(board);

			while (isRunning && !Program.ReturnToMenu) {
				int x = -1, y = -1;
				do {
					XY? current = GetMouseInput?.Invoke();

					if (x != -1 && y != -1) {
						current = new(y, x);
						if (tempcurrent != current && current != (-1, -1))
							Renderer.RenderElement(board, true, y, x);
						tempcurrent = current;
					}
					else current = new(-1, -1);

					if (tempcurrent != current && tempcurrent != (-1, -1)) {
						Renderer.RenderElement(board, false, tempcurrent.X, tempcurrent.Y);
						tempcurrent = current;
					}
					Thread.Sleep(16);

					if (cell.X == -1 || cell.Y == -1) Current = new(-1, -1);
					else Current = new(y, x);

				} while (button != MouseButton.LeftButton);

				if (pos >= (0, 0) && pos.X <= Renderer.sidebar && pos.Y <= 4) {
					Program.ReturnToMenu = true;
					cancellationTokenSource.Cancel();
					throw new ReturnToMenu();
				}
				else if (pos.XInBtwn(0, Renderer.sidebar) && pos.YInBtwn(12, 18)) {
					board.Cheat();

					Renderer.RenderBoard(board);
				}
				else if (current != (-1, -1)) {
					board.Swap(current);
					Renderer.RenderBoard(board);
					if (board.CheckWin()) {
						cancellationTokenSource.Cancel();
						Renderer.RenderBoard(board);
						Renderer.RenderElement(board, true, 3, 3);
						Renderer.RenderEndMessage();
						isRunning = false;
					}
				}
			}
		}
	}
}

//using ConsoleMiniGames.Types;

//namespace ConsoleMiniGames.Games.Fifteens {

//	internal class Game {
//		private bool isRunning = true;
//		private readonly Board board = new();

//		public void Start() {
//			CancellationTokenSource cancellationTokenSource = new();
//			Task.Run(() => Fifteens.StartStopwatch(cancellationTokenSource.Token));
//			Thread.Sleep(100);
//			Renderer.RenderBoard(board);

//			while (isRunning && !Program.ReturnToMenu) {
//				XY pos;
//				MouseButton button;
//				int x = -1, y = -1;
//				XY current = new(-1, -1);
//				XY tempcurrent = new(-1, -1);
//				do {
//					(pos, button) = MouseClickHandler.GetMouse();

//					if (pos.Y is >= 2 and <= 4) y = 0;
//					else if (pos.Y is >= 6 and <= 8) y = 1;
//					else if (pos.Y is >= 10 and <= 12) y = 2;
//					else if (pos.Y is >= 14 and <= 16) y = 3;
//					else y = -1;
//					if (pos.X is >= 17 and <= 22) x = 0;
//					else if (pos.X is >= 25 and <= 30) x = 1;
//					else if (pos.X is >= 33 and <= 38) x = 2;
//					else if (pos.X is >= 41 and <= 46) x = 3;
//					else x = -1;

//					if (x != -1 && y != -1) {
//						current = new(y, x);
//						if (tempcurrent != current && current != (-1, -1))
//							Renderer.RenderElement(board, true, y, x);
//						tempcurrent = current;
//					}
//					else current = new(-1, -1);

//					if (tempcurrent != current && tempcurrent != (-1, -1)) {
//						Renderer.RenderElement(board, false, tempcurrent.X, tempcurrent.Y);
//						tempcurrent = current;
//					}
//					Thread.Sleep(16);

//				} while (button != MouseButton.LeftButton);

//				if (pos >= (0, 0) && pos.X <= Renderer.sidebar && pos.Y <= 4) {
//					Program.ReturnToMenu = true;
//					cancellationTokenSource.Cancel();
//					throw new ReturnToMenu();
//				}
//				else if (pos.XInBtwn(0, Renderer.sidebar) && pos.YInBtwn(12, 18)) {
//					board.Cheat();

//					Renderer.RenderBoard(board);
//				}
//				else if (current != (-1, -1)) {
//					board.Swap(current);
//					Renderer.RenderBoard(board);
//					if (board.CheckWin()) {
//						cancellationTokenSource.Cancel();
//						Renderer.RenderBoard(board);
//						Renderer.RenderElement(board, true, 3, 3);
//						Renderer.RenderEndMessage();
//						isRunning = false;
//					}
//				}
//			}
//		}
//	}
//}