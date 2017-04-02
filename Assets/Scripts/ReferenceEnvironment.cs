using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceEnvironment : MonoBehaviour {

	private Conways conways;

	public int width, height;

	public bool autoRun;

	private GameObject[,] cells;

	private int lastFrameTriggerStepped;

	void Start () {
		conways = new Conways(width, height);

		conways[12, 10] = 1;
		conways[12, 11] = 1;
		conways[12, 12] = 1;
		conways[11, 12] = 1;
		conways[10, 11] = 1;

		conways[-1, 0] = 1;
		conways[0, 0] = 1;
		conways[1, 0] = 1;

		GetComponent<CellTorus>().SetPlaneSize(width, height);
		GetComponent<CellTorus>().automata = conways;
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				StateCell clickedCell = hit.transform.gameObject.GetComponent<StateCell>();
				if (clickedCell != null) {
					int x = clickedCell.x;
					int y = clickedCell.y;
					conways[x, y] = conways[x, y] == 0 ? 1 : 0;
				}
			}
		}

		if (autoRun ||
			Input.GetKey(KeyCode.Space) ||
			Input.GetKeyDown(KeyCode.RightArrow) ||
			OVRInput.GetDown(OVRInput.Button.Three | OVRInput.Button.One)) {

			conways.Step();
		}

		float triggerPull = Mathf.Max(
			OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch),
			OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch)
		);

		if (triggerPull > 0.01f && (Time.frameCount - lastFrameTriggerStepped) >= Mathf.FloorToInt(Mathf.Lerp(120f, 1f, triggerPull))) {
			conways.Step();
			lastFrameTriggerStepped = Time.frameCount;
		}
	}
}
