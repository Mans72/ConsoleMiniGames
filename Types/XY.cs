namespace ConsoleMiniGames.Types {

	internal struct XY(int x, int y) : IEquatable<XY> {
		internal int X = x;
		internal int Y = y;

		public readonly bool Equals(XY other) {
			return this == other;
		}
		public readonly override bool Equals(object? obj) {
			return obj is XY bulls && Equals(bulls);
		}
		public readonly override int GetHashCode() {
			return HashCode.Combine(X, Y);
		}

		public readonly bool XInBtwn(int a, int b) {
			return X >= a && X <= b;
		}
		public readonly bool YInBtwn(int a, int b) {
			return Y >= a && Y <= b;
		}
		public readonly bool XBtwn(int a, int b) {
			return X > a && X < b;
		}
		public readonly bool YBtwn(int a, int b) {
			return Y > a && Y < b;
		}

		public static bool operator ==(XY left, XY right) {
			return left.X == right.X && left.Y == right.Y;
		}
		public static bool operator ==(XY left, (int x, int y) right) {
			return left.X == right.x && left.Y == right.y;
		}
		public static bool operator !=(XY left, XY right) {
			return left.X != right.X || left.Y != right.Y;
		}
		public static bool operator !=(XY left, (int x, int y) right) {
			return left.X != right.x || left.Y != right.y;
		}

		public static bool operator <(XY left, XY right) {
			return left.X < right.X && left.Y < right.Y;
		}
		public static bool operator <(XY left, (int x, int y) right) {
			return left.X < right.x && left.Y < right.y;
		}
		public static bool operator >(XY left, XY right) {
			return left.X > right.X && left.Y > right.Y;
		}
		public static bool operator >(XY left, (int x, int y) right) {
			return left.X > right.x && left.Y > right.y;
		}

		public static bool operator <=(XY left, XY right) {
			return left.X <= right.X && left.Y <= right.Y;
		}
		public static bool operator <=(XY left, (int x, int y) right) {
			return left.X <= right.x && left.Y <= right.y;
		}
		public static bool operator >=(XY left, XY right) {
			return left.X >= right.X && left.Y >= right.Y;
		}
		public static bool operator >=(XY left, (int x, int y) right) {
			return left.X >= right.x && left.Y >= right.y;
		}
	}
}