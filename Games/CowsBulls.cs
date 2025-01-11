using ConsoleMiniGames.Types;

// 5040
namespace ConsoleMiniGames {

	internal static partial class CowsBulls {
		private static readonly List<string> options = [
			"You Guess Random Number",
			"Bot Guesses Your Number",
			"Bot Guesses, You Enter Results",
			"Exit",
		];
		private static int selected = -1;
		private static int tempselected = -1;

		public static void Start() {
			Initialization();

			do {
				RenderMenu();

				MouseButton button;
				do {
					(XY pos, button) = MouseClickHandler.GetMouse();

					if (pos.X is >= 5 and <= 38) {
						if (pos.Y is >= 6 and <= 8) selected = 0;
						else if (pos.Y is >= 10 and <= 12) selected = 1;
						else if (pos.Y is >= 14 and <= 16) selected = 2;
						else if (pos.Y is >= 18 and <= 20) selected = 3;
						else selected = -1;
					}
					else selected = -1;

					if (tempselected != selected && selected != -1) {
						RenderSelected(selected);
						tempselected = selected;
					}

					if (tempselected != selected && tempselected != -1) {
						RenderUnSelected(tempselected);
						tempselected = selected;
					}

				} while (selected == -1 || button != MouseButton.LeftButton);

				Game game = new();
				switch (selected) {
					case 0:
						game.Start();
						break;
					case 1:
						game.StartBot();
						break;
					case 2:
						game.StartBotBlindly();
						break;
					case 3:
						Program.ReturnToMenu = true;
						break;
				}

				if (!Program.ReturnToMenu)
					MouseClickHandler.GetMouseClick();
			} while (!Program.ReturnToMenu);
		}



		private class Game {
			private XY result;
			private int tryes = 0;
			private string input = "";
			private string number = "";
			private readonly Random rand = new();
			private List<string> vars = [];

			public void Start() {
				number = GenRandNum();
				RenderStart("Enter a number and get the result:");
				do {
					RenderTryMessage(tryes);
					input = RenderRead(tryes);

					result = new(0, 0);
					for (int i = 0; i < 4; i++)
						if (number.Contains(input[i]))
							if (number[i] == input[i]) result.X++;
							else result.Y++;

					RenderResult(tryes, result);
					tryes++;
				} while (result.X != 4);

				RenderWin(tryes, $"YOU WON!    Tryes: {tryes}");
			}

			public void StartBot() {
				vars = GenUniqueDigitNumbers();
				RenderStart("Enter a number:       ");
				number = RenderReadForBot();
				do {
					if (tryes != 0) vars = FilteringVars(vars);
					input = vars[rand.Next(vars.Count)];

					result = new(0, 0);
					for (int i = 0; i < 4; i++) {
						if (number.Contains(input[i]))
							if (number[i] == input[i]) result.X++;
							else result.Y++;
					}
					tryes++;
					RenderBotTryMessage(tryes, input, result, vars.Count);
					Thread.Sleep(200);
				} while (result.X != 4);

				RenderWin(tryes, $"I won in {tryes} tries!");
			}

			public void StartBotBlindly() {
				vars = GenUniqueDigitNumbers();
				RenderStart("Bot trying to guess a number:");
				do {
					try {
						if (tryes != 0) vars = FilteringVars(vars);
						input = vars[rand.Next(vars.Count)];
						tryes++;
					}
					catch (ArgumentOutOfRangeException) {
						RenderWin(tryes, "You are deceiving me!");
						tryes = 0;
						break;
					}
					RenderBotTryMessage(tryes, input, vars.Count);

					if (result.X != 4)
						result = RenderResultForBot(tryes);
				} while (result.X != 4);

				if (tryes != 0)
					RenderWin(tryes, $"I won in {tryes} tries!");
			}


			private static string GenRandNum() {
				Random rand = new();
				string dig, number = "";
				do {
					dig = rand.Next(0, 10).ToString();
					if (!number.Contains(dig))
						number += dig;
				} while (number.Length != 4);
				return number;
			}
			private static List<string> GenUniqueDigitNumbers() {
				List<string> result = [];
				string number;
				for (int i = 0; i < 10000; i++) {
					number = i.ToString("0000");
					if (Sources.HasUniqueDigits(number, 4))
						result.Add(number);
				}
				return result;
			}
			private List<string> FilteringVars(List<string> vars) {
				List<string> temp = [];
				for (int i = 0; i < vars.Count; i++)
					if (result == CheckCorrect(vars[i]))
						temp.Add(vars[i]);
				return temp;
			}
			private XY CheckCorrect(string correct) {
				XY check = new();
				for (int i = 0; i < 4; i++) {
					if (correct.Contains(input[i]))
						if (correct[i] == input[i]) check.X++;
						else check.Y++;
				}
				return check;
			}
		}
	}
}