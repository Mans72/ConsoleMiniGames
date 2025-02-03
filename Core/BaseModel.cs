using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ConsoleMiniGames.Interfaces;

namespace ConsoleMiniGames {

	internal abstract class BaseModel {
		public delegate void BoardRenderer(IBoard board);

		public event BoardRenderer? RenderBoard;

	}
}
