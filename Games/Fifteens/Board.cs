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
}