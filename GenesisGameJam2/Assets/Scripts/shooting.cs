using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
	public GameObject small_bullet_prefab;
	GameObject large_bullet;


	void shoot_once() {
		if (Input.GetKeyUp(KeyCode.Space)){

			Instantiate(small_bullet_prefab, transform.position, transform.parent.rotation);
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
		shoot_once();

	}
}
