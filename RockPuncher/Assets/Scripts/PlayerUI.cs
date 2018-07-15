using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

	#region public vars
	public SpriteRenderer _chargeCursor;
	#endregion

	#region caches for profiency
	Player _player;
	float _chargeCursorScale = 1;
	[SerializeField]
	float _offset;
	#endregion

	// Use this for initialization
	void Start () {
		_player = this.GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		TrackCharge ();
	}

	void TrackCharge()
	{
		if (_chargeCursor == null) {
			Debug.Log ("_chargeCursor is null!");
			return;
		}

		if (_player._punchRange <= 0) {
			/// Hide if there is no charge
			_chargeCursor.transform.localScale = new Vector3(.2f,.2f,.2f);
		} 
		else 
		{
			//_chargeCursor.enabled = true;
			_chargeCursorScale = _player._punchRange; // Scale the cursor in accordance to Unity distance unit
			_chargeCursor.transform.localScale = new Vector3(_chargeCursorScale,_chargeCursorScale,_chargeCursorScale);
		}


	}
}
