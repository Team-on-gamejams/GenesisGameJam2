using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

public class GrabableWithCallbacks : OVRGrabbable
{
	public Action onGrabBegin;
	public Action onGrabEnd;

	public override void GrabBegin(OVRGrabber hand, Collider grabPoint) {
		base.GrabBegin(hand, grabPoint);
		onGrabBegin?.Invoke();
	}

	public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity) {
		base.GrabEnd(linearVelocity, angularVelocity);
		onGrabEnd?.Invoke();
	}
}
