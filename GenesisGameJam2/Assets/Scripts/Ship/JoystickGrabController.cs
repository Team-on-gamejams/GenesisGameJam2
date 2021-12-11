using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

public class JoystickGrabController : MonoBehaviour {
	public bool IsGrabbed => grabbable.isGrabbed;
	public Vector2 Value => value;

	[Header("Balance"), Space]
	[SerializeField] Vector3 bounds1 = new Vector3(-90, 0, -90);
	[SerializeField] Vector3 bounds2 = new Vector3(0, 0, 90);

	[Header("Refs"), Space]
	public GrabableWithCallbacks grabbable;
	[SerializeField] Transform headRenderer;
	[SerializeField] Transform headGrab;


	Vector2 value = Vector3.zero;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!grabbable)
			grabbable = GetComponentInChildren<GrabableWithCallbacks>();
	}
#endif

	private void OnEnable() {
		grabbable.onGrabBegin += OnGrabStart;
		grabbable.onGrabEnd += OnGrabEnd;
	}

	private void OnDisable() {
		grabbable.onGrabBegin -= OnGrabStart;
		grabbable.onGrabEnd -= OnGrabEnd;
	}

	private void Update() {
		if (IsGrabbed) {
			transform.localRotation = ClampRotation(grabbable.grabbedBy.transform.parent.parent.localRotation, bounds1, bounds2);
		}
	}

	void OnGrabStart() {

	}

	void OnGrabEnd() {
		headGrab.transform.localRotation = headRenderer.transform.localRotation;
		headGrab.transform.position = headRenderer.transform.position;
		transform.localEulerAngles = Vector2.zero;
		value.x = value.y = 0;
	}

	Quaternion ClampRotation(Quaternion q, Vector3 boundsMin, Vector3 boundsMax) {
		Vector3 angles = q.eulerAngles;

		angles.x += 15;

		q = Quaternion.Euler(angles);

		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
		angleX = Mathf.Clamp(angleX, boundsMin.x, boundsMax.x);
		q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

		float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
		angleY = Mathf.Clamp(angleY, boundsMin.y, boundsMax.y);
		q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

		float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
		angleZ = Mathf.Clamp(angleZ, boundsMin.z, boundsMax.z);
		q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);

		value.x = (angleX) / boundsMax.x;
		value.y = angleZ / boundsMin.x;

		return q;
	}
}
