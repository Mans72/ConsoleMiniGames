using ConsoleMiniGames.Types;

namespace ConsoleMiniGames {

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

