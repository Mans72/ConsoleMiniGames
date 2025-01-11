namespace ConsoleMiniGames {

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
}

