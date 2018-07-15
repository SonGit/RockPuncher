using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerUI))]

public class Player : MonoBehaviour {

	#region public vars
	public float _chargeRate;
	public float _currentCharge;
	public float _punchRange;
	public float _punchAngle;
	public float _punchForce;
	#endregion

	#region caches for profiency
	PlayerController _playerController;
	#endregion

	// Use this for initialization
	void Start () {
		_playerController = this.GetComponent<PlayerController> ();
		_currentCharge = Constants.startCharge;
		_punchRange = Constants.defaultRange;
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

		// Cap the charge
		if (_currentCharge > Constants.maxCharge)
			_currentCharge = Constants.maxCharge;

		// Take the charge into account
		if (_currentCharge < Constants.maxCharge) {
			_punchRange += _currentCharge / Constants.chargeToRangeConversionRate;
			_punchForce += _currentCharge / Constants.chargeToForceConversionRate;
		}

		// Cap the punch range
		if (_punchRange > Constants.maxCharge)
			_punchRange = Constants.maxCharge;
		if (_punchForce > Constants.maxForce)
			_punchForce = Constants.maxForce;
	}

	/// <summary>
	/// Punch!
	/// </summary>
	public void Discharge()
	{
		Invoke ("RaycastForHit",.25f);
	}

	/// <summary>
	/// Raycast a line with a length of _punchRange
	/// If hit anything, apply approriate hit logic
	/// </summary>
	void RaycastForHit()
	{
		RaycastHit hit;

		Vector3 origin =  _playerController.mesh.transform.position + new Vector3(0,1,0); // the offset amke sure raycast is cast from center of body
		Vector3 direction =  _playerController.mesh.transform.forward;

		if (Physics.Raycast(origin, direction, out hit,_punchRange)) {
			// push rock away
			if (hit.transform.tag == "Rock") {
				hit.rigidbody.AddForceAtPosition (_punchForce * direction ,hit.point);
				print ("PUSHED " + _punchRange);
			}
		}

		// Reset
		_currentCharge = Constants.startCharge;
		_punchRange = Constants.defaultRange;
		_punchForce = Constants.defaultForce;
	}
		
}
