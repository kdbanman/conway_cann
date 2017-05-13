using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainableEnvironment : MonoBehaviour {

	private NeuralNetAutomata neuralAutomata;

	public int width, height;

	public bool autoRun;

	public int rngSeed, hiddenUnits;

	public float initializationMax;

	public GameObject batchCostPanel;

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
		if (TouchInput.ButtonTwoPressed || TouchInput.ButtonFourPressed) {
			neuralAutomata.ResetEnvironment();
		}

		if (Input.GetKeyDown(KeyCode.A)) {
			float[,] toMirror = EnvironmentStatePresets.Get("Glider Land");
			MirrorEnvironment(toMirror);
		}

		if (Input.GetKeyDown(KeyCode.O))
        {
            float[,] toMirror = EnvironmentStatePresets.Get("Gabriel's P138");
            MirrorEnvironment(toMirror);
        }

		if (autoRun ||
			Input.GetKey(KeyCode.Space) ||
			Input.GetKeyDown(KeyCode.RightArrow) ||
			TouchInput.ButtonOnePressed ||
			TouchInput.ButtonThreePressed) {

			neuralAutomata.Step();
		}

		float triggerPull = Mathf.Max(
			TouchInput.LeftTriggerSqueezeFraction,
			TouchInput.RightTriggerSqueezeFraction
		);

		if (triggerPull > 0.01f && (Time.frameCount - lastFrameTriggerStepped) >= Mathf.FloorToInt(Mathf.Lerp(120f, 1f, triggerPull))) {
			neuralAutomata.Step();
			lastFrameTriggerStepped = Time.frameCount;
		}
	}

    private void MirrorEnvironment(float[,] toMirror)
    {
		width = toMirror.GetLength(0);
		height = toMirror.GetLength(1);
		neuralAutomata.MirrorEnvironment(toMirror);
		GetComponent<CellTorus>().SetPlaneSize(width, height);
    }

    public void TrainFrom(AbstractAutomata targetAutomata, float learningRate) {
		neuralAutomata.TrainFrom(targetAutomata.TrainingBatch, learningRate);
		batchCostPanel.GetComponent<BatchCostPanel>().Add(neuralAutomata.PreviousAvgBatchCost);
	}
}
