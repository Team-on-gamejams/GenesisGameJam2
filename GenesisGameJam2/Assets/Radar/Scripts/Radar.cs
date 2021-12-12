/* 
    ------------------- Code Monkey -------------------
    
    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour {

    private float rotationSpeed;

    private void Start() {
        rotationSpeed = 180f;
    }

    private void Update() {
        transform.eulerAngles -= new Vector3(0, 0, rotationSpeed * Time.deltaTime);
    }

}
