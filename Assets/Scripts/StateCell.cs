using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCell : MonoBehaviour {

	public Material liveMaterial, deadMaterial;

	public int x, y;

	public void SetState(int state) {
		GetComponent<Renderer>().material = state == 1 ? liveMaterial : deadMaterial;
	}
}
