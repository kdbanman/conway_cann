using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScalable : MonoBehaviour {

	private Vector3 lastGripLeftPosition, lastGripRightPosition;

	private bool leftGripDown, rightGripDown;
	
	private int lastScaledFrame;

	void Update () {
		float leftGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
		float rightGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);

		if (leftGrip > 0.5 && rightGrip > 0.5) {
			Vector3 gripLeftPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
			Vector3 gripRightPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

			if (Time.frameCount - lastScaledFrame < 5) {
				Vector3 midPoint = Vector3.Lerp(gripRightPosition, gripLeftPosition, 0.5f);
				Vector3 lastMidPoint = Vector3.Lerp(lastGripRightPosition, lastGripLeftPosition, 0.5f);
				transform.localPosition += midPoint - lastMidPoint;

				Vector3 delta = gripRightPosition - gripLeftPosition;
				Vector3 lastDelta = lastGripRightPosition - lastGripLeftPosition;
				transform.rotation *= Quaternion.FromToRotation(lastDelta, delta);

				float displacement = Vector3.Magnitude(delta);
				float lastDisplacement = Vector3.Magnitude(lastDelta);
				transform.localScale *= displacement / lastDisplacement;

			}

			lastGripLeftPosition = gripLeftPosition;
			lastGripRightPosition = gripRightPosition;
			lastScaledFrame = Time.frameCount;
		}
	}
}
