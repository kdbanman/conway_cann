using UnityEngine;

public class TouchScalable : MonoBehaviour {

	public GameObject leftHand, rightHand;

	private Vector3 lastGripLeftPosition, lastGripRightPosition;

	private Transform ungrabbedParent;
	private GameObject scaleCenter;
	
	void Update () {
		float leftGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
		float rightGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);

		Vector3 gripLeftPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
		Vector3 gripRightPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

		if (leftGrip > 0.5 && rightGrip > 0.5 && scaleCenter != null) {
			Vector3 delta = gripRightPosition - gripLeftPosition;
			Vector3 lastDelta = lastGripRightPosition - lastGripLeftPosition;

			float displacement = Vector3.Magnitude(delta);
			float lastDisplacement = Vector3.Magnitude(lastDelta);

			scaleCenter.transform.localScale *= displacement / lastDisplacement;
		} else if (leftGrip > 0.5) {
				SetScaleCenter(leftHand);
		} else if (rightGrip > 0.5) {
				SetScaleCenter(rightHand);
		} else {
			ResetScaleCenter();
		}

		lastGripLeftPosition = gripLeftPosition;
		lastGripRightPosition = gripRightPosition;
	}

	private void SetScaleCenter(GameObject hand) {
		if (ungrabbedParent == null) {
			ungrabbedParent = transform.parent;
		}
		scaleCenter = new GameObject();
		scaleCenter.transform.position = hand.transform.position;
		transform.parent = scaleCenter.transform;
		scaleCenter.transform.parent = hand.transform;
	}

	private void ResetScaleCenter() {
		transform.parent = ungrabbedParent;
		ungrabbedParent = null;
		Destroy(scaleCenter);
	}
}
