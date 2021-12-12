using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public bool IsEquiped;
	public bool IsShooting;

    public void Equip() {
		IsEquiped = true;
	}

	public void Deequip() {
		IsEquiped = false;
	}

	public void StartShooting() {
		IsShooting = true;
	}

	public void StopShooting() {
		IsShooting = false;
	}
}
