
namespace MiniGames_con {

	internal static partial class TicTacToe {
		private static int wins = 0;
		private static int losses = 0;
		private static int draws = 0;

		public static void Start() {

			Initialization();

			do {
				// сделать выбор символа
				Game game = new();
				game.Start(ref wins, ref losses, ref draws);

				RenderScore(wins, losses, draws);

				(XY pos, MouseButton button) = MouseClickHandler.GetMouseClick();
				if (pos.X >= 0 && pos.Y >= 0 && pos.X <= sidebar && pos.Y <= 4 && button == MouseButton.LeftButton)
					Program.ReturnToMenu = true;
			} while (!Program.ReturnToMenu);
		}




		private class Game {
			private bool isRunning;
			private readonly Board board;
			private Player currentPlayer;
			private readonly Player player;
			private readonly Player bot;

			public Game() {
				isRunning = true;
				board = new Board();
				player = new Human('X');
				bot = new Bot('O');
				Random rnd = new();
				currentPlayer = rnd.Next(100) < 50 ? player : bot;
			}

			public void Start(ref int wins, ref int losses, ref int draws) {
				RenderClear();
				while (isRunning && !Program.ReturnToMenu) {
					XY position = currentPlayer.MakeMove(board);

					board.Update(position, currentPlayer.Symbol);
					(bool win, int winCase) = board.CheckWinner(currentPlayer.Symbol);
					if (win) {
						if (currentPlayer == player) wins++;
						else losses++;
						EndGame(true, winCase);
					}
					else if (board.IsFull()) {
						draws++;
						EndGame(false, 0);
					}
					else SwitchPlayer();
				}
			}

			private void SwitchPlayer() {
				currentPlayer = currentPlayer == player ? bot : player;
			}
			private void EndGame(bool win, int winCase) {
				isRunning = false;
				if (win) {
					RenderWinLine(winCase);
					RenderEndMessage(win, currentPlayer == player);
				}
				else RenderEndMessage(false, false);
			}
		}



		private class Board {
			public char[,] Grid { get; private set; }
			public Board() {
				Grid = new char[3, 3];
				for (int i = 0; i < 9; i++)
					Grid[i / 3, i % 3] = ' ';
			}

			public void Update(XY position, char symbol) {
				Grid[position.X, position.Y] = symbol;
				DrawElement(symbol, position.Y, position.X);
			}
			public (bool, int) CheckWinner(char symbol) {
				if (Grid[0, 0] == symbol && Grid[1, 1] == symbol && Grid[2, 2] == symbol)
					return (true, 7);
				if (Grid[0, 2] == symbol && Grid[1, 1] == symbol && Grid[2, 0] == symbol)
					return (true, 8);
				for (int i = 0; i < 3; i++) {
					if (Grid[i, 0] == symbol && Grid[i, 1] == symbol && Grid[i, 2] == symbol)
						return (true, i + 1);
					if (Grid[0, i] == symbol && Grid[1, i] == symbol && Grid[2, i] == symbol)
						return (true, i + 4);
				}
				return (false, 0);
			}
			public bool IsFull() {
				foreach (char c in Grid)
					if (c == ' ')
						return false;
				return true;
			}
		}



		private abstract class Player(char symbol) {
			public char Symbol { get; } = symbol;
			public abstract XY MakeMove(Board board);
		}

		private class Human(char symbol) : Player(symbol) {
			public override XY MakeMove(Board board) {
				XY pos;
				MouseButton button;
				do {
					do {
						(pos, button) = MouseClickHandler.GetMouseClick();
						if (pos.X >= 0 && pos.Y >= 0 && pos.X <= 17 && pos.Y <= 4) {
							Program.ReturnToMenu = true;
							throw new ReturnToMenu();
						}
					} while (!IsValidPos(ref pos) || button != MouseButton.LeftButton);

					Thread.Sleep(16);
				} while (board.Grid[pos.X, pos.Y] != ' ');
				return pos;
			}

			private static bool IsValidPos(ref XY pos) {
				if (pos.Y < 2 || pos.Y > 18 || pos.Y == 7 || pos.Y == 13) return false;
				if (pos.X < 17 + 4 || pos.X > 17 + 37 || (pos.X - 17) / 2 == 7 || (pos.X - 17) / 2 == 13) return false;
				pos.Y /= 7; pos.X -= 17; pos.X /= 14;
				(pos.X, pos.Y) = (pos.Y, pos.X);
				return true;
			}
		}

		private class Bot(char symbol) : Player(symbol) {
			public override XY MakeMove(Board board) {
				// Попытка сделать выигрышный ход
				XY move = TryCompleteLine(board, Symbol);
				if (move.X != -1) return move;
				// Попытка заблокировать ход игрока
				move = TryBlockPlayer(board);
				if (move.X != -1) return move;
				// Если не удалось выиграть или заблокировать, делаем случайный ход
				return MakeRandomMove(board);
			}

			private XY TryBlockPlayer(Board board) {
				if (Symbol == 'X')
					return TryCompleteLine(board, 'O');
				else
					return TryCompleteLine(board, 'X');
			}
			private static XY TryCompleteLine(Board board, char symbol) {
				for (int i = 0; i < 3; i++) {
					// Проверка трёх строк
					if (board.Grid[i, 0] == symbol && board.Grid[i, 1] == symbol && board.Grid[i, 2] == ' ')
						return new XY(i, 2);
					if (board.Grid[i, 1] == symbol && board.Grid[i, 2] == symbol && board.Grid[i, 0] == ' ')
						return new XY(i, 0);
					if (board.Grid[i, 0] == symbol && board.Grid[i, 2] == symbol && board.Grid[i, 1] == ' ')
						return new XY(i, 1);
					// Проверка трёх столбцов
					if (board.Grid[0, i] == symbol && board.Grid[1, i] == symbol && board.Grid[2, i] == ' ')
						return new XY(2, i);
					if (board.Grid[1, i] == symbol && board.Grid[2, i] == symbol && board.Grid[0, i] == ' ')
						return new XY(0, i);
					if (board.Grid[0, i] == symbol && board.Grid[2, i] == symbol && board.Grid[1, i] == ' ')
						return new XY(1, i);
				}
				// Проверка диагоналей
				if (board.Grid[0, 0] == symbol && board.Grid[1, 1] == symbol && board.Grid[2, 2] == ' ')
					return new XY(2, 2);
				if (board.Grid[1, 1] == symbol && board.Grid[2, 2] == symbol && board.Grid[0, 0] == ' ')
					return new XY(0, 0);
				if (board.Grid[0, 0] == symbol && board.Grid[2, 2] == symbol && board.Grid[1, 1] == ' ')
					return new XY(1, 1);
				if (board.Grid[0, 2] == symbol && board.Grid[1, 1] == symbol && board.Grid[2, 0] == ' ')
					return new XY(2, 0);
				if (board.Grid[1, 1] == symbol && board.Grid[2, 0] == symbol && board.Grid[0, 2] == ' ')
					return new XY(0, 2);
				if (board.Grid[0, 2] == symbol && board.Grid[2, 0] == symbol && board.Grid[1, 1] == ' ')
					return new XY(1, 1);

				return new XY(-1, -1); // Нет выигрышного или блокирующего хода
			}
			private static XY MakeRandomMove(Board board) {
				Random rand = new();
				int move;
				do {
					move = rand.Next(0, 9);
				} while (board.Grid[move / 3, move % 3] != ' ');
				return new XY(move / 3, move % 3);
			}
		}
	}
}