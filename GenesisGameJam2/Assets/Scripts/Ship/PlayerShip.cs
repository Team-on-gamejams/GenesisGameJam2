using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour {
	[Header("Balance"), Space]
	[SerializeField] float speed = 5;

	[Header("Refs"), Space]
	[SerializeField] Rigidbody rigidbody;
	[SerializeField] JoystickGrabController control;
	[SerializeField] JoystickGrabController thrust;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rigidbody)
			rigidbody = GetComponent<Rigidbody>();
	}
#endif

	private void FixedUpdate() {
		if (control.IsGrabbed) {
			rigidbody.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Acceleration);
		}
	}
}
