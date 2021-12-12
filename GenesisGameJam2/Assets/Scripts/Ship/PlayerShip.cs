using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
	[SerializeField] TextMeshProUGUI[] dieTexts;


	[Header("Audio"), Space]
	[SerializeField] AudioClip mainTheme;

	[Header("UI"), Space]
	[SerializeField] Slider shieldKDSlider;
	[SerializeField] Slider doubleWeaponKDSlider;

	[Header("Refs"), Space]
	[SerializeField] Rigidbody rb;
	[SerializeField] JoystickLinearMove moveJoy;
	[SerializeField] JoystickGrabController rotateJoy;
	[SerializeField] Health health;
	[SerializeField] Weapon weaponLight;
	[SerializeField] Weapon weaponHeavy;

	bool isSwitchingWeapon = false;
	bool isHoldShooting;

	bool isHoldLeftTrigger;

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

		tempTextField.text = "42F";
		sectorTextField.text = "Sector:\n MATRIX-177013";

		shieldKDSlider.minValue = 0;
		shieldKDSlider.maxValue = shieldKD;

		doubleWeaponKDSlider.minValue = 0;
		doubleWeaponKDSlider.maxValue = doubleAttackKD;

		shieldKDSlider.value = 0;
		doubleWeaponKDSlider.value = 0;
	}

	private void Start() {
		weaponLight.Equip();

		AudioManager.Instance.PlayMusic(mainTheme, 0.33f);
	}

	private void OnDestroy() {
		health.OnDie -= OnDie;
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

		if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) >= 0.7f && !isHoldShooting) {
			Debug.Log("Down");
			PressShoot();
		}
		if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) < 0.7f && isHoldShooting) {
			Debug.Log("Up");
			ReleaseShoot();
		}

		if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) >= 0.7f && !isHoldLeftTrigger) {
			isHoldLeftTrigger = true;
		}
		if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) < 0.7f && isHoldLeftTrigger) {
			isHoldLeftTrigger = false;
		}
	}

	private void FixedUpdate() {
		Vector3 tmp = Vector3.zero;
		Vector3 targetVelocity = transform.TransformDirection(new Vector3(0, 0, moveJoy.Value * moveSpeed));
		if (isHoldLeftTrigger)
			targetVelocity = -targetVelocity;
		if (targetVelocity != Vector3.zero)
			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref tmp, 0.1f);

		rb.angularVelocity = transform.TransformDirection(new Vector3(rotateJoy.Value.x * rotateSpeed, rotateJoy.Value.y * rotateSpeed, 0));
	}

	public void SwitchWeapon() {
		if (weaponLight.IsEquiped && weaponHeavy.IsEquiped)
			return;

		if (isSwitchingWeapon) {
			return;
		}

		if (weaponLight.IsEquiped) {
			weaponLight.Deequip();
			weaponHeavy.Equip();
			if (isHoldShooting) 
				weaponHeavy.StartShooting();
		}
		else if (weaponHeavy.IsEquiped) {
			weaponHeavy.Deequip();
			weaponLight.Equip();
			if (isHoldShooting)
				weaponLight.StartShooting();
		}
	}

	public void UseShieldAbility() {
		if (!isShieldAvaliable)
			return;
		isShieldAvaliable = false;

		Debug.Log("UseShieldAbility");

		health.IsShielded = true;
		shieldKDSlider.value = shieldKD;

		LeanTween.delayedCall(gameObject, shieldTime, () => {
			health.IsShielded = false;

			LeanTween.value(gameObject, shieldKD, 0, shieldKD)
			.setOnUpdate((float t) => {
				shieldKDSlider.value = t;
			})
			.setOnComplete(() => {
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
		doubleWeaponKDSlider.value = doubleAttackKD;

		if (isHoldShooting) {
			weaponLight.StartShooting();
			weaponHeavy.StartShooting();
		}

		LeanTween.delayedCall(gameObject, doubleAttackTime, () => {
			weaponHeavy.Deequip();

			LeanTween.value(gameObject, doubleAttackKD, 0, doubleAttackKD)
			.setOnUpdate((float t) => {
				doubleWeaponKDSlider.value = t;
			})
			.setOnComplete(() => {
				isDoubleAttackAvaliable = true;
			});
		});
	}

	void PressShoot() {
		isHoldShooting = true;

		Debug.Log($"Start shooting {weaponLight.IsEquiped} {weaponHeavy.IsEquiped}");

		if (weaponLight.IsEquiped)
			weaponLight.StartShooting();
		if (weaponHeavy.IsEquiped)
			weaponHeavy.StartShooting();
	}

	void ReleaseShoot() {
		isHoldShooting = false;

		Debug.Log($"End shooting {weaponLight.IsEquiped} {weaponHeavy.IsEquiped}");

		if (weaponLight.IsEquiped)
			weaponLight.StopShooting();
		if (weaponHeavy.IsEquiped)
			weaponHeavy.StopShooting();
	}

	void OnDie() {
		enabled = false;

		tempTextField.text = "YOU DEAD";
		sectorTextField.text = "YOU DEAD";
		speedTextField.text = "YOU DEAD";
		timeTextField.text = "YOU DEAD";

		foreach (var text in dieTexts) {
			if(text)
				text.text = "YOU DEAD";
		}

		//TODO: red flash lights
		//TODO: flash texts
		//TODO: slowdown time

		Debug.Log("Game END");

		LeanTween.delayedCall(10.0f, () => {
			Scene scene = SceneManager.GetActiveScene(); 
			SceneManager.LoadScene(scene.name);
		});
	}
}
