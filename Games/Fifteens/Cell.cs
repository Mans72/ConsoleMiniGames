using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiniGames.Games.Fifteens {

	internal struct Cell(int x, int y, int number, bool isMoveable) {
		public int X { get; set; } = x;
		public int Y { get; set; } = y;
		public int Number { get; set; } = number;
		public bool IsMoveable { get; set; } = isMoveable;

		public static bool operator ==(Cell left, Cell right) {
			return left.X == right.X &&
				   left.Y == right.Y &&
				   left.Number == right.Number &&
				   left.IsMoveable == right.IsMoveable;
		}

		public static bool operator !=(Cell left, Cell right) {
			return !(left == right);
		}
	}
}
