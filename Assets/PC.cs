using UnityEngine;

public class PC : MonoBehaviour {
	struct TimestampedDirection {
		public float startSpeed, maxSpeed, curSpeed;
		public float accelerationTime, decelerationTime;
		public AnimationCurve accelerationCurve, decelerationCurve;
		float timeSinceHeld, timeSinceReleased;
		bool isHeld;

		public void UpdatePressed() {
			if (!isHeld) {
				timeSinceHeld = 0;
				startSpeed = curSpeed;
			}

			isHeld = true;
			timeSinceHeld += Time.deltaTime;
			if (accelerationTime > 0)
				curSpeed = Mathf.Lerp(startSpeed, maxSpeed, accelerationCurve.Evaluate(timeSinceHeld / accelerationTime));
			else curSpeed = maxSpeed;
		}

		public void UpdateUnpressed() {
			if (isHeld) {
				timeSinceReleased = 0;
				startSpeed = curSpeed;
			}

			isHeld = false;
			timeSinceReleased += Time.deltaTime;
			if (decelerationTime > 0)
				curSpeed = Mathf.Lerp(0, startSpeed, decelerationCurve.Evaluate(timeSinceReleased / decelerationTime));
			else curSpeed = 0;
		}

		public void Update(float inputDirection, float inputTolerance) {
			if (inputDirection > inputTolerance) { 
				UpdatePressed();
			}
			else UpdateUnpressed();
		}
	}

	Rigidbody _rb;
	PlayerInputActions playerInput;
	Vector3 finalDirection;
	Vector2 inputVector;

	[Header("Stats")]
	public float moveSpeed = 6;
	
	[Header("InputData"), Range(.2f, .6f)]
	[Tooltip("The Input of a Controller is to sensitive for a 4-Button-System, therefore you use a Input-Tolerance")]
	public float inputTolerance = .3f;

	[Header("Inertia Stats")]
	public float maxDirectionalValue = 1;
	public float accelerationTime = 1;
	public float decelerationTime = 1;
	public AnimationCurve accelerationCurve = AnimationCurve.Linear(0, 0, 1, 1);
	public AnimationCurve decelerationCurve = AnimationCurve.Linear(0, 1, 1, 0);
	TimestampedDirection leftSpeed, rightSpeed, upSpeed, downSpeed;

	private void Awake() {
        playerInput = new PlayerInputActions();
        _rb = GetComponent<Rigidbody>();

        leftSpeed = new TimestampedDirection();
        rightSpeed = new TimestampedDirection();
        upSpeed = new TimestampedDirection();
        downSpeed = new TimestampedDirection();

        leftSpeed.maxSpeed = rightSpeed.maxSpeed = upSpeed.maxSpeed = downSpeed.maxSpeed = maxDirectionalValue;
        leftSpeed.accelerationTime = rightSpeed.accelerationTime = upSpeed.accelerationTime = downSpeed.accelerationTime = accelerationTime;
        leftSpeed.decelerationTime = rightSpeed.decelerationTime = upSpeed.decelerationTime = downSpeed.decelerationTime = decelerationTime;
        leftSpeed.accelerationCurve = rightSpeed.accelerationCurve = upSpeed.accelerationCurve = downSpeed.accelerationCurve = accelerationCurve;
        leftSpeed.decelerationCurve = rightSpeed.decelerationCurve = upSpeed.decelerationCurve = downSpeed.decelerationCurve = decelerationCurve;


	}

    private void Start() {
		
    }

    // Controls
    public void Update() {
		inputVector = playerInput.Player.Move.ReadValue<Vector2>();
		//Calculates the speed for each direction
		rightSpeed.Update(inputVector.x, inputTolerance);
		leftSpeed.Update(-inputVector.x, inputTolerance);
		upSpeed.Update(inputVector.y, inputTolerance);
		downSpeed.Update(-inputVector.y, inputTolerance);

		finalDirection = new Vector3(rightSpeed.curSpeed - leftSpeed.curSpeed, upSpeed.curSpeed - downSpeed.curSpeed);

		int rotationSpeed = 360;
		if (_rb.velocity.normalized != Vector3.zero) {
			Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, _rb.velocity.normalized);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
		}
	}

    //Move Player
    private void FixedUpdate() {
		//_rb.MovePosition(_rb.position + (finalDirection * moveSpeed * Time.deltaTime));
		_rb.velocity = Vector3.ClampMagnitude(finalDirection * moveSpeed, 40);
	}

	private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }
}
