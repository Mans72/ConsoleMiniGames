﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiniGames {

	internal abstract class BaseView<T> where T : BaseController {
		protected string Title = string.Empty;
		protected int windowHeight;
		protected int windowWidth;

		protected abstract T Context { get; set; }

		protected void ConfigureWindow() {
			Console.Title = Title;
			Console.CursorVisible = false;
			Console.SetWindowSize(windowWidth, windowHeight);
			//Console.SetBufferSize(WindowWidth, WindowHeight);
			Console.ResetColor();
			Console.Clear();
		}

		protected abstract void Initialization();
	}
}
