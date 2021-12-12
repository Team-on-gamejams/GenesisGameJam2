using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCone : MonoBehaviour
{
	public AimToEnemy aim;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!aim)
			aim = GetComponentInChildren<AimToEnemy>();
	}
#endif

	private void OnTriggerEnter(Collider other) {
		aim.AddTarget(other);
	}

	public void OnTriggerExit(Collider other) {
		aim.RemoveTarget(other);
	}
}
