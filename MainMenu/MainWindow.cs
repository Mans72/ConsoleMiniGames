using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiniGames.MainMenu {

	internal class MainWindow : BaseView {

		public MainWindow() {
			Title = "Console Mini Games";
			windowHeight = 4 * (1 + (MainMenu.Games.Length + 2) / 3) + 1;
			windowWidth = 56;
			Initialization();
		}


		protected override void Initialization() {
			ConfigureWindow();
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;

			for (int i = 0; i < 3; i++) {
				Console.SetCursorPosition(2, 1 + i);
				if (i == 1) Console.Write(Sources.CentereString("Choose a game:", 52));
				else Console.Write(Sources.CentereString("", 52));
			}

			for (int g = 0; g < MainMenu.Games.Length; g++) {
				for (int i = 0; i < 3; i++) {
					Console.SetCursorPosition(2 + g % 3 * 18, 5 + g / 3 * 4 + i);
					Console.Write(i switch {
						0 => "╔══════════════╗",
						1 => $"║{Sources.CentereString(MainMenu.Games[g], 14)}║",
						2 => "╚══════════════╝",
					});

					//  ╔ ═ ╗ ║ ╚ ╝
				}
			}
			Console.ResetColor();
		}

		public void RenderSelected(object? sender, PropertyChangedEventArgs e) {
			if (sender is MainMenu menu && e.PropertyName == nameof(menu.Selected)) {
				Console.BackgroundColor = ConsoleColor.DarkGray;
				Console.ForegroundColor = ConsoleColor.Gray;
				for (int i = 0; i < 3; i++) {
					Console.SetCursorPosition(2 + menu.Selected % 3 * 18, 5 + menu.Selected / 3 * 4 + i);
					Console.Write(i switch {
						0 => "╔══════════════╗",
						1 => $"║{Sources.CentereString(MainMenu.Games[menu.Selected], 14)}║",
						2 => "╚══════════════╝",
					});
				}
				Console.ResetColor();
			}
		}

		public void RenderUnselected(object? sender, PropertyChangedEventArgs e) {
			if (sender is MainMenu menu && e.PropertyName == nameof(menu.Selected)) {
					Console.BackgroundColor = ConsoleColor.Gray;
				Console.ForegroundColor = ConsoleColor.Black;
				for (int i = 0; i < 3; i++) {
					Console.SetCursorPosition(2 + menu.Selected % 3 * 18, 5 + menu.Selected / 3 * 4 + i);
					Console.Write(i switch {
						0 => "╔══════════════╗",
						1 => $"║{Sources.CentereString(MainMenu.Games[menu.Selected], 14)}║",
						2 => "╚══════════════╝",
					});
				}
				Console.ResetColor();
			}
		}

	}
}
