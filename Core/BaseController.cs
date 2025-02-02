using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiniGames {

	internal interface IBaseController {
		public void Start();
	}

	internal abstract class BaseController<T> : IBaseController where T : BaseView {

		protected abstract T View { get; set; }

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

		public abstract void Start();
	}
}
