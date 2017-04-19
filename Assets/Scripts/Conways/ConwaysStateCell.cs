using System;
using UnityEngine;

public class ConwaysStateCell : StateCell {

	public Material liveMaterial, deadMaterial;

	public override void SetState(float state) {
		GetComponent<Renderer>().material = state == 1 ? liveMaterial : deadMaterial;
	}
}
