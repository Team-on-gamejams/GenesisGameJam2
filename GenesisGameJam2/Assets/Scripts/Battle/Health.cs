using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class Health : MonoBehaviour {
	public Action OnDie;

	public bool IsShielded = false;

	[Header("Balance"), Space]
	[SerializeField] int maxHealth = 1000;
	int currHealth;

	[Header("Player Tweaks"), Space]
	[SerializeField] bool isPlayerHealth = false;
	[SerializeField] float threshold = 0.05f;
	int hpToOneshoot;

	[Header("Get damage clip"), Space]
	[SerializeField] AudioClip Getdamagesclip;


	[Header("UI"), Space]
	[SerializeField] Transform barParent;
	[SerializeField] Slider barFirst;
	[SerializeField] Slider barSecond;

	bool isDead = false;

	private void Awake() {
		currHealth = maxHealth;

		barFirst.minValue = barSecond.minValue = 0;
		barFirst.maxValue = barSecond.maxValue = maxHealth;
		barFirst.value = barSecond.value = currHealth;

		if (isPlayerHealth) {
			hpToOneshoot = Mathf.RoundToInt(maxHealth * threshold);
		}
	}

	public void GetDamage(int damage) {
		if (isDead)
			return;

		if (IsShielded)
			return;

		if (isPlayerHealth) {
			bool IsCanBeKilled = currHealth <= hpToOneshoot;
			currHealth = Mathf.Clamp(currHealth - damage, 0, maxHealth);
			if(!IsCanBeKilled && currHealth <= hpToOneshoot) {
				hpToOneshoot = UnityEngine.Random.Range(1, hpToOneshoot);
			}
		}
		else {
			currHealth = Mathf.Clamp(currHealth - damage, 0, maxHealth);
		}

		if (barFirst != null) {
			LeanTween.cancel(barFirst.gameObject, false);
			LeanTween.value(barFirst.gameObject, barFirst.value, currHealth, 0.1f)
			.setOnUpdate((float hp) => {
				barFirst.value = hp;
			});
		}

		if (barSecond != null) {
			LeanTween.cancel(barSecond.gameObject, false);
			LeanTween.value(barSecond.gameObject, barSecond.value, currHealth, 0.4f)
			.setEase(LeanTweenType.easeInQuart)
			.setOnUpdate((float hp) => {
				barSecond.value = hp;
			});
		}

		if (currHealth == 0) {
			Die();
		}
	}

	void Die() {
		isDead = true;
		OnDie?.Invoke();
	}
}
