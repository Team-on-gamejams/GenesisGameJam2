using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickLinearMove : MonoBehaviour {
	public bool IsGrabbed => grabbable.isGrabbed;
	public float Value => value;

	[Header("Balance"), Space]
	[SerializeField] float maxMove;

	[Header("Refs"), Space]
	public GrabableWithCallbacks grabbable;
	[SerializeField] Transform movingPartCenter;
	[SerializeField] Transform movingPart;
	[SerializeField] Transform headRenderer;
	[SerializeField] Transform headGrab;

	float value = 0;

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
			value = Mathf.Clamp(movingPartCenter.InverseTransformVector(movingPartCenter.position - grabbable.grabbedBy.transform.position).z / maxMove, 0, 1);
			movingPart.localPosition = Vector3.zero.SetZ(value * -maxMove);
		}
	}

	void OnGrabStart() {

	}

	void OnGrabEnd() {
		headGrab.transform.localRotation = headRenderer.transform.localRotation;
		headGrab.transform.position = headRenderer.transform.position;
	}
}
