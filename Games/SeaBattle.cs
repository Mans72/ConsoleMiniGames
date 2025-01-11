using ConsoleMiniGames.Types;

namespace ConsoleMiniGames {

	internal static partial class SeaBattle {
		private const bool human = true;

		public static void Start() {

			Initialization();

			do {
				Game game = new();
				game.Start();

				do {
					(XY pos, MouseButton button) = MouseClickHandler.GetMouseClick();
					if (pos.Y >= 22 && pos.Y <= 28 && pos.X >= 2 && pos.X <= 41 && button == MouseButton.LeftButton)
						Program.ReturnToMenu = true;
					if (pos.Y >= 22 && pos.Y <= 28 && pos.X >= 44 && pos.X <= 83 && button == MouseButton.LeftButton)
						break;
				} while (true && !Program.ReturnToMenu);
			} while (!Program.ReturnToMenu);
		}




		private class Game {
			private bool isRunning;
			private Player currentPlayer;
			private readonly Player player1;
			private readonly Player player2;

			public Game() {
				RenderEmptyFields();
				isRunning = true;
				player1 = human ? new Human() : new Bot();
				player2 = new Bot();
				Random rnd = new();
				currentPlayer = rnd.Next(100) < 50 ? player1 : player2;
			}

			public void Start() {
				RenderBoard(player1.GetField(true), human ? player1.GetField(false) : player2.GetField(true));

				while (isRunning) {
					bool keepMove;
					do {
						keepMove = false;
						XY position = currentPlayer.MakeMove();
						if (!human) RenderBotHits(currentPlayer == player2, position);

						(bool boolResult, Ship? ship) = currentPlayer == player1 ? player2.Answer(position) : player1.Answer(position);
						if (boolResult && ship != null) {// победа
							currentPlayer.Marking(position, ship);
							Console.Beep();
							EndGame(currentPlayer == player1);
							break;
						}
						else if (boolResult == false && ship != null) {// уничтожил
							currentPlayer.Marking(position, ship);
							keepMove = true;
							Console.Beep();
						}
						else if (ship == null) {// попадание / промах
							currentPlayer.Marking(position, boolResult);
							keepMove = boolResult;
						}

						RenderBoard(player1.GetField(true), human ? player1.GetField(false) : player2.GetField(true));
						Thread.Sleep(50);
					} while (keepMove);

					SwitchPlayer();
				}

			}

			private void SwitchPlayer() {
				currentPlayer = currentPlayer == player1 ? player2 : player1;
			}
			private void EndGame(bool status) {
				isRunning = false;
				RenderBoard(player1.GetField(true), human ? player1.GetField(false) : player2.GetField(true));
				RenderEndMessage(status);
			}
		}



		private abstract class Player() {
			protected Field MyField { get; private set; } = new Field();
			protected Field EnemyField { get; private set; } = new Field();
			private readonly List<Ship> ships = [];

			public abstract XY MakeMove();
			public abstract void Marking(XY position, bool hit);
			public abstract void Marking(XY position, Ship ship);

			public int[,] GetField(bool myField) {
				return myField ? MyField.Grid : EnemyField.Grid;
			}

			public (bool, Ship?) Answer(XY pos) {
				if (MyField.IsHit(pos)) {
					Ship ship = ships.First(s => s.Cells.Contains(pos));
					if (ship.RegisterHit(pos)) {
						MyField.ShipDestroyed(ship);
						if (!HasUndestroyedShips()) return (true, ship);// победа
						else return (false, ship);// уничтожил
					}
					else return (true, null);// попадание
				}
				else return (false, null);// промах
			}
			private bool HasUndestroyedShips() {
				foreach (Ship ship in ships) {
					if (!ship.IsDestroyed()) return true;
				}
				return false;
			}

