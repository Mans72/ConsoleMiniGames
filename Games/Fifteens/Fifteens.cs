using ConsoleMiniGames.Types;

namespace ConsoleMiniGames.Games.Fifteens {

	internal class Fifteens : BaseController<FifteensView> {
		private CancellationTokenSource cancellationTokenSource = new();

		protected override FifteensView View { get; set; }

		public Fifteens() {
			View = new FifteensView();
		}


		public override void Start() {
			do {
				Game game = new();
				game.Start();

				MouseClick click = MouseClickHandler.HandleMouseClick();
				if (click.Position >= (0, 0) && click.PosX <= View.Sidebar && click.PosY <= 4 && click.Button == MouseButton.LeftButton)
					Program.ReturnToMenu = true;
			} while (!Program.ReturnToMenu);
		}

		public async Task StartStopwatch(CancellationToken token) {
			int seconds = 0;

			while (!token.IsCancellationRequested) {
				seconds++;
				View.RenderTime(seconds);
				await Task.Delay(1000);
			}
		}

		public XY Trackmouse() {
			MouseClick click;
			XY cell;

			while (!Program.ReturnToMenu) {
				click = MouseClickHandler.HandleMouseClick();

				if (click.Position >= (0, 0) && click.PosX <= View.Sidebar && click.PosY <= 4 && click.Button == MouseButton.LeftButton) {
					Program.ReturnToMenu = true;
					cancellationTokenSource.Cancel();
					throw new ReturnToMenu();
				}

				if (click.PosY is >= 2 and <= 4) cell.Y = 0;
				else if (click.PosY is >= 6 and <= 8) cell.Y = 1;
				else if (click.PosY is >= 10 and <= 12) cell.Y = 2;
				else if (click.PosY is >= 14 and <= 16) cell.Y = 3;
				else cell.Y = -1;
				if (click.PosX is >= 17 and <= 22) cell.X = 0;
				else if (click.PosX is >= 25 and <= 30) cell.X = 1;
				else if (click.PosX is >= 33 and <= 38) cell.X = 2;
				else if (click.PosX is >= 41 and <= 46) cell.X = 3;
				else cell.X = -1;

				
			}
		}
	}
}

//using ConsoleMiniGames.Types;

//namespace ConsoleMiniGames.Games.Fifteens {

//	internal class Fifteens : BaseController<FifteensView> {
//		private CancellationTokenSource cancellationTokenSource = new();

//		protected override FifteensView View { get; set; }

//		public Fifteens() {
//			View = new FifteensView();
//		}

//		public override void Start() {
//			do {
//				Game game = new();
//				game.Start();

//				MouseClick click = MouseClickHandler.HandleMouseClick();
//				if (click.Position >= (0, 0) && click.PosX <= View.Sidebar && click.PosY <= 4 && click.Button == MouseButton.LeftButton)
//					Program.ReturnToMenu = true;
//			} while (!Program.ReturnToMenu);
//		}

//		public async Task StartStopwatch(CancellationToken token) {
//			int seconds = 0;

//			while (!token.IsCancellationRequested) {
//				seconds++;
//				View.RenderTime(seconds);
//				await Task.Delay(1000);
//			}
//		}
//	}
//}