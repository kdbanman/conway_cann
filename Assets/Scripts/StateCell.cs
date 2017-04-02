using UnityEngine;

public class StateCell : MonoBehaviour {

	public Material liveMaterial, deadMaterial;

	public int x, y;

	public void SetState(float state) {
		GetComponent<Renderer>().material = state == 1 ? liveMaterial : deadMaterial;
	}
}
