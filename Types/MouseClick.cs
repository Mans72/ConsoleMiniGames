using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiniGames.Types {

	internal readonly struct MouseClick(XY position, MouseButton button) {
		public XY Position { get; } = position;
		public MouseButton Button { get; } = button;
		public int PosX { get; } = position.X;
		public int PosY { get; } = position.Y;
	}
}
