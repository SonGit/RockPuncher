using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDistance : MonoBehaviour {

	public Transform dest;

	// Use this for initialization
	void Start () {
		
	}
	public float distance;
	// Update is called once per frame
	void Update () {
		distance = Vector3.Distance (transform.position,dest.position);
	}
}
