using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	[SerializeField] Weapon weapon;

	private void Start() {
		weapon.StartShooting();
	}
}