			protected void SetShips(bool auto) {
				int[] ShipsAmount = [4, 3, 2, 1];
				for (int size = ShipsAmount.Length; size >= 1; size--)
					for (int i = 0; i < ShipsAmount[size - 1]; i++)
						if (auto) SelectPositionAuto(size);
						else SelectPosition(size);
			}
			private void SelectPosition(int length) {
				RenderField(MyField.Grid, true);
				bool[] place = new bool[length];
				bool horizontal = false;
				XY temp = new(-1, -1);
				do {
					(XY pos, MouseButton button) = MouseClickHandler.GetMouse();

					if (pos.X >= leftFP.X && pos.X < leftFP.X + 40 && pos.Y >= leftFP.Y && pos.Y < leftFP.Y + 20) {
						pos.X = (pos.X - 2) / 4; pos.Y = (pos.Y - 1) / 2;
						(pos.X, pos.Y) = (pos.Y, pos.X);

						if (horizontal == true && pos.Y > 10 - length) pos.Y = 10 - length;
						else if (horizontal == false && pos.X > 10 - length) pos.X = 10 - length;

						if (button == MouseButton.RightButton) {
							if (horizontal == true && pos.X <= 10 - length) horizontal = false;
							else if (horizontal == false && pos.Y <= 10 - length) horizontal = true;
							temp = new(-1, -1);
						}

						if (temp != pos) {
							XY pos2 = pos;
							for (int k = 0; k < length; k++) {
								place[k] = true;
								for (int i = -1; i <= 1; i++)
									for (int j = -1; j <= 1; j++)
										if (pos2.X + j >= 0 && pos2.X + j < 10 && pos2.Y + i >= 0 && pos2.Y + i < 10)
											if (MyField.Grid[pos2.X + j, pos2.Y + i] == (int)Cell.Boat)
												place[k] = false;
								if (horizontal) pos2.Y++;
								else pos2.X++;
							}

							RenderField(MyField.Grid, true);
							RenderPlacing(pos, length, horizontal, place);
							temp = pos;
						}

						if (button == MouseButton.LeftButton && MyField.PlaceShip(pos, length, horizontal, ships)) return;
					}
					else if (pos.Y >= 22 && pos.Y <= 28 && pos.X >= 2 && pos.X <= 41 && button == MouseButton.LeftButton) {
						Program.ReturnToMenu = true;
						throw new ReturnToMenu();
					}
					Thread.Sleep(50);
				} while (true);
			}
			private void SelectPositionAuto(int length) {
				Random rnd = new();
				bool done;
				do {
					bool horiz = rnd.Next(100) < 50;
					int x = horiz ? rnd.Next(10) : rnd.Next(10 - length);
					int y = horiz ? rnd.Next(10 - length) : rnd.Next(10);
					done = MyField.PlaceShip(new XY(x, y), length, horiz, ships);
				} while (!done);
			}
		}

		private class Human : Player {
			public Human() {
				SetShips(SelectSetMethod());
			}
			public override XY MakeMove() {
				XY pos;
				MouseButton button;
				do {
					do {
						(pos, button) = MouseClickHandler.GetMouseClick();
						if (pos.Y >= 22 && pos.Y <= 28 && pos.X >= 2 && pos.X <= 41 && button == MouseButton.LeftButton) {
							Program.ReturnToMenu = true;
							throw new ReturnToMenu();
						}
					} while (!IsValidPos(ref pos) || button != MouseButton.LeftButton);

					RenderSelectingCoords(EnemyField.Grid, pos);
					Thread.Sleep(500);
					RenderField(EnemyField.Grid, false);
				} while (EnemyField.Grid[pos.X, pos.Y] != (int)Cell.Sea);
				return pos;
			}
			public override void Marking(XY position, bool hit) {
				EnemyField.MarkHit(position, hit);
			}
			public override void Marking(XY position, Ship ship) {
				EnemyField.MarkHit(position, true);
				EnemyField.ShipDestroyed(ship);
			}

			private static bool IsValidPos(ref XY pos) {
				if (pos.X < 44 || pos.X > 83 || pos.Y < 1 || pos.Y > 20) return false;
				pos.X = (pos.X - 44) / 4;
				pos.Y = (pos.Y - 1) / 2;
				(pos.X, pos.Y) = (pos.Y, pos.X);
				return true;
			}
			private static bool SelectSetMethod() {
				int selected = -1;
				int tempselected = -1;
				MouseButton button;
				do {
					(XY pos, button) = MouseClickHandler.GetMouse();

					if (pos.Y >= 22 && pos.Y <= 28 && pos.X >= 2 && pos.X <= 41 && button == MouseButton.LeftButton) {
						Program.ReturnToMenu = true;
						throw new ReturnToMenu();
					}
					else if (pos.Y is >= 25 and <= 27) {
						if (pos.X is >= 47 and <= 60) selected = 0;
						else if (pos.X is >= 67 and <= 80) selected = 1;
						else selected = -1;
					}
					else selected = -1;

					if (tempselected != selected && selected != -1) {
						RenderSelectionSelected(true, selected);
						tempselected = selected;
					}

					if (tempselected != selected && tempselected != -1) {
						RenderSelectionSelected(false, tempselected);
						tempselected = selected;
					}

				} while (selected == -1 || button != MouseButton.LeftButton);

				return selected == 0;
			}
		}

		private class Bot : Player {
			private int flagHit = 0;// is hit
			private int flagDir = 0;// direction
			private XY rPos = new(-1, -1); //remembered position

