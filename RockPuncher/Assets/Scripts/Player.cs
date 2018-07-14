using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]

public class Player : MonoBehaviour {

	#region public vars
	public float _chargeRate;
	public float _currentCharge;
	public float _punchRange;
	public float _punchAngle;
	#endregion

	#region caches for profiency
	PlayerController _playerController;
	#endregion

	// Use this for initialization
	void Start () {
		_playerController = this.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	/// <summary>
	/// Charge your punch
	/// </summary>
	public void Charge()
	{
		_currentCharge += _chargeRate * Time.deltaTime;
	}

	/// <summary>
	/// Punch!
	/// </summary>
	public void Discharge()
	{
		RaycastForHit ();
		_currentCharge = 0;
	}

	/// <summary>
	/// Raycast a line with a length of _punchRange
	/// If hit anything, apply approriate hit logic
	/// </summary>
	void RaycastForHit()
	{
		RaycastHit hit;

		Vector3 origin =  _playerController.mesh.transform.position + new Vector3(0,1,0.4f);
		Vector3 direction =  _playerController.mesh.transform.forward;

		if (Physics.SphereCast(origin, _punchAngle, direction, out hit,_punchRange)) {
			print (hit.transform.name);
		}

	}
}
