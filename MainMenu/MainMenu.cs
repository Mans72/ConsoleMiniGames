using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleMiniGames.Types;
using ConsoleMiniGames.Games;

namespace ConsoleMiniGames.MainMenu {

	internal class MainMenu : BaseController {
		public static readonly string[] Games = [
			"Sea Battle",	// +
			"Minesweeper",	// +
			"Tic Tac Toe",	// +
			"Fifteens",		// +
			"Cows & Bulls",	// +
			"Tetris (Soon)",// 
			"Snake (Soon)",	// 
		];

		private delegate void StartGame();
		internal static bool ReturnToMenu = false;

		private int selected = -1;
		public int NewSelected {
			get => selected;
			private set {
				if (selected != value && value < Games.Length) {
					if (selected != -1)
						OnUnselection(nameof(NewSelected));
					selected = value;
					if (selected != -1)
						OnSelection(nameof(NewSelected));
				}
			}
		}

		public void Start() {
			while (true) {
				int x = -1, y = -1;
				MouseClick mouse;

				do {
					mouse = MouseClickHandler.HandleMouse();

					if (mouse.PosY is >= 5 and <= 7) y = 0;
					else if (mouse.PosY is >= 9 and <= 11) y = 1;
					else if (mouse.PosY is >= 13 and <= 15) y = 2;
					else y = -1;

					if (mouse.PosX is >= 2 and <= 17) x = 0;
					else if (mouse.PosX is >= 20 and <= 35) x = 1;
					else if (mouse.PosX is >= 38 and <= 53) x = 2;
					else x = -1;

					if (x == -1 || y == -1) NewSelected = -1;
					else NewSelected = y * 3 + x;

					//if (x != -1 && y != -1) {
					//	NewSelected = y * 3 + x;
					//	if (NewSelected >= Games.Length) NewSelected = -1;

					//	if (selected != NewSelected && NewSelected != -1)
					//		RenderSelected(NewSelected);

					//	selected = NewSelected;
					//}
					//else NewSelected = -1;

					//if (selected != NewSelected && selected != -1) {
					//	RenderUnSelected(selected);
					//	selected = NewSelected;
					//}

				} while (NewSelected == -1 || mouse.Button != MouseButton.LeftButton);

				StartGame start = NewSelected switch {
					0 => ConsoleMiniGames.Games.SeaBattle.SeaBattle.Start,
					1 => ConsoleMiniGames.Games.MineSweeper.MineSweeper.Start,
					2 => ConsoleMiniGames.Games.TicTacToe.TicTacToe.Start,
					3 => ConsoleMiniGames.Games.Fifteens.Fifteens.Start,
					4 => ConsoleMiniGames.Games.CowsBulls.CowsBulls.Start,
					_ => () => { }
					,
				};

				try {
					start();
				}
				catch (ReturnToMenu) { }

				NewSelected = -1;
				ReturnToMenu = false; // Сбрасываем флаг для следующего выбора игры
			}
		}
	}
}
