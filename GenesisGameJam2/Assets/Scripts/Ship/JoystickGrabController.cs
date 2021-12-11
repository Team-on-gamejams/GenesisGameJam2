using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

public class JoystickGrabController : OVRGrabbable {
	public bool IsGrabbed { get; set; }

	[Header("Refs"), Space]
	[SerializeField] OVRGrabbable grabbable;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!grabbable)
			grabbable = GetComponent<OVRGrabbable>();
	}
#endif


	public override void GrabBegin(OVRGrabber hand, Collider grabPoint) {
		IsGrabbed = true;
		base.GrabBegin(hand, grabPoint);
	}

	public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity) {
		IsGrabbed = false;
		base.GrabEnd(linearVelocity, angularVelocity);
	}
}
