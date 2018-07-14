using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrower : MonoBehaviour {
	public GameObject prefab;
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds (1.5f);
		while (true) {
			GameObject go = (GameObject)Instantiate (prefab,transform.position,prefab.transform.rotation);

			Rigidbody rb = go.GetComponent<Rigidbody> ();

			if (rb != null) {
				rb.AddForce (transform.forward * 1000);
			}
			yield return new WaitForSeconds (4);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
