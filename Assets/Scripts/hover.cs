using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hover : MonoBehaviour {
    float speed = 3;
    float amplitude = 0.005f;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.up * amplitude * Mathf.Sin(speed * Time.time), Space.World);


    }
}
