﻿using UnityEngine;

public class Rotater : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        // to rotate the PickUp objects
        transform.Rotate(new Vector3 (15, 30, 45) * Time.deltaTime);
	}
}