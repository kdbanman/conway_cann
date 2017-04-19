using UnityEngine;

// The view layer for an IAutomata.  Implementations should know about materials, colors,
// and how to use those things to render floating point state.
public abstract class StateCell : MonoBehaviour {

	// The state cell's location in its toroidal plane.
	public int x, y;

	// Called only when clicked/poked by user, wired up by CellTorus.
	// Naive action is to toggle between 1 and 0 for conways.
	// Arbitrary floating point state action "toggle" is less clear.
	public System.Action<int, int> onToggle;
	
	// Called by the CellTorus that holds the StateCell with the automata's floating
	// point state corresponding with the StateCell's (x, y) coordinate.
	public abstract void SetState(float state);

	// Called when something like a VR finger intersects the cell.
	void OnTriggerEnter(Collider other) {
		CellToggleCollider toggler = other.gameObject.GetComponent<CellToggleCollider>();
		if (toggler != null && onToggle != null) {
			onToggle(x, y);
		}
	}
}
