using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {
	Rigidbody _rb;
	float velocity;

	float _timeCount;

	// Use this for initialization
	void Start () {
		_rb = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		_timeCount += Time.deltaTime;

		if (_timeCount < .25f) {
			velocity = _rb.velocity.magnitude;
			_timeCount = 0;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		//print ("collision " + collision.transform.name + "  " + velocity);
	}

}
