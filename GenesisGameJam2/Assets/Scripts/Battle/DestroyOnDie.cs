using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDie : MonoBehaviour
{
	[Header("Refs"), Space]
	[SerializeField] Health health;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!health)
			health = GetComponent<Health>();
	}
#endif

	private void Awake() {
		health.OnDie += OnDie;
	}

	private void OnDestroy() {
		health.OnDie -= OnDie;
	}

	void OnDie() {
		Destroy(gameObject);
	}
}
