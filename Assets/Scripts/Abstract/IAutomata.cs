using System.Collections;
using System.Collections.Generic;

public interface IAutomata {
		void Step();
		float this[int x, int y] { get; set; }
		System.Action<int, int> onToggle { get; }
}
