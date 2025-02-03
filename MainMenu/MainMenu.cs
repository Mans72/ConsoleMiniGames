using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleMiniGames.Types;
using ConsoleMiniGames.Games;

namespace ConsoleMiniGames.MainMenu {

	internal class MainMenu : BaseController<MainWindow> {
		protected override MainWindow View { get; set; }

		public static readonly string[] Games = [
			"Sea Battle",	// +
			"Minesweeper",	// +
			"Tic Tac Toe",	// +
			"Fifteens",		// +
			"Cows & Bulls",	// +
			"Tetris (Soon)",// 
			"Snake (Soon)",	// 
		];

		public static bool ReturnToMenu = false;

		private int selected = -1;
		public int Selected {
			get => selected;
			private set {
				if (selected != value && value < Games.Length) {
					if (selected != -1)
						OnUnselection(nameof(Selected));
					selected = value;
					if (selected != -1)
						OnSelection(nameof(Selected));
				}
			}
		}

		public MainMenu() {
			View = new MainWindow();
			ElementSelected += View.RenderSelected;
			ElementUnselected += View.RenderUnselected;
		}

		public override void Start() {
			while (true) {
				int x, y;
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

					if (x == -1 || y == -1) Selected = -1;
					else Selected = y * 3 + x;

				} while (Selected == -1 || mouse.Button != MouseButton.LeftButton);


				IBaseController game = Selected switch {
					0 => new Games.SeaBattle.SeaBattle(),
					1 => new Games.MineSweeper.MineSweeper(),
					2 => new Games.TicTacToe.TicTacToe(),
					3 => new Games.Fifteens.Fifteens(),
					4 => new Games.CowsBulls.CowsBulls(),
					_ => new(),
				};

				try {
					game.Start();
				}
				catch (ReturnToMenu) { }

				Selected = -1;
				ReturnToMenu = false; // Сбрасываем флаг для следующего выбора игры
			}
		}
	}
}
