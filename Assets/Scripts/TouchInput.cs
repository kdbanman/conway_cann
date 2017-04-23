using UnityEngine;

public class TouchInput
{

    public static Vector3 LeftControllerPosition { 
		get {
			#if UNITY_EDITOR_WIN
			return OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
			#else
			return Vector3.zero;
			#endif
		}
	}

    public static Vector3 RightControllerPosition { 
		get {
			#if UNITY_EDITOR_WIN
			return OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
			#else
			return Vector3.zero;
			#endif
		}
	}

    public static float LeftGripSqueezeFraction { 
		get {
			#if UNITY_EDITOR_WIN
			return OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
			#else
			return 0f;
			#endif
		}
	}

    public static float RightGripSqueezeFraction { 
		get {
			#if UNITY_EDITOR_WIN
			return OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
			#else
			return 0f;
			#endif
		}
	}

	public static float LeftTriggerSqueezeFraction { 
		get {
			#if UNITY_EDITOR_WIN
			return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
			#else
			return 0f;
			#endif
		}
	}

	public static float RightTriggerSqueezeFraction { 
		get {
			#if UNITY_EDITOR_WIN
			return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
			#else
			return 0f;
			#endif
		}
	}

	public static bool ButtonOnePressed {
		get {
			#if UNITY_EDITOR_WIN
			return OVRInput.GetDown(OVRInput.Button.One);
			#else
			return false;
			#endif
		}
	}

	public static bool ButtonTwoPressed {
		get {
			#if UNITY_EDITOR_WIN
			return OVRInput.GetDown(OVRInput.Button.Two);
			#else
			return false;
			#endif
		}
	}

	public static bool ButtonThreePressed {
		get {
			#if UNITY_EDITOR_WIN
			return OVRInput.GetDown(OVRInput.Button.Three);
			#else
			return false;
			#endif
		}
	}

	public static bool ButtonFourPressed {
		get {
			#if UNITY_EDITOR_WIN
			return OVRInput.GetDown(OVRInput.Button.Four);
			#else
			return false;
			#endif
		}
	}
}
