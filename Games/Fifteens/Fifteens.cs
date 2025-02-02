using ConsoleMiniGames.Types;

namespace ConsoleMiniGames.Games.Fifteens {

	internal static class Fifteens {
		private static readonly object consoleLock = new();

		public static void Start() {

			Renderer.Initialization();
			do {
				Game game = new();
				game.Start();

				(XY pos, MouseButton button) = MouseClickHandler.GetMouseClick();
				if (pos >= (0, 0) && pos.X <= Renderer.sidebar && pos.Y <= 4 && button == MouseButton.LeftButton)
					Program.ReturnToMenu = true;
			} while (!Program.ReturnToMenu);
		}

		public static async Task StartStopwatch(CancellationToken token) {
			int seconds = 0;

			while (!token.IsCancellationRequested) {
				seconds++;
				lock (consoleLock)
					Renderer.RenderTime(seconds);
				await Task.Delay(1000);
			}
		}
	}
}