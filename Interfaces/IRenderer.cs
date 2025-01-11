namespace ConsoleMiniGames.Interfaces {

	internal interface IRenderer {
		void RenderBoard<T>(IBoard<T> board);
		void RenderMessage(string message);
	}
}