using ConsoleMiniGames.Interfaces;
using ConsoleMiniGames.Types;

namespace ConsoleMiniGames.Games.Fifteens {

	internal class Board {
		public int[,] Grid { get; private set; }

		public Board() {
			Grid = new int[4, 4];
			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++)
					Grid[i, j] = i * 4 + j;
			Sources.ArrayShuffle(Grid);
		}

		public void Swap(XY pos) {
			if (pos.X != 3 && Grid[pos.X + 1, pos.Y] == 0)
				(Grid[pos.X, pos.Y], Grid[pos.X + 1, pos.Y]) = (Grid[pos.X + 1, pos.Y], Grid[pos.X, pos.Y]);
			else if (pos.X != 0 && Grid[pos.X - 1, pos.Y] == 0)
				(Grid[pos.X, pos.Y], Grid[pos.X - 1, pos.Y]) = (Grid[pos.X - 1, pos.Y], Grid[pos.X, pos.Y]);
			else if (pos.Y != 3 && Grid[pos.X, pos.Y + 1] == 0)
				(Grid[pos.X, pos.Y], Grid[pos.X, pos.Y + 1]) = (Grid[pos.X, pos.Y + 1], Grid[pos.X, pos.Y]);
			else if (pos.Y != 0 && Grid[pos.X, pos.Y - 1] == 0)
				(Grid[pos.X, pos.Y], Grid[pos.X, pos.Y - 1]) = (Grid[pos.X, pos.Y - 1], Grid[pos.X, pos.Y]);
		}

		public bool IsMoveable(int x, int y) {
			if (x != 3 && Grid[x + 1, y] == 0) return true;
			else if (x != 0 && Grid[x - 1, y] == 0) return true;
			else if (y != 3 && Grid[x, y + 1] == 0) return true;
			else if (y != 0 && Grid[x, y - 1] == 0) return true;
			else return false;
		}

		public bool CheckWin() {
			for (int i = 0; i < 15; i++)
				if (Grid[i / 4, i % 4] != i + 1)
					return false;
			return true;
		}

		public void Cheat() {
			Grid = new int[,] {
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 10, 11, 12 },
				{ 13, 14, 0, 15 }
			};
		}
	}

	internal class Board2 : IBoard<Cell> {
		public Cell[,] Grid { get; private set; }

		public Board2() {
			Grid = new Cell[4, 4];
			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++)
					Grid[i, j].Number = i * 4 + j;
			Sources.ArrayShuffle(Grid);

			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++) {
					Grid[i, j].Y = i;
					Grid[i, j].X = j;
					Grid[i, j].IsMoveable = IsMoveable(i, j);
				}
		}

		public void Swap(Cell cell) {
			if (cell.X != 3 && Grid[cell.X + 1, cell.Y].Number == 0)
				(Grid[cell.X, cell.Y].Number, Grid[cell.X + 1, cell.Y].Number) = (Grid[cell.X + 1, cell.Y].Number, Grid[cell.X, cell.Y].Number);
			else if (cell.X != 0 && Grid[cell.X - 1, cell.Y].Number == 0)
				(Grid[cell.X, cell.Y].Number, Grid[cell.X - 1, cell.Y].Number) = (Grid[cell.X - 1, cell.Y].Number, Grid[cell.X, cell.Y].Number);
			else if (cell.Y != 3 && Grid[cell.X, cell.Y + 1].Number == 0)
				(Grid[cell.X, cell.Y].Number, Grid[cell.X, cell.Y + 1].Number) = (Grid[cell.X, cell.Y + 1].Number, Grid[cell.X, cell.Y].Number);
			else if (cell.Y != 0 && Grid[cell.X, cell.Y - 1].Number == 0)
				(Grid[cell.X, cell.Y].Number, Grid[cell.X, cell.Y - 1].Number) = (Grid[cell.X, cell.Y - 1].Number, Grid[cell.X, cell.Y].Number);

			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++)
					Grid[i, j].IsMoveable = IsMoveable(i, j);
		}

		public bool CheckWin() {
			for (int i = 0; i < 15; i++)
				if (Grid[i / 4, i % 4].Number != i + 1)
					return false;
			return true;
		}

		public void Cheat() {
			Grid = new Cell[4, 4];
			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++)
					Grid[i, j].Number = i * 4 + j;
			Grid[3, 2].Number = 0;
			Grid[3, 3].Number = 15;
		}

		private bool IsMoveable(int x, int y) {
			if (x != 3 && Grid[x + 1, y].Number == 0) return true;
			else if (x != 0 && Grid[x - 1, y].Number == 0) return true;
			else if (y != 3 && Grid[x, y + 1].Number == 0) return true;
			else if (y != 0 && Grid[x, y - 1].Number == 0) return true;
			else return false;
		}
	}
}