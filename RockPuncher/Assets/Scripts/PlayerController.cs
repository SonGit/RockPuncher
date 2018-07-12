using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class PlayerController : MonoBehaviour {
	
	#region public vars
	[Tooltip("Only if PhoTon View is mine does this true")]
	public bool isControllable;
	[Tooltip("Chacter move speed")]
	public float walkSpeed = 10;
	[Tooltip(" after trotAfterSeconds of walking we trot with trotSpeed")]
	public float trotSpeed = 4.0f;
	[Tooltip("Chacter run speed")]
	public float runSpeed = 6.0f;
	[Tooltip("Trot after seconds")]
	public float trotAfterSeconds = 0;
	[Tooltip("Rotation speed when turns head")]
	public float rotateSpeed = 10;
	[Tooltip("Smoothness")]
	public float speedSmoothing = 10.0f;
	[Tooltip("Should point to Mesh object, child of this transform")]
	public Transform mesh;
	#endregion

	#region private vars
	// The current move direction in x-z
	private float moveSpeed;
	// The current vertical speed
	private float verticalSpeed= 0;
	private bool movingBack;
	private bool wasMoving;
	private bool isMoving;
	private Vector3 moveDirection;
	// The current player velocity
	private Vector3 velocity;
	// When did the user start walking (Used for going into trot after a while)
	protected float walkTimeStart = 0.0f;
	private bool grounded;
	// is player using attack animation?
	[SerializeField]
	private bool isAttacking;

	// curretn state
	[SerializeField]
	private CharacterState _state;
	private CharacterState _prevState;
	#endregion

	#region caches for profiency
	private Transform _cameraTransform;
	private float curSmooth;
	private Vector3 lastPos;
	private Animator animator;

	private bool isMouseHeldDown;
	private Vector3 point;
	private int layerMask;
	private float stopShootAnim;
	private PhotonView photonView;
	[SerializeField]
	private bool _readyToShoot;
	#endregion

	PhotonView[] gos ;
	void Awake()
	{
		_cameraTransform = Camera.main.transform;
		animator = this.GetComponent<Animator> ();

		grounded = true;
		layerMask = 1 << LayerMask.NameToLayer ("Floor"); // only check for collisions with Floor layer
		photonView = this.GetComponent<PhotonView>();



	}

	void Update()
	{
		if (!isControllable)
			return;
		
		UpdateSmoothedMovementDirection ();
		ApplyGravityToCharacterController ();
		UpdateAnimation ();
		Turning ();

		// Calculate actual motion
		Vector3 movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0) ;
		movement *= Time.deltaTime;

		// Move the controller
		CharacterController controller = GetComponent<CharacterController>();
		controller.Move(movement);

		velocity = (transform.position - lastPos)*25;

		lastPos = transform.position;

		if (Input.GetMouseButton (0)) {
			isMouseHeldDown = true;
		}
		if (Input.GetMouseButtonUp (0)) {
			//print ("isMouseHeldDown");
			isMouseHeldDown = false;
		}
	}
		
	#region attack 
	/// <summary>
	/// Check if player is using melee or ranged weapon and play appriate animation
	/// </summary>
	void Attack()
	{
		isMouseHeldDown = true;
	}

	#endregion

	#region movement
	/// <summary>
	/// Calculate movement direction based on camera and input
	/// </summary>
	void UpdateSmoothedMovementDirection()
	{
		// Forward vector relative to the camera along the x-z plane	
		Vector3 forward = _cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;


		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");

		// Are we moving backwards or looking backwards
		if (v < -0.2f)
			movingBack = true;
		else
			movingBack = false;

		bool wasMoving = isMoving;
		isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

		// Target direction relative to the camera
		Vector3 targetDirection = h * right + v * forward;

		// Grounded controls
		if (grounded)
		{

			// We store speed and direction seperately,
			// so that when the character stands still we still have a valid forward direction
			// moveDirection is always normalized, and we only update it if there is user input.
			if (targetDirection != Vector3.zero)
			{
				moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
				moveDirection = moveDirection.normalized;
			}

			// Smooth the speed based on the current target direction
			float curSmooth = speedSmoothing * Time.deltaTime;

			// Choose target speed
			//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
			float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

			// Pick speed modifier
			if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
			{
				targetSpeed *= runSpeed;
			}
			else if (Time.time - trotAfterSeconds > walkTimeStart)
			{
				targetSpeed *= trotSpeed;
			}
			else
			{
				targetSpeed *= walkSpeed;
			}

			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

			// Reset walk time start when we slow down
			if (moveSpeed < walkSpeed * 0.3f)
				walkTimeStart = Time.time;
		}
		if(moveDirection != Vector3.zero)
		transform.rotation = Quaternion.LookRotation(moveDirection);
	}


	void Turning ()
	{
		// Create a ray from the mouse cursor on screen in the direction of the camera.
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit floorHit;

		// Perform the raycast and if it hits something on the floor layer...
		if(Physics.Raycast (camRay, out floorHit, 10000,layerMask))
		{
			point = new Vector3 (floorHit.point.x,0,floorHit.point.z);

			if(mesh != null)
			mesh.LookAt (point);
		}
	}

	#endregion

	#region gravity
	void ApplyGravityToCharacterController()
	{
		CharacterController controller = GetComponent<CharacterController>();
		controller.Move( transform.up * Time.deltaTime * -9.81f );
	}
	#endregion

	#region animation
	void UpdateAnimation()
	{
		animator.SetFloat ("ForwardSpeed",velocity.sqrMagnitude);
		animator.SetBool ("Charging",isMouseHeldDown);
	}

	void EndAttackAnimation()
	{
		_state = CharacterState.Idle;
	}
		
	#endregion

}
