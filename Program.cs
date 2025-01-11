using ConsoleMiniGames.Types;

namespace ConsoleMiniGames {

	internal partial class Program {
		private static readonly List<string> games = [
			"Sea Battle",//		+
			"Minesweeper",//	+
			"Tic Tac Toe",//	+
			"Fifteens",//		+
			"Cows & Bulls",//	+
			"Tetris (Soon)",//	
			"Snake (Soon)",//	
		];
		private static int selected = -1;
		private static int tempselected = -1;

		private delegate void StartGame();
		internal static bool ReturnToMenu = false;

		internal static void Main() {

			while (true) {
				Initialization();
				RenderMenu();

				int x = -1, y = -1;
				XY pos;
				MouseButton button;
				do {
					(pos, button) = MouseClickHandler.GetMouse();

					if (pos.Y is >= 5 and <= 7) y = 0;
					else if (pos.Y is >= 9 and <= 11) y = 1;
					else if (pos.Y is >= 13 and <= 15) y = 2;
					else y = -1;

					if (pos.X is >= 2 and <= 17) x = 0;
					else if (pos.X is >= 20 and <= 35) x = 1;
					else if (pos.X is >= 38 and <= 53) x = 2;
					else x = -1;

					if (x != -1 && y != -1) {
						selected = y * 3 + x;
						if (selected >= games.Count) selected = -1;

						if (tempselected != selected && selected != -1)
							RenderSelected(selected);

						tempselected = selected;
					}
					else selected = -1;

					if (tempselected != selected && tempselected != -1) {
						RenderUnSelected(tempselected);
						tempselected = selected;
					}

				} while (selected == -1 || button != MouseButton.LeftButton);

				StartGame start = selected switch {
					0 => SeaBattle.Start,
					1 => MineSweeper.Start,
					2 => TicTacToe.Start,
					3 => Fifteens.Start,
					4 => CowsBulls.Start,
					_ => () => { },
				};

				try {
					start();
				}
				catch (ReturnToMenu) { }

				selected = -1;
				ReturnToMenu = false; // Сбрасываем флаг для следующего выбора игры
			}
		}
	}
}
