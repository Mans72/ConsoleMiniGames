using ConsoleMiniGames.Types;

namespace ConsoleMiniGames.Interfaces {

	internal interface IBoard<T> {
		public T[,] Grid { get; }
		public bool IsValidMove(XY position);
	}
}