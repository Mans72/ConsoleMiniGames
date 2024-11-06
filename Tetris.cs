
using System.Drawing;
using System.Threading;

namespace MiniGames_con {

	internal partial class Program {
		private const int h = 20;
		private const int w = 15;
		private static readonly object consoleLock = new();
		private static readonly ConsoleColor[] colors = [ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Magenta];


		static void Main() {

			Initialization();

			Game game = new(h, w);
			game.Start();

			Console.ReadKey();
		}



		private class Game(int h, int w) {
			private bool isRunning = true;
			private readonly Board board = new(h, w);
			private Tetromino nextTetromino = GenerateRandomTetromino();
			private Tetromino currentTetromino = GenerateRandomTetromino();
			private readonly CancellationTokenSource cancellationTokenSource1 = new();
			private readonly CancellationTokenSource cancellationTokenSource2 = new();

			public void Start() {
				//Task.Run(() => ReadMouse(cancellationTokenSource1.Token));
				Task.Run(() => ReadKeyy(cancellationTokenSource2.Token));
				RenderNextTetromindo(nextTetromino);

				while (isRunning) {

					ExecuteFall();

					Thread.Sleep(500); // Задержка перед падением следующей фигуры
				}
			}

			// Методы для движения фигуры
			private void ExecuteFall() {
				if (!board.CheckCollision(currentTetromino)) {
					board.RemoveTetromino(currentTetromino);
					currentTetromino.Move(0, 1);
					board.AddTetromino(currentTetromino);

					lock (consoleLock) board.Draw();
				}
				else NextTetromindo();
			}

			private void ExecuteMove(int dx, int dy) {
				if (board.ValidMove(currentTetromino, dx)) {
					board.RemoveTetromino(currentTetromino);
					currentTetromino.Move(dx, dy);
					board.AddTetromino(currentTetromino);

					lock (consoleLock) board.Draw();
				}
			}

			private void ExecuteMoveDown() {
				board.RemoveTetromino(currentTetromino);
				while (!board.CheckCollision(currentTetromino))
					currentTetromino.Move(0, 1);
				board.AddTetromino(currentTetromino);

				lock (consoleLock) board.Draw();
				NextTetromindo();
			}

			private void ExecuteRotate() {
				board.RemoveTetromino(currentTetromino);
				currentTetromino.Rotate();
				board.AddTetromino(currentTetromino);

				lock (consoleLock) board.Draw();
			}

			// Генерация случайной фигуры
			private static Tetromino GenerateRandomTetromino() {
				Random rand = new();
				return new Tetromino((rand.Next(350) % 7) switch {
					0 => shapeO,
					1 => shapeI,
					2 => shapeJ,
					3 => shapeL,
					4 => shapeZ,
					5 => shapeT,
					6 => shapeS,
				}, colors[rand.Next(colors.Length * 5) % colors.Length]);
			}

			private void NextTetromindo() {
				board.CheckFullLines();

				currentTetromino = nextTetromino;
				nextTetromino = GenerateRandomTetromino();
				RenderNextTetromindo(nextTetromino);

				foreach (var pos in currentTetromino.Shape)
					if (board.Grid[pos.Y, pos.X] != ConsoleColor.Black)
						End();

				board.AddTetromino(currentTetromino);
			}
			private void End() {
				isRunning = false;
			}

			private async Task ReadKeyy(CancellationToken token) {
				while (!token.IsCancellationRequested) {
					ConsoleKeyInfo key = Console.ReadKey(true);
					switch (key.Key) {
						case ConsoleKey.LeftArrow:
							ExecuteMove(-1, 0);
							break;
						case ConsoleKey.RightArrow:
							ExecuteMove(1, 0);
							break;
						case ConsoleKey.DownArrow:
							ExecuteMoveDown();
							break;
						case ConsoleKey.Spacebar:
							ExecuteRotate();
							break;
						default:
							await Task.Delay(100);
							break;
					}
					await Task.Delay(50);
				}
			}
			private async Task ReadMouse(CancellationToken token) {
				while (!token.IsCancellationRequested) {
					(XY pos, MouseButton button) = MouseClickHandler.GetMouseClick();
					if (pos.X >= 0 && pos.X < sidebar && pos.Y >= 0 && pos.Y < 5 && button == MouseButton.LeftButton) {
						cancellationTokenSource1.Cancel();
						cancellationTokenSource2.Cancel();
						Environment.Exit(0);
					}
					await Task.Delay(50);
				}
			}
		}


		private class Board(int height, int width) {
			private readonly int width = width;
			private readonly int height = height;
			//private readonly ConsoleColor[,] grid = new ConsoleColor[height, width];
			public ConsoleColor[,] Grid { get; } = new ConsoleColor[height, width];

			// Метод для отображения поля
			public void Draw() {
				RenderBoard(Grid);
			}

			public void CheckFullLines() {
				for (int i = height - 1; i >= 0; i--) {

					bool full = true;
					for (int j = 0; j < width; j++)
						if (Grid[i, j] == ConsoleColor.Black)
							full = false;

					if (full) {
						for (int j = 0; j < width; j++)
							Grid[i, j] = ConsoleColor.Black;
						RenderLineRemove(i);

						for (int k = i; k > 0; k--)
							for (int j = 0; j < width; j++)
								Grid[k, j] = Grid[k - 1, j];
					}
				}
			}

			// Метод для добавления фигуры на доску
			public void AddTetromino(Tetromino tetromino) {
				foreach (var pos in tetromino.Shape) {
					Grid[pos.Y, pos.X] = tetromino.Color;
				}
			}
			public void RemoveTetromino(Tetromino tetromino) {
				foreach (var pos in tetromino.Shape) {
					Grid[pos.Y, pos.X] = ConsoleColor.Black;
				}
			}

			// Метод для проверки столкновения
			public bool ValidMove(Tetromino tetromino, int dx) {
				foreach (var pos in tetromino.Shape) {
					int newX = pos.X + dx;

					if (tetromino.IsIn(newX, pos.Y))
						continue;
					if (newX < 0 || newX >= width || Grid[pos.Y, newX] != ConsoleColor.Black)
						return false;
				}
				return true;
			}
			public bool CheckCollision(Tetromino tetromino) {
				foreach (var pos in tetromino.Shape) {
					int newY = pos.Y + 1;

					if (tetromino.IsIn(pos.X, newY))
						continue;
					if (newY < 0 || newY >= height || Grid[newY, pos.X] != ConsoleColor.Black)
						return true;
				}
				return false;
			}
		}


		private class Tetromino {
			public ConsoleColor Color { get; }
			public XY[] Shape { get; private set; }
			private readonly XY[][] rotations; // Все возможные ротации
			private int currentRotation;

			public Tetromino(XY[][] rotations, ConsoleColor color) {
				Color = color;
				currentRotation = 0;
				this.rotations = rotations;
				Shape = new XY[rotations[0].Length];
				Array.Copy(rotations[0], Shape, rotations[0].Length);
			}

			// Метод для поворота фигуры
			public void Rotate() {
				// Сохраняем текущее смещение фигуры
				int currentX = Shape[0].X;
				int currentY = Shape[0].Y;

				// Вычисляем смещение для каждой точки (разницу между текущим положением и базовой формой)
				XY offset = new(currentX - rotations[currentRotation][0].X,
										 currentY - rotations[currentRotation][0].Y);

				// Изменяем ротацию
				currentRotation = (currentRotation + 1) % rotations.Length;

				// Применяем смещение к новой ротации
				Shape = new XY[rotations[currentRotation].Length];
				for (int i = 0; i < rotations[currentRotation].Length; i++) {
					Shape[i] = new(rotations[currentRotation][i].X + offset.X,
									rotations[currentRotation][i].Y + offset.Y);
				}
			}
			// Метод для перемещения фигуры
			public void Move(int dx, int dy) {
				for (int i = 0; i < Shape.Length; i++) {
					Shape[i].X += dx;
					Shape[i].Y += dy;
				}
			}

			public bool IsIn(int x, int y) {
				foreach (var pos in Shape)
					if (pos.X == x && pos.Y == y) return true;
				return false;
			}
		}



		private static readonly XY[][] shapeO = [
			[new(6, 0), new(7, 0), new(6, 1), new(7, 1)]  // форма квадрата
		];

		private static readonly XY[][] shapeI = [
			[new(5, 0), new(6, 0), new(7, 0), new(8, 0)], // горизонтальная форма
			[new(7, 0), new(7, 1), new(7, 2), new(7, 3)]  // вертикальная форма
		];

		private static readonly XY[][] shapeT = [
			[new(7, 0), new(6, 1), new(7, 1), new(8, 1)], // базовая форма
			[new(7, 0), new(7, 1), new(8, 1), new(7, 2)], // поворот на 90 градусов
			[new(6, 1), new(7, 1), new(8, 1), new(7, 2)], // поворот на 180 градусов
			[new(7, 0), new(6, 1), new(7, 1), new(7, 2)]  // поворот на 270 градусов
		];

		private static readonly XY[][] shapeJ = [
			[new(6, 0), new(6, 1), new(7, 1), new(8, 1)], // базовая форма
			[new(7, 0), new(8, 0), new(7, 1), new(7, 2)], // поворот на 90 градусов
			[new(6, 1), new(7, 1), new(8, 1), new(8, 2)], // поворот на 180 градусов
			[new(7, 0), new(7, 1), new(6, 2), new(7, 2)]  // поворот на 270 градусов
		];

		private static readonly XY[][] shapeL = [
			[new(8, 0), new(6, 1), new(7, 1), new(8, 1)], // базовая форма
			[new(7, 0), new(7, 1), new(7, 2), new(8, 2)], // поворот на 90 градусов
			[new(6, 1), new(7, 1), new(8, 1), new(6, 2)], // поворот на 180 градусов
			[new(6, 0), new(7, 0), new(7, 1), new(7, 2)]  // поворот на 270 градусов
		];

		private static readonly XY[][] shapeZ = [
			[new(6, 0), new(7, 0), new(7, 1), new(8, 1)], // базовая форма
			[new(7, 0), new(7, 1), new(6, 1), new(6, 2)]  // поворот на 90 градусов
		];

		private static readonly XY[][] shapeS = [
			[new(7, 0), new(8, 0), new(6, 1), new(7, 1)], // базовая форма
			[new(7, 0), new(7, 1), new(8, 1), new(8, 2)]  // поворот на 90 градусов
		];

	}
}



//private static async Task StartStopwatch(CancellationToken token2) {
//	int seconds = 0;

//	while (!token.IsCancellationRequested) {
//		seconds++;
//		lock (consoleLock)
//			RenderTime(seconds);
//		await Task.Delay(1000);
//	}
//}