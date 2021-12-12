using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGunShoot : MonoBehaviour
{
	[SerializeField] Weapon weapon;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!weapon)
			weapon = GetComponentInChildren<Weapon>();
	}
#endif

	private void Start() {
		weapon.Equip();
		weapon.StartShooting();
	}
}
