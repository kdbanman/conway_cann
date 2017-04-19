using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainableStateCell : StateCell {
	public Material liveReferenceMaterial, deadReferenceMaterial;

	private Material renderedMaterial;
	public override void SetState(float state) {
		if (renderedMaterial == null) {
			// Hack to give each tile a separate material
			renderedMaterial = new Material(liveReferenceMaterial);
			GetComponent<Renderer>().material = renderedMaterial;
		}

		GetComponent<Renderer>().material.color = Color.Lerp(deadReferenceMaterial.color, liveReferenceMaterial.color, state);
	}
}
