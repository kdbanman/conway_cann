using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCube : MonoBehaviour {

	public Material liveMaterial, deadMaterial;

	public void SetState(int state) {
		GetComponent<Renderer>().material = state == 1 ? liveMaterial : deadMaterial;
	}
}
