using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerShip : MonoBehaviour {
	[Header("Balance"), Space]
	[SerializeField] float speedAcc = 500;

	[Header("Moving")]
	[Space]
	[SerializeField] float moveSpeed = 4.0f;
	[SerializeField] float rotateSpeed = 0.5f;

	[Header("UI"), Space]
	[SerializeField] TextMeshProUGUI speedTextField;
	[SerializeField] TextMeshProUGUI timeTextField;
	[SerializeField] TextMeshProUGUI tempTextField;
	[SerializeField] TextMeshProUGUI debugTextField;

	[Header("Refs"), Space]
	[SerializeField] Rigidbody rb;
	[SerializeField] JoystickGrabController moveJoy;
	[SerializeField] JoystickGrabController rotateJoy;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rb)
			rb = GetComponent<Rigidbody>();
	}
#endif

	private void Update() {
		speedTextField.text = "Speed: " + rb.velocity.magnitude.ToString("0") + "m/s";
		timeTextField.text = DateTime.Now.ToShortTimeString();
		tempTextField.text = "42°C";
	}

	private void FixedUpdate() {
		Vector3 tmp = Vector3.zero;
		Vector3 targetVelocity = transform.TransformDirection(new Vector3(moveJoy.Value.y * moveSpeed, 0, moveJoy.Value.x * moveSpeed));
		if (targetVelocity != Vector3.zero)
			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref tmp, 0.1f);

		rb.angularVelocity = transform.TransformDirection(new Vector3(rotateJoy.Value.x * rotateSpeed, rotateJoy.Value.y * rotateSpeed, 0));

		if(moveJoy.grabbable.grabbedBy)
			debugTextField.text = $"{moveJoy.Value}\n{moveJoy.transform.localEulerAngles}\n{moveJoy.grabbable.grabbedBy.transform.parent.parent.localEulerAngles}";
		else if (rotateJoy.grabbable.grabbedBy)
			debugTextField.text = $"{rotateJoy.Value}\n{rotateJoy.transform.localEulerAngles}\n{rotateJoy.grabbable.grabbedBy.transform.parent.parent.localEulerAngles}";
		else
			debugTextField.text = $"{moveJoy.Value}\n{rotateJoy.Value}\n{rotateJoy.transform.localEulerAngles}";
	}
}
