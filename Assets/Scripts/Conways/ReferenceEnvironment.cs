using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceEnvironment : MonoBehaviour {

	private ConwaysAutomata conways;

	private TrainableEnvironment currentlyTraining;

	public int width, height;

	public bool autoRun;

	private GameObject[,] cells;

	private int lastFrameTriggerStepped;

	void Start () {
		conways = new ConwaysAutomata(width, height);

		conways[12, 10] = 1;
		conways[12, 11] = 1;
		conways[12, 12] = 1;
		conways[11, 12] = 1;
		conways[10, 11] = 1;

		conways[-1, 0] = 1;
		conways[0, 0] = 1;
		conways[1, 0] = 1;

		GetComponent<CellTorus>().automata = conways;
		GetComponent<CellTorus>().SetPlaneSize(width, height);
	}

	void Update () {
		if (TouchInput.ButtonTwoPressed || TouchInput.ButtonFourPressed) {
			conways.ResetEnvironment();
		}

		if (Input.GetKeyDown(KeyCode.A))
        {
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

			conways.Step();
		}

		float triggerPull = Mathf.Max(
			TouchInput.LeftTriggerSqueezeFraction,
			TouchInput.RightTriggerSqueezeFraction
		);

		if (triggerPull > 0.01f && (Time.frameCount - lastFrameTriggerStepped) >= Mathf.FloorToInt(Mathf.Lerp(120f, 1f, triggerPull))) {
			conways.Step();
			lastFrameTriggerStepped = Time.frameCount;
		}

		TrainIfPossible();
	}

    private void MirrorEnvironment(float[,] toMirror)
    {
        width = toMirror.GetLength(0);
        height = toMirror.GetLength(1);
        conways = new ConwaysAutomata(toMirror);
        GetComponent<CellTorus>().SetPlaneSize(width, height);
        GetComponent<CellTorus>().automata = conways;
    }

    void OnTriggerEnter(Collider other) {
		TrainableEnvironment trainable = other.gameObject.GetComponent<TrainableEnvironment>();

		if (trainable != null) {
			currentlyTraining = trainable;
		}
	}

	void OnTriggerExit(Collider other) {
		TrainableEnvironment trainable = other.gameObject.GetComponent<TrainableEnvironment>();

		if (trainable != null) {
			currentlyTraining = null;
		}
	}

	private void TrainIfPossible() {
		if (currentlyTraining != null) {
			SphereCollider thisCollider = GetComponent<SphereCollider>();
			SphereCollider otherCollider = currentlyTraining.GetComponent<SphereCollider>();

			float thisColliderRadius = thisCollider.radius * Mathf.Max(thisCollider.transform.lossyScale.x, thisCollider.transform.lossyScale.y, thisCollider.transform.lossyScale.z);
			float otherColliderRadius = otherCollider.radius * Mathf.Max(otherCollider.transform.lossyScale.x, otherCollider.transform.lossyScale.y, otherCollider.transform.lossyScale.z);

			float separationDistance = Vector3.Distance(thisCollider.transform.position, otherCollider.transform.position);
			float intersectionDistance = thisColliderRadius + otherColliderRadius - separationDistance;
			float intersectionFraction = intersectionDistance / otherColliderRadius;

			float learningRate = Mathf.Min(intersectionFraction, 1f);

			currentlyTraining.TrainFrom(conways, learningRate);
		}
	}
}
