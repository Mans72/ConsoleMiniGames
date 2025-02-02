using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiniGames {

	internal abstract class BaseController {

		//public delegate void SelectionChangedEventHandler(object? sender, string e);
		//public event SelectionChangedEventHandler? SelectionChanged;

		public event PropertyChangedEventHandler? ElementSelected;
		public event PropertyChangedEventHandler? ElementUnselected;

		public void OnSelection([CallerMemberName] string propertyName = "") {
			ElementSelected?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void OnUnselection([CallerMemberName] string propertyName = "") {
			ElementUnselected?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
