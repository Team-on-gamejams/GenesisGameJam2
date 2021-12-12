using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimToEnemy : MonoBehaviour
{
	[Header("Refs"), Space]
	[SerializeField] Transform aim;
	[SerializeField] Transform targetToAim;

	List<Transform> targets;

	private void Update() {
		while(targets.Count != 0 && targets[0] == null) {
			targets.RemoveAt(0);
		}

		if (targets.Count == 0)
			return;

		Transform nearestTarget = targets[0];
		float nearestDists = (nearestTarget.transform.position - targetToAim.position).magnitude;
		float dist;
		for (int i = 1; i < targets.Count; ++i) {
			if (targets[i] == null)
				continue;
			dist = (targets[i].transform.position - targetToAim.position).magnitude;
			if (dist < nearestDists) {
				nearestTarget = targets[i];
				nearestDists = dist;
			}
		}

		var targetRotation = Quaternion.LookRotation(nearestTarget.position - aim.position);
		var str = 10 * Time.deltaTime;
		aim.rotation = Quaternion.RotateTowards(aim.rotation, targetRotation, 60 * Time.deltaTime);
	}

	public void AddTarget(Collider other) {
		if(other.transform.GetComponent<Health>())
			targets.Add(other.transform);
	}

	public void RemoveTarget(Collider other) {
		targets.Remove(other.transform);
	}
}
