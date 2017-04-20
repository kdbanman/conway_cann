using UnityEngine;

public class TouchScalable : MonoBehaviour {


	public GameObject leftHand, rightHand;


	// static so that only one thing can ever be grabbed at a time
	private static bool leftGrabbedGlobal, rightGrabbedGlobal;

	private bool leftHandInCollider, rightHandInCollider;

	private Vector3 lastGripLeftPosition, lastGripRightPosition;

	private Transform ungrabbedParent;
	private GameObject scaleCenter;

	void OnTriggerEnter(Collider other) {
		GrabHandCollider grabber = other.gameObject.GetComponent<GrabHandCollider>();
		
		if (grabber != null) {
			switch (grabber.hand)
			{
				case GrabHandCollider.Hand.LEFT:
					leftHandInCollider = true;
					break;
				case GrabHandCollider.Hand.RIGHT:
					rightHandInCollider = true;
					break;
			}
		}
	}

	void OnTriggerExit(Collider other) {
		GrabHandCollider grabber = other.gameObject.GetComponent<GrabHandCollider>();
		
		if (grabber != null) {
			float leftGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
			float rightGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);

			// only let go if we've exited the trigger _and_ we've let go of the grip.
			if (grabber.hand == GrabHandCollider.Hand.LEFT && leftGrip < 0.5f) {
				leftHandInCollider = false;
			}
			if (grabber.hand == GrabHandCollider.Hand.RIGHT && rightGrip < 0.5f) {
				rightHandInCollider = false;
			}
		}
	}
	
	void Update () {
		float leftGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
		float rightGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);

		Vector3 gripLeftPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
		Vector3 gripRightPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

		if (leftGrip > 0.5 && rightGrip > 0.5 && leftHandInCollider && rightHandInCollider && scaleCenter != null) {
			leftGrabbedGlobal = true;
			rightGrabbedGlobal = true;

			Vector3 delta = gripRightPosition - gripLeftPosition;
			Vector3 lastDelta = lastGripRightPosition - lastGripLeftPosition;

			float displacement = Vector3.Magnitude(delta);
			float lastDisplacement = Vector3.Magnitude(lastDelta);

			scaleCenter.transform.localScale *= displacement / lastDisplacement;
		} else if (leftGrip > 0.5 && leftHandInCollider && !leftGrabbedGlobal) {
			leftGrabbedGlobal = true;
			rightGrabbedGlobal = false;

			SetScaleCenter(leftHand);
		} else if (rightGrip > 0.5 && rightHandInCollider && !rightGrabbedGlobal) {
			leftGrabbedGlobal = false;
			rightGrabbedGlobal = true;

			SetScaleCenter(rightHand);
		} else {
			leftGrabbedGlobal = false;
			rightGrabbedGlobal = false;

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
