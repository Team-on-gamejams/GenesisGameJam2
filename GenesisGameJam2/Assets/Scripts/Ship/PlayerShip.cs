using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerShip : MonoBehaviour {
	[Header("Moving")]
	[Space]
	[SerializeField] float moveSpeed = 4.0f;
	[SerializeField] float rotateSpeed = 0.5f;

	[Header("Balance")]
	[Space]
	[SerializeField] float shieldTime = 10;
	[SerializeField] float shieldKD = 30;
	[SerializeField] float doubleAttackTime = 10;
	[SerializeField] float doubleAttackKD = 30;
	bool isShieldAvaliable = true;
	bool isDoubleAttackAvaliable = true;

	[Header("UI"), Space]
	[SerializeField] TextMeshProUGUI speedTextField;
	[SerializeField] TextMeshProUGUI timeTextField;
	[SerializeField] TextMeshProUGUI tempTextField;
	[SerializeField] TextMeshProUGUI sectorTextField;
	[SerializeField] TextMeshProUGUI debugTextField;

	[Header("Refs"), Space]
	[SerializeField] Rigidbody rb;
	[SerializeField] JoystickLinearMove moveJoy;
	[SerializeField] JoystickGrabController rotateJoy;
	[SerializeField] Health health;
	[SerializeField] Weapon weaponLight;
	[SerializeField] Weapon weaponHeavy;

	bool isSwitchingWeapon = true;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rb)
			rb = GetComponent<Rigidbody>();
		if (!health)
			health = GetComponent<Health>();
	}
#endif

	private void Awake() {
		health.OnDie += OnDie;
	}

	private void Start() {
		tempTextField.text = "42F";
		sectorTextField.text = "Sector:\nMATRIX 177013";

		weaponLight.Equip();
	}

	private void OnDestroy() {

	}

	private void Update() {
		speedTextField.text = "Speed: " + rb.velocity.magnitude.ToString("0") + "m/s";
		timeTextField.text = DateTime.Now.ToShortTimeString();

		
		if (OVRInput.GetDown(OVRInput.Button.One)) {
			SwitchWeapon();
		}
		if (OVRInput.GetDown(OVRInput.Button.Two)) {
			SwitchWeapon();
		}
		if (OVRInput.GetDown(OVRInput.Button.Three)) {
			UseShieldAbility();
		}
		if (OVRInput.GetDown(OVRInput.Button.Four)) {
			UseDoubleWeaponAbility();
		}
	}

	private void FixedUpdate() {
		Vector3 tmp = Vector3.zero;
		Vector3 targetVelocity = transform.TransformDirection(new Vector3(0, 0, moveJoy.Value * moveSpeed));
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

	void SwitchWeapon() {
		if (weaponLight.IsEquiped && weaponHeavy.IsEquiped)
			return;

		if (isSwitchingWeapon) {
			return;
		}

		Debug.Log("SwitchWeapon");

		if (weaponLight.IsEquiped) {
			weaponLight.Deequip();
			weaponHeavy.Equip();
		}

		if (weaponHeavy.IsEquiped) {
			weaponHeavy.Deequip();
			weaponLight.Equip();
		}
	}

	public void UseShieldAbility() {
		if (!isShieldAvaliable)
			return;
		isShieldAvaliable = false;

		Debug.Log("UseShieldAbility");

		health.IsShielded = true;

		LeanTween.delayedCall(gameObject, shieldTime, () => {
			health.IsShielded = false;
			LeanTween.delayedCall(gameObject, shieldKD, () => {
				isShieldAvaliable = true;
			});
		});
	}

	public void UseDoubleWeaponAbility() {
		if (!isDoubleAttackAvaliable)
			return;
		isDoubleAttackAvaliable = false;

		Debug.Log("UseDoubleWeaponAbility");

		weaponLight.Equip();
		weaponHeavy.Equip();

		LeanTween.delayedCall(gameObject, doubleAttackTime, () => {
			weaponHeavy.Deequip();
			LeanTween.delayedCall(gameObject, doubleAttackKD, () => {
				isDoubleAttackAvaliable = true;
			});
		});
	}

	void OnDie() {
		enabled = false;

		tempTextField.text = "YOU DEAD";
		sectorTextField.text = "YOU DEAD";
		speedTextField.text = "YOU DEAD";
		timeTextField.text = "YOU DEAD";

		//TODO: red flash lights
		//TODO: flash texts
		//TODO: slowdown time

		Debug.Log("Game END");

		LeanTween.delayedCall(5.0f, () => {
			Scene scene = SceneManager.GetActiveScene(); 
			SceneManager.LoadScene(scene.name);
		});
	}
}