			public Bot() {
				SetShips(true);
			}
			public override XY MakeMove() {
				XY pos = new();
				Random rnd = new();

				do {
					if (flagHit == 0) {
						pos.X = rnd.Next(10);
						pos.Y = rnd.Next(10);
						rPos = pos;
					}
					else {
						flagDir = flagHit == 1 ? rnd.Next(4) : flagDir;
						switch (flagDir) {
							case 0: pos.X = rPos.X - flagHit; pos.Y = rPos.Y; break;
							case 1: pos.X = rPos.X + flagHit; pos.Y = rPos.Y; break;
							case 2: pos.Y = rPos.Y - flagHit; pos.X = rPos.X; break;
							case 3: pos.Y = rPos.Y + flagHit; pos.X = rPos.X; break;
						}
					}
					if (pos >= (0, 0) && pos < (10, 10) && EnemyField.Grid[pos.X, pos.Y] == (int)Cell.Sea) return pos;
					else if (flagHit > 1) flagHit = -1;
				} while (true);
			}
			public override void Marking(XY position, bool hit) {
				if (human) RenderBotHits(position, hit);
				EnemyField.MarkHit(position, hit);
				if (hit) flagHit = flagHit >= 0 ? ++flagHit : --flagHit;
				else if (flagHit != 0) flagHit = flagHit == 1 ? 1 : -1;
			}
			public override void Marking(XY position, Ship ship) {
				if (human) RenderBotHits(position, true);
				EnemyField.MarkHit(position, true);
				EnemyField.ShipDestroyed(ship);
				flagHit = 0;
			}
		}



		private class Field {
			public int[,] Grid { get; }
			public Field() {
				Grid = new int[10, 10];
				for (int i = 0; i < 10; i++)
					for (int j = 0; j < 10; j++)
						Grid[i, j] = (int)Cell.Sea;
			}

			// Логика уничтожения кораблей
			public bool IsHit(XY pos) {
				if (Grid[pos.X, pos.Y] == (int)Cell.Boat) {
					MarkHit(pos, true);
					return true;
				}
				else {
					MarkHit(pos, false);
					return false;
				}
			}
			public void MarkHit(XY pos, bool isHit) {
				if (isHit) Grid[pos.X, pos.Y] = (int)Cell.Hit;
				else Grid[pos.X, pos.Y] = (int)Cell.Miss;
			}
			public void ShipDestroyed(Ship ship) {
				XY pos = ship.Cells[0];
				for (int i = -1; i <= ship.Cells.Count; i++) {
					for (int j = -1; j <= 1; j++) {
						int checkX = ship.Horizontal ? pos.X + j : pos.X + i;
						int checkY = ship.Horizontal ? pos.Y + i : pos.Y + j;

						if (checkX >= 0 && checkX < 10 && checkY >= 0 && checkY < 10)
							if (Grid[checkX, checkY] == (int)Cell.Sea)
								Grid[checkX, checkY] = (int)Cell.Miss;
					}
				}
			}
			// Логика установки кораблей
			public bool PlaceShip(XY pos, int length, bool horizontal, List<Ship> ships) {
				if (!CheckPlace(pos, length, horizontal))
					return false;

				List<XY> ship = [];
				if (horizontal)
					for (int i = 0; i < length; i++) {
						Grid[pos.X, pos.Y + i] = (int)Cell.Boat;
						ship.Add(new XY(pos.X, pos.Y + i));
					}
				else
					for (int i = 0; i < length; i++) {
						Grid[pos.X + i, pos.Y] = (int)Cell.Boat;
						ship.Add(new XY(pos.X + i, pos.Y));
					}
				ships.Add(new Ship(ship, horizontal));
				return true;
			}
			private bool CheckPlace(XY pos, int length, bool horizontal) {
				if (horizontal) {
					if (pos.Y + length > 10) return false;
				}
				else {
					if (pos.X + length > 10) return false;
				}
				for (int i = -1; i <= length; i++) {
					for (int j = -1; j <= 1; j++) {
						int checkX = horizontal ? pos.X + j : pos.X + i;
						int checkY = horizontal ? pos.Y + i : pos.Y + j;

						if (checkX >= 0 && checkX < 10 && checkY >= 0 && checkY < 10)
							if (Grid[checkX, checkY] == (int)Cell.Boat)
								return false;
					}
				}
				return true;
			}
		}

		private class Ship(IEnumerable<XY> cells, bool horizontal) {
			public List<XY> Cells { get; } = new List<XY>(cells);
			public List<XY> Hits { get; } = [];
			public bool Horizontal { get; } = horizontal;

			public bool IsDestroyed() {
				// Корабль считается уничтоженным, если все его клетки поражены
				return !Cells.Except(Hits).Any();
			}
			public bool RegisterHit(XY pos) {
				if (Cells.Contains(pos) && !Hits.Contains(pos))
					Hits.Add(pos);
				return IsDestroyed();
			}
		}



		private enum Cell {
			Sea,// пусто/море
			Miss,// промах
			Boat,// кораблик
			Hit// подбитый кораблик
		}
	}
}