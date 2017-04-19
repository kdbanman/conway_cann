using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainableEnvironment : MonoBehaviour {

	private NeuralNetAutomata neuralAutomata;

	public int width, height;

	public bool autoRun;

	public int rngSeed, hiddenUnits;

	public float initializationMax;

	private GameObject[,] cells;

	private int lastFrameTriggerStepped;

	void Start () {
		neuralAutomata = new NeuralNetAutomata(width, height, hiddenUnits, rngSeed, initializationMax);

		neuralAutomata[12, 10] = 1;
		neuralAutomata[12, 11] = 1;
		neuralAutomata[12, 12] = 1;
		neuralAutomata[11, 12] = 1;
		neuralAutomata[10, 11] = 1;

		neuralAutomata[-1, 0] = 1;
		neuralAutomata[0, 0] = 1;
		neuralAutomata[1, 0] = 1;

		GetComponent<CellTorus>().automata = neuralAutomata;
		GetComponent<CellTorus>().SetPlaneSize(width, height);
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
					neuralAutomata[x, y] = neuralAutomata[x, y] == 0 ? 1 : 0;
				}
			}
		}

		if (autoRun ||
			Input.GetKey(KeyCode.Space) ||
			Input.GetKeyDown(KeyCode.RightArrow) ||
			OVRInput.GetDown(OVRInput.Button.Three | OVRInput.Button.One)) {

			neuralAutomata.Step();
		}

		float triggerPull = Mathf.Max(
			OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch),
			OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch)
		);

		if (triggerPull > 0.01f && (Time.frameCount - lastFrameTriggerStepped) >= Mathf.FloorToInt(Mathf.Lerp(120f, 1f, triggerPull))) {
			neuralAutomata.Step();
			lastFrameTriggerStepped = Time.frameCount;
		}
	}
}
