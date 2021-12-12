using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {
	[NonSerialized] public bool IsEquiped;
	[NonSerialized] public bool IsShooting;

	[Header("Balance"), Space]
	[SerializeField] bool IsPlayer = false;
	[SerializeField] GameObject beamPrefab;
	[SerializeField] int damage = 50;
	[SerializeField] float attackDist = 10;

	[Header("KD"), Space]
	[SerializeField] bool isHaveCooldown = true;
	[SerializeField] float cooldownTime = 3;

	[Header("Overheat"), Space]
	[SerializeField] bool isHaveOverheat = false;
	[SerializeField] float overheatMaxTime = 10;

	[Header("Animate"), Space]
	[SerializeField] bool isAnimateRotation = false;
	[SerializeField] bool isAnimateBack = false;
	[SerializeField] Transform gameObjectToAnimate;


	[Header("UI"), Space]
	[SerializeField] Slider slider;

	[Header("Refs"), Space]
	[SerializeField] Transform shootPos;

	float cooldownTimer;
	float overheatTimer;

	private void Awake() {
		if(slider)
			slider.minValue = 0;
		if(slider)
			slider.maxValue = 1;
	}

	private void Update() {
		if (!IsShooting) {
			if (isHaveCooldown) {
				cooldownTimer -= Time.deltaTime;
				cooldownTimer = Mathf.Clamp(cooldownTimer, 0, cooldownTimer);
			}
			if (isHaveOverheat) {
				overheatTimer -= Time.deltaTime;
				overheatTimer = Mathf.Clamp(overheatTimer, 0, overheatMaxTime);
			}
			UpdateSlider();

			return;
		}

		bool isCanShoot = false;
		if (isHaveOverheat) {
			overheatTimer += Time.deltaTime;
			overheatTimer = Mathf.Clamp(overheatTimer, 0, overheatMaxTime);
			if (overheatTimer < overheatMaxTime) {
				isCanShoot = true;
			}
		}

		if (isHaveCooldown) {
			cooldownTimer -= Time.deltaTime;
			if (cooldownTimer <= 0) {
				isCanShoot = true;
			}
			else {
				isCanShoot = false;
			}
		}

		if (isHaveOverheat && overheatTimer >= overheatMaxTime) {
			isCanShoot = false;
		}

		UpdateSlider();

		if (isCanShoot) {
			Shoot();
		}
	}

	public void Equip() {
		if (IsEquiped)
			return;
		IsEquiped = true;
	}

	public void Deequip() {
		if (!IsEquiped)
			return;
		IsEquiped = false;

		if (IsShooting)
			StopShooting();
	}

	public void StartShooting() {
		if (IsShooting)
			return;
		IsShooting = true;
	}

	public void StopShooting() {
		if (!IsShooting)
			return;
		IsShooting = false;
	}

	void Shoot() {
		cooldownTimer = cooldownTime;

		GameObject beam = Instantiate(beamPrefab, shootPos.transform.position, shootPos.transform.rotation);
		Destroy(beam, 0.05f);

		if (isAnimateBack) {
			LeanTween.value(gameObject, gameObjectToAnimate.localPosition.z, gameObjectToAnimate.localPosition.z + 0.6f, cooldownTime * 0.45f)
				.setEase(LeanTweenType.easeOutExpo)
				.setOnUpdate((float angle) => {
					gameObjectToAnimate.localPosition = gameObjectToAnimate.localPosition.SetZ(angle);
				})
				.setOnComplete(() => {
					LeanTween.value(gameObject, gameObjectToAnimate.localPosition.z, gameObjectToAnimate.localPosition.z - 0.6f, cooldownTime * 0.45f)
					.setEase(LeanTweenType.easeOutQuad)
					.setOnUpdate((float angle) => {
						gameObjectToAnimate.localPosition = gameObjectToAnimate.localPosition.SetZ(angle);
					});
				});
		}
		else if (isAnimateRotation) {
			LeanTween.value(gameObject, gameObjectToAnimate.eulerAngles.z, gameObjectToAnimate.eulerAngles.z + 120, cooldownTime * 0.99f)
				.setOnUpdate((float angle) => {
					gameObjectToAnimate.eulerAngles = gameObjectToAnimate.eulerAngles.SetZ(angle);
				});
		}

		Vector3 fwd = shootPos.transform.TransformDirection(Vector3.forward);
		if (Physics.Raycast(shootPos.transform.position, fwd, out RaycastHit objectHit, attackDist, IsPlayer ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("PlayerShip"), QueryTriggerInteraction.Ignore)) {
			Health health = objectHit.transform.GetComponent<Health>();
			if (health) {
				health.GetDamage(damage);
			}
		}
	}

	void UpdateSlider() {
		if (!slider)
			return;

		if (isHaveOverheat) {
			slider.value = overheatTimer / overheatMaxTime;
		}
		else if (isHaveCooldown) {
			slider.value = cooldownTimer / cooldownTime;
		}
	}
}
