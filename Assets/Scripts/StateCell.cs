using UnityEngine;

public class StateCell : MonoBehaviour {

	public Material liveMaterial, deadMaterial;

	public int x, y;

	public System.Action<int, int> onToggle;

	public void SetState(float state) {
		GetComponent<Renderer>().material = state == 1 ? liveMaterial : deadMaterial;
	}

	void OnTriggerEnter(Collider other) {
		CellToggleCollider toggler = other.gameObject.GetComponent<CellToggleCollider>();
		if (toggler != null && onToggle != null) {
			onToggle(x, y);
		}
	}
}
