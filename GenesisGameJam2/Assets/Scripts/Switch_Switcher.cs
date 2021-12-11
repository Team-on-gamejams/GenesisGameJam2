using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch_Switcher : MonoBehaviour
{
	private bool _isPressed;
	private Vector3 _startPos;
	private ConstantForce _force;

	public UnityEvent onPressed, onReleased;


	// Start is called before the first frame update
	void Start() {
		_startPos = transform.localPosition;
		_force = GetComponent<ConstantForce>();
	}

	// Update is called once per frame
	void Update() {
		if (!_isPressed && Vector3.Distance(_startPos, transform.localPosition) <= 0)
			Pressed();

		if (_isPressed && Vector3.Distance(_startPos, transform.localPosition) > 0)
			Released();

	}


	private void Pressed() {
		_isPressed = true;
		_force.relativeForce.SetX(-0.5f);
		onPressed.Invoke();
		Debug.Log("Pressed");
	}

	private void Released() {
		_isPressed = false;
		_force.relativeForce.SetX(0.5f);
		onReleased.Invoke();
		Debug.Log("Released");
	}
}
