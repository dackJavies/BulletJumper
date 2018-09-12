using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D RB;
	private Buffer InputBuffer;
	private BufferReadout Readout;

	private bool grounded;

	private const float RUN_VELOCITY = 35.0f;
	private const float RUN_STARTUP = 3.0f;
	private const float RUN_JOYSTICK_THRESHOLD = 0.4f;
	private const float RUN_TERMINAL_VELOCITY = 13.0f;
	private const float JUMP_VELOCITY = 17.0f;
	private const float SINKING_THRESHOLD = 0.15f;
	private bool holdingJump;
	private bool holdingRight;
	private bool holdingLeft;
	private bool holdingUp;
	private bool holdingDown;
//	private int jumps;
	
	private const float GRAVITY = 60.0f;
	private const float TERMINAL_FALLING_VELOCITY = 35.0f;//70.0f;
	private const float WALL_SLIDE_TERMINAL_VELOCITY = 6.0f;
	private const float WALL_JUMP_STALL = 0.5f;
	private const float WALL_JUMP_VELOCITY = 22.0f;

	private const float HORIZ_SURFACE_FRICTION = 90.0f;
	private const float HORIZ_AIR_RESISTANCE = 40.0f;//45.0f;
	private const float WALL_FRICTION = 80.0f;
	private const float FRICTION_SNAP_THRESHOLD = 2f;

	private const float DASH_MAGNITUDE = 25.0f;
	private bool dashing;
	private const float DASH_DURATION = 0.2f;
	private Vector2 lastDash;
	private bool canDash;
	private bool holdingDash;

	public GameObject hitbox;
	private const float ATTACK_JOYSTICK_THRESHOLD = 0.6f;
	private const float HITBOX_SIZE = 0.8f;
	private const float HITBOX_DISTANCE = 1.1f;
	private const float HITBOX_WINDUP = 0.0f;
	private const float HITBOX_DURATION = 0.2f;
	private const float HITBOX_END_LAG = 0.1f;
	private const float ATTACK_KNOCKBACK = 20.0f;
	private const float DASH_ATTACK_KNOCKBACK = 30.0f;
	private const float POST_HIT_DURATION = 0.2f;
	private bool attacking;
	private bool dashAttacking;
	private Vector2 lastAttack;
	private bool postHit;
	private bool holdingRightAttack;
	private bool holdingLeftAttack;
	private bool holdingUpAttack;
	private bool holdingDownAttack;

	private const float MIN_WIDTH = 0.2f;
	private const float MAX_WIDTH = 1.5f;
	private const float MIN_HEIGHT = 0.2f;
	private const float MAX_HEIGHT = 1.5f;
	private const float STRETCH_THRESHOLD = 15.0f;
	private const float STRETCH_RATE = 8.0f;
	private const float SCALE_SNAP_THRESHOLD = 0.05f;
	private float stretchPercentage;

	private bool wallToLeft;
	private bool wallToRight;
	private bool wallSliding;

	private bool leftBlocked;
	private bool rightBlocked;

	private Vector2 temp;

	public Vector2 checkPoint;

	private string[] buffer;
	private int bufferIndex;

	private SpriteRenderer SR;
	/*
	public Sprite dashingSprite;
	public Sprite usedDashSprite;
	private Sprite defaultSprite;
	*/
	public Color dashingColor;
	public Color usedDashColor;
	private Color defaultColor;

	// Use this for initialization
	void Start () {
		RB = GetComponent<Rigidbody2D>();
		InputBuffer = GetComponent<Buffer>();
		Readout = transform.Find("Readout").GetComponent<BufferReadout>();
		SR = GetComponent<SpriteRenderer>();
		defaultColor = SR.color;

		grounded = false;
		leftBlocked = false;
		rightBlocked = false;
		dashing = false;
		attacking = false;
		dashAttacking = false;
		postHit = false;
		canDash = true;
		holdingDash = false;
		buffer = new string[5];
		bufferIndex = 0;
		//jumps = 1;
	}
	
	// Update is called once per frame
	void Update () {
		temp = RB.velocity;

		//AddToBuffer();
		string latest;

		/*if (Input.GetAxis("Horizontal") > RUN_JOYSTICK_THRESHOLD && !holdingRight) {
			holdingLeft = false;
			holdingRight = true;
			InputBuffer.AddCons('>');
		} else if (Input.GetAxis("Horizontal") < -1 * RUN_JOYSTICK_THRESHOLD && !holdingLeft) {
			holdingRight = false;
			holdingLeft = true;
			InputBuffer.AddCons('<');
		} else if (Mathf.Abs(Input.GetAxis("Horizontal")) <= RUN_JOYSTICK_THRESHOLD
					&& (holdingRight || holdingLeft)) {
			holdingRight = false;
			holdingLeft = false;
		}

		if (Input.GetAxis("Vertical") > RUN_JOYSTICK_THRESHOLD && !holdingUp) {
			holdingDown = false;
			holdingUp = true;
			InputBuffer.AddCons('^');
		} else if (Input.GetAxis("Vertical") < -1 * RUN_JOYSTICK_THRESHOLD && !holdingDown) {
			holdingUp = false;
			holdingDown = true;
			InputBuffer.AddCons('v');
		} else if (Mathf.Abs(Input.GetAxis("Vertical")) <= RUN_JOYSTICK_THRESHOLD
					&& (holdingUp || holdingDown)) {
			holdingUp = false;
			holdingDown = false;
		}

		if (Input.GetAxis("RightHorizontal") > ATTACK_JOYSTICK_THRESHOLD && !holdingRightAttack) {
			holdingLeftAttack = false;
			holdingRightAttack = true;
			InputBuffer.AddCons('r');
		} else if (Input.GetAxis("RightHorizontal") < -1 * ATTACK_JOYSTICK_THRESHOLD && !holdingLeftAttack) {
			holdingRightAttack = false;
			holdingLeftAttack = true;
			InputBuffer.AddCons('l');
		} else if (Mathf.Abs(Input.GetAxis("RightHorizontal")) <= ATTACK_JOYSTICK_THRESHOLD
					&& (holdingRightAttack || holdingLeftAttack)) {
			holdingRightAttack = false;
			holdingLeftAttack = false;
					}*/

		//Readout.ShowCurrentBuffer(InputBuffer.GetCurrentBuffer());

		/*if (bufferIndex > 0) {
			latest = PopBuffer();
		}*/

		if (grounded && Input.GetAxis("Horizontal") > RUN_JOYSTICK_THRESHOLD && !rightBlocked && Mathf.Abs(RB.velocity.x) < RUN_STARTUP) {
			if (wallToLeft) {
				wallToLeft = false;
				wallSliding = false;
			}
			if (wallToRight) {
				wallSliding = true;
			} else {
				temp.x = RUN_STARTUP;
			}
		}

//		if (grounded && Input.GetKeyDown(/*KeyCode.LeftArrow*/"a") && !leftBlocked && RB.velocity.x == 0) {
		if (grounded && Input.GetAxis("Horizontal") < -1 * RUN_JOYSTICK_THRESHOLD && !leftBlocked && Mathf.Abs(RB.velocity.x) < RUN_STARTUP) {
			if (wallToRight) {
				wallToRight = false;
				wallSliding = false;
			}
			if (wallToLeft) {
				wallSliding = true;
			} else {
				temp.x = -1 * RUN_STARTUP;
			}
		}

//		if (Input.GetKey(/*KeyCode.RightArrow*/"d") && !rightBlocked) {
		if (Input.GetAxis("Horizontal") > RUN_JOYSTICK_THRESHOLD && !rightBlocked) {
			if (wallToLeft) {
				wallToLeft = false;
				wallSliding = false;
			}
			if (wallToRight) {
				wallSliding = true;
			} else /*if (!(RB.velocity.x > RUN_VELOCITY))*/{
				temp.x += RUN_VELOCITY * Time.deltaTime;
			}
//		} else if (Input.GetKey(/*KeyCode.LeftArrow*/"a") && !leftBlocked) {
		} else if (Input.GetAxis("Horizontal") < -1 * RUN_JOYSTICK_THRESHOLD && !leftBlocked) {
			if (wallToRight) {
				wallToRight = false;
				wallSliding = false;
			}
			if (wallToLeft) {
				wallSliding = true;
			} else /* if (!(RB.velocity.x < (-1 * RUN_VELOCITY)))*/{
				temp.x -= RUN_VELOCITY * Time.deltaTime;
			}
		}

		if (!grounded && !dashing) {
			if (RB.velocity.y > (-1 * TERMINAL_FALLING_VELOCITY)) {
				temp.y -= GRAVITY * Time.deltaTime;
			} else {
				temp.y = -1 * TERMINAL_FALLING_VELOCITY;
			}
		}

		if (wallSliding && !grounded) {
			if (Mathf.Abs(RB.velocity.y) > WALL_SLIDE_TERMINAL_VELOCITY) {
				temp.y += ((RB.velocity.y > 0) ? -1 : 1) * WALL_FRICTION * Time.deltaTime;
			}
		}

		if (RB.velocity.x > FRICTION_SNAP_THRESHOLD) {
//			if (Input.GetKey(/*KeyCode.RightArrow*/"d")) {
			if (Input.GetAxis("Horizontal") > RUN_JOYSTICK_THRESHOLD && !rightBlocked) {
				if (RB.velocity.x >= RUN_TERMINAL_VELOCITY) {
					temp.x -= (grounded) ? HORIZ_SURFACE_FRICTION * Time.deltaTime :
					 					   HORIZ_AIR_RESISTANCE * Time.deltaTime;
				}
//			} else if (Input.GetKey(/*KeyCode.LeftArrow*/"a")) {
			} else if (Input.GetAxis("Horizontal") < -1 * RUN_JOYSTICK_THRESHOLD && !leftBlocked) {
				temp.x -= (grounded) ? HORIZ_SURFACE_FRICTION * 3 * Time.deltaTime :
									   HORIZ_AIR_RESISTANCE * 4 * Time.deltaTime;
			} else {
				temp.x -= (grounded) ? HORIZ_SURFACE_FRICTION * Time.deltaTime :
										HORIZ_AIR_RESISTANCE * Time.deltaTime;
			}
		} else if (RB.velocity.x < -1 * FRICTION_SNAP_THRESHOLD) {
//			if (Input.GetKey(/*KeyCode.LeftArrow*/"a")) {
			if (Input.GetAxis("Horizontal") < -1 * RUN_JOYSTICK_THRESHOLD && !leftBlocked) {
				if (RB.velocity.x < -1 * RUN_TERMINAL_VELOCITY) {
					temp.x += (grounded) ? HORIZ_SURFACE_FRICTION * Time.deltaTime :
										   HORIZ_AIR_RESISTANCE * Time.deltaTime;
				}
//			} else if (Input.GetKey(/*KeyCode.RightArrow*/"d")) {
			} else if (Input.GetAxis("Horizontal") > RUN_JOYSTICK_THRESHOLD && !rightBlocked) {
				temp.x += (grounded) ? HORIZ_SURFACE_FRICTION * 3 * Time.deltaTime :
									   HORIZ_AIR_RESISTANCE * 4 * Time.deltaTime;
			} else {
				temp.x += (grounded) ? HORIZ_SURFACE_FRICTION * Time.deltaTime :
									   HORIZ_AIR_RESISTANCE * Time.deltaTime;
			}
//		} else if (RB.velocity.x != 0 && !Input.GetKey(/*KeyCode.RightArrow*/"d") && !Input.GetKey(/*KeyCode.LeftArrow*/"a")) {
		} else if (RB.velocity.x != 0 && Input.GetAxis("Horizontal") == 0) {
			temp.x = 0;
		}

		if (Input.GetKeyUp(/*KeyCode.RightArrow*/"d") && RB.velocity.x > 0 && RB.velocity.x < RUN_VELOCITY) {
			//temp.x = 0;
		} else if (Input.GetKeyUp(/*KeyCode.LeftArrow*/"a") && RB.velocity.x < 0 && RB.velocity.x > -1 * RUN_VELOCITY) {
			//temp.x = 0;
		}

//		if (Input.GetKeyUp(/*KeyCode.RightArrow*/"d") && wallSliding && wallToRight) {
		if (Input.GetAxis("Horizontal") == 0 && wallSliding && wallToRight) {
			//wallSliding = false;
//		} else if (Input.GetKeyUp(/*KeyCode.LeftArrow*/"a") && wallSliding && wallToLeft) {
		} else if (Input.GetAxis("Horizontal") == 0 && wallSliding && wallToLeft) {
			//wallSliding = false;
		}

		if (wallSliding && Input.GetAxis("Vertical") <= -1) {
			wallSliding = false;
		}

//		if (Input.GetKeyDown(KeyCode.Space) && (grounded || wallSliding) /*|| (Input.GetKeyDown("z") /* && jumps > 0*/) {

		if (Input.GetAxis("RightTrigger"/*"Jump"*/) > 0.8f && (grounded || wallSliding) && !holdingJump) {
			holdingJump = true;
			holdingDash = true;
			SelfJump();
		} else if (Input.GetAxis("RightTrigger"/*"Jump"*/) <= 0.2f) {
			holdingJump = false;
			holdingDash = false;
		}

//		if (Input.GetKeyDown(KeyCode.LeftShift) && !attacking && canDash) {
		if (Input.GetAxis("RightTrigger") > 0.6f && /*!attacking &&*/ canDash /*&& !dashing*/ && !holdingDash) {
			StartCoroutine(Dash());
		}

		holdingDash = Input.GetAxis("RightTrigger") > 0.3f;

/*		if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow) ||
			Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))*/
		if ((Mathf.Abs(Input.GetAxis("RightHorizontal")) > ATTACK_JOYSTICK_THRESHOLD 
			|| Mathf.Abs(Input.GetAxis("RightVertical")) > ATTACK_JOYSTICK_THRESHOLD)
			&& !attacking && !dashAttacking) {
			attacking = true;
			StartCoroutine(Attack(CalculateAttackVector(HITBOX_DISTANCE)));
		}

		if (!dashing) {
			RB.velocity = temp;
		}

	/*	
		temp = transform.localScale;

		if (Mathf.Abs(RB.velocity.x) > STRETCH_THRESHOLD) {
			stretchPercentage = Mathf.Abs(RB.velocity.x) / DASH_MAGNITUDE;
			if (Mathf.Abs(transform.localScale.x) < MAX_WIDTH) {
				temp.x += stretchPercentage * STRETCH_RATE * Time.deltaTime;
			}
			if (Mathf.Abs(transform.localScale.y) > MIN_HEIGHT) {
				temp.y -= stretchPercentage * STRETCH_RATE * Time.deltaTime;
			} else {
				temp.y = MIN_HEIGHT;
			}
		} else if (Mathf.Abs(transform.localScale.x - 1) > SCALE_SNAP_THRESHOLD) {
			temp.x += ((transform.localScale.x > 1) ? -1 : 1) * STRETCH_RATE * Time.deltaTime;
		} else {
			temp.x = 1;
		}

		if (Mathf.Abs(RB.velocity.y) > STRETCH_THRESHOLD) {
			stretchPercentage = Mathf.Abs(RB.velocity.y) / TERMINAL_FALLING_VELOCITY;
			if (Mathf.Abs(transform.localScale.y) < MAX_HEIGHT) {
				temp.y += stretchPercentage * STRETCH_RATE * Time.deltaTime;
			}
			if (Mathf.Abs(transform.localScale.x) > MIN_WIDTH) {
				temp.x -= stretchPercentage * STRETCH_RATE * Time.deltaTime;
			} else {
				temp.x = MIN_WIDTH;
			}
		} else if (Mathf.Abs(transform.localScale.y - 1) > SCALE_SNAP_THRESHOLD) {
			temp.y += ((transform.localScale.y > 1) ? -1 : 1) * STRETCH_RATE * Time.deltaTime;
		} else {
			temp.y = 1;
		}

		transform.localScale = temp;
		*/
		

	}

	private void AddToBuffer() {
		if (bufferIndex >= buffer.Length - 1) {
			return;
		}
		string frameTotal = "";
		bool up, down, left, right, z, x, c;
		up = Input.GetKeyDown(/*KeyCode.UpArrow*/"w") || Input.GetKey(/*KeyCode.UpArrow*/"w");
		down = Input.GetKeyDown(/*KeyCode.DownArrow*/"s") || Input.GetKey(/*KeyCode.DownArrow*/"s");
		left = Input.GetKeyDown(/*KeyCode.LeftArrow*/"a") || Input.GetKey(/*KeyCode.LeftArrow*/"a");
		right = Input.GetKeyDown(/*KeyCode.RightArrow*/"d") || Input.GetKey(/*KeyCode.RightArrow*/"d");
		z = Input.GetKeyDown("z") || Input.GetKey("z");
		x = Input.GetKeyDown("x") || Input.GetKey("x");
		c = Input.GetKeyDown("c") || Input.GetKey("c");
		if ((up && down) || (left && right)) {
			if (z) {
				buffer[bufferIndex++] = "z";
				return;
			}
		}
		if (up) {
			frameTotal += "^";
		}
		if (right) {
			frameTotal += ">";
		}
		if (left) {
			frameTotal += "<";
		}
		if (down) {
			frameTotal += "v";
		}
		if (up || down || left || right) {
			if (x) {
				frameTotal += "x";
			} else if (c) {
				frameTotal += "c";
			}
		}
		if (z) {
			frameTotal += "z";
		}
		if (frameTotal != "") {
			buffer[bufferIndex++] = frameTotal;
		}
	}

	private string PopBuffer() {
		if (buffer[0] == null || buffer[0] == "") {
			return "";
		}
		string result = buffer[0];
		for(int i = 0; i < buffer.Length - 1; i++) {
			buffer[i] = buffer[i+1];
		}
		bufferIndex -= 1;
		return result;
	}

	private Vector2 CalculateAttackVector(float magnitude) {
		bool right = Input.GetAxis("RightHorizontal") > ATTACK_JOYSTICK_THRESHOLD;
		bool up = Input.GetAxis("RightVertical") > ATTACK_JOYSTICK_THRESHOLD;
		bool left = Input.GetAxis("RightHorizontal") < -1 * ATTACK_JOYSTICK_THRESHOLD;
		bool down = Input.GetAxis("RightVertical") < -1 * ATTACK_JOYSTICK_THRESHOLD;
		Vector2 result = new Vector2(0f, 0f);
		float angle = 45;
		if (up && right) {
			result.x = magnitude * Mathf.Sin(angle);
			result.y = magnitude * Mathf.Sin(angle);
		} else if (up && left) {
			result.x = -1 * magnitude * Mathf.Sin(angle);
			result.y = magnitude * Mathf.Sin(angle);
		} else if (up && !down) {
			result.y = magnitude;
		} else if (right && down) {
			result.x = magnitude * Mathf.Sin(angle);
			result.y = -1 * magnitude * Mathf.Sin(angle);
		} else if (right && !left) {
			result.x = magnitude;
		} else if (down && left) {
			result.x = -1 * magnitude * Mathf.Sin(angle);
			result.y = -1 * magnitude * Mathf.Sin(angle);
		} else if (down && !up) {
			result.y = -1 * magnitude;
		} else if (left && !right) {
			result.x = -1 * magnitude;
		}
		return result;
	}

	private Vector2 CalculateInputVector(float magnitude) {
		bool right = Input.GetAxis("Horizontal") > RUN_JOYSTICK_THRESHOLD;
		bool up = Input.GetAxis("Vertical") > RUN_JOYSTICK_THRESHOLD;
		bool left = Input.GetAxis("Horizontal") < -1 * RUN_JOYSTICK_THRESHOLD;
		bool down = Input.GetAxis("Vertical") < -1 * RUN_JOYSTICK_THRESHOLD;
		Vector2 result = new Vector2(0f, 0f);
		float angle = 45;
		if (up && right) {
			result.x = magnitude * Mathf.Sin(angle);
			result.y = magnitude * Mathf.Sin(angle);
		} else if (up && left) {
			result.x = -1 * magnitude * Mathf.Sin(angle);
			result.y = magnitude * Mathf.Sin(angle);
		} else if (up && !down) {
			result.y = magnitude;
		} else if (right && down) {
			result.x = magnitude * Mathf.Sin(angle);
			result.y = -1 * magnitude * Mathf.Sin(angle);
		} else if (right && !left) {
			result.x = magnitude;
		} else if (down && left) {
			result.x = -1 * magnitude * Mathf.Sin(angle);
			result.y = -1 * magnitude * Mathf.Sin(angle);
		} else if (down && !up) {
			result.y = -1 * magnitude;
		} else if (left && !right) {
			result.x = -1 * magnitude;
		}
		return result;
	}

	private IEnumerator WindUp(Vector2 direction) {
		attacking = true;
		dashAttacking = dashing;
		for(float i = 0; i < HITBOX_WINDUP; i += Time.deltaTime) {
			if (grounded || wallSliding) {
				//attacking = false;
				StartCoroutine(EndLag());
				yield break;
			}
			yield return null;
		}
		StartCoroutine(Attack(direction));
	}

	private IEnumerator Attack(Vector2 direction, float duration) {
		bool startedWithDash = dashing || dashAttacking;
		if (transform.Find("Hitbox(Clone)") != null) {
			yield break;
		}
		attacking = true;
		lastAttack = direction;

		GameObject hb = Object.Instantiate(hitbox, transform);
		if (direction.x * direction.y == 0) {
			hb.transform.localPosition = new Vector2(
				direction.x * HITBOX_DISTANCE,
				direction.y * HITBOX_DISTANCE
			);
		} else {
			hb.transform.localPosition = new Vector2(
				direction.x * HITBOX_DISTANCE,
				direction.y * HITBOX_DISTANCE
			);
		}
		hb.transform.localScale = new Vector2(HITBOX_SIZE, HITBOX_SIZE);
		for(float i = 0; i < duration; i += Time.deltaTime) {
			if (!startedWithDash && dashing) {
				Object.Destroy(hb);
				attacking = false;
				dashAttacking = false;
				yield break;
			}
			if (grounded || wallSliding || postHit) {
//				postHit = false;
				Object.Destroy(hb);
				attacking = false;
				StartCoroutine(EndLag());
				yield break;
			}
			yield return null;
		}
		Object.Destroy(hb);
		StartCoroutine(EndLag());
	}

	private IEnumerator Attack(Vector2 direction) {
		StartCoroutine(Attack(direction, HITBOX_DURATION));
		yield return null;
	}

	private IEnumerator EndLag() {
		for(float i = 0; i < HITBOX_END_LAG; i += Time.deltaTime) {
			if (grounded || wallSliding) {
				dashAttacking = false;
				attacking = false;
				yield break;
			}
			yield return null;
		}
		attacking = false;
		dashAttacking = false;
	}

	public void LandedAttack() {
		attacking = false;
		StartCoroutine(PostHit());
		postHit = true;
		StartCoroutine(EndLag());
		canDash = true;
		SR.color = defaultColor;
		if (dashing || dashAttacking) {
			dashAttacking = false;
			temp = RB.velocity;
			temp.y = DASH_ATTACK_KNOCKBACK;
			RB.velocity = temp;
		} else {
			temp = RB.velocity;
			temp.y = ATTACK_KNOCKBACK;
			RB.velocity = temp;
		}
	}

	private IEnumerator PostHit() {
		postHit = true;
		for(float i = 0; i < POST_HIT_DURATION; i += Time.deltaTime) {
			if (!postHit) {
				yield break;
			}
			yield return null;
		}
		postHit = false;
	}

	private void SelfJump() {
		if (wallSliding) {
			temp.x += (wallToLeft) ? WALL_JUMP_VELOCITY : -1 * WALL_JUMP_VELOCITY;
			StartCoroutine(BlockDirection((wallToLeft) ? Vector2.left : Vector2.right, WALL_JUMP_STALL));
			wallSliding = false;
		}
		temp.y = JUMP_VELOCITY;
	}
	
	public void Jump() {
		temp = RB.velocity;
		if (wallSliding) {
			temp.x += (wallToLeft) ? WALL_JUMP_VELOCITY : -1 * WALL_JUMP_VELOCITY;
			StartCoroutine(BlockDirection((wallToLeft) ? Vector2.left : Vector2.right, WALL_JUMP_STALL));
			wallSliding = false;
		}
		temp.y = JUMP_VELOCITY;
		RB.velocity = temp;
	}

	public void Die() {
		transform.position = checkPoint;
		RB.velocity = Vector2.zero;
		grounded = false;
	}

	public void SetGrounded(bool b) {
		grounded = b;
	}

	private IEnumerator BlockDirection(Vector2 direction, float time) {
		if (direction == Vector2.right) {
			rightBlocked = true;
		} else if (direction == Vector2.left) {
			leftBlocked = true;
		}
		for(float i = 0; i < time; i += Time.deltaTime) {
			yield return null;
		}
		if (direction == Vector2.right) {
			rightBlocked = false;
		} else if (direction == Vector2.left) {
			leftBlocked = false;
		}	
	}

	private IEnumerator Dash() {
		bool startedOnGround = grounded;
		dashing = true;
		canDash = false;
		Vector2 direction = CalculateInputVector(DASH_MAGNITUDE);
		Vector2 lastVelocity = RB.velocity;
		if (direction == Vector2.zero) {
			dashing = false;
//			if (grounded) {
			canDash = true;
			yield break;
//			}
		}
		lastDash = direction;
		RB.velocity = direction;
		bool sliding = false;
		SR.color = dashingColor;
		for(float i = 0; i < DASH_DURATION; i += Time.deltaTime) {
			if (/*Input.GetAxis("DashAttack") > 0 && */attacking && !dashAttacking) {
				dashAttacking = true;
				StartCoroutine(Attack(/*direction * (HITBOX_DISTANCE / DASH_MAGNITUDE)*/lastAttack, DASH_DURATION - i));
			}
			if (grounded) {
				if (direction.x * direction.y != 0) {
					dashing = false;
					if (!startedOnGround) {
						WaveDash((i < 0.05f) ? 1 : (DASH_DURATION - i) / DASH_DURATION);
						StartCoroutine(BlockDirection((direction.x > 0) ? Vector2.right : Vector2.left, /*0.11f*/DASH_DURATION));
						sliding = true;
						SR.color = defaultColor;
						yield break;
					}
				} else if (startedOnGround && direction.y < 0) {
					dashing = false;
					canDash = true;
					SR.color = defaultColor;
					yield break;
				}
				/*dashing = false;
				canDash = true;
				yield break;*/
			} else if (wallToLeft || wallToRight) {
				dashing = false;
				wallSliding = true;
				sliding = true;
				if (direction.x * direction.y != 0) {
					WallDash((i < 0.05f) ? 1 : (DASH_DURATION - i) / DASH_DURATION);
				} else {
					CameraRumble();
				}
				//SR.sprite = defaultSprite;
				SR.color = usedDashColor;
				yield break;
			} else if (grounded) {
				dashing = false;
				sliding = false;
				canDash = true;
				if (direction == DASH_MAGNITUDE * Vector2.down) {
					CameraRumble();
				}
				SR.color = defaultColor;
				yield break;
			}
			if (postHit) {
				postHit = false;
				canDash = true;
				dashing = false;
				sliding = false;
				SR.color = defaultColor;
				yield break;
			}
			yield return null;
		}
		if (!sliding && !postHit) {
			temp = RB.velocity;
			//temp.x = 0;
			temp.x /= 4;
			temp.y /= (temp.y > 0) ? 2 : 1;
			RB.velocity = temp;
		}
		dashing = false;
		if (!grounded && !postHit) {
			SR.color = usedDashColor;
		} else {
			SR.color = defaultColor;
		}
		canDash = grounded;
	}

	private void CameraRumble() {
		StartCoroutine(Camera.main.GetComponent<CameraMove>().Rumble());
	}

	private void WaveDash(float boost) {
		bool left = lastDash.x < 0;
		bool right = lastDash.x > 0;
		float verticalSpeed = Mathf.Abs(lastDash.y);
		temp = RB.velocity;
		if (left) {
			temp.x -= verticalSpeed * boost;
			RB.velocity = temp;
		} else if (right) {
			temp.x += verticalSpeed * boost;
			RB.velocity = temp;
		}
	}

	private void WallDash(float boost) {
		wallSliding = true;
		bool up = lastDash.y > 0;
		bool down = lastDash.y < 0;
		float horizontalSpeed = Mathf.Abs(lastDash.x);
		temp = RB.velocity;
		if (up) {
			temp.y += horizontalSpeed * boost;
			RB.velocity = temp;
		} else if (down) {
			temp.y -= horizontalSpeed * boost;
			RB.velocity = temp;
		}
	}

	private bool LeftOf(Transform other) {
		return transform.position.x + (transform.localScale.x / 2) < 
			other.position.x - (other.localScale.x / 2) + SINKING_THRESHOLD ;
	}

	private bool RightOf(Transform other) {
		return transform.position.x - (transform.localScale.x / 2) >
			other.position.x + (other.localScale.x / 2) - SINKING_THRESHOLD;
	}

	private bool OnTopOf(Transform other) {
		return transform.position.y - (transform.localScale.y / 2) >=
			other.position.y + (other.localScale.y / 2) - SINKING_THRESHOLD;
	}

	private bool Below(Transform other) {
		return transform.position.y + (transform.localScale.y / 2) <=
			other.position.y - (other.localScale.y / 2) + SINKING_THRESHOLD;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.tag == "Spikes") {
			Die();
		} else if (collision.collider.tag == "Bullet") {
			return;
		}
		Transform other = collision.collider.transform;
		bool leftOf = LeftOf(other);
		bool rightOf = RightOf(other);
		bool onTopOf = OnTopOf(other);
		bool below = Below(other);
		if (!leftOf && !rightOf) {
			grounded = onTopOf;
			canDash = onTopOf;
		} else if (leftOf) {
			wallToRight = !onTopOf && !below;
			wallSliding = wallToRight;
			if (grounded) {
				wallSliding = false;
			}
		} else if (rightOf) {
			wallToLeft = !onTopOf && !below;
			wallSliding = wallToLeft;
			if (grounded) {
				wallSliding = false;
			}
		}
		if (grounded) {
			SR.color = defaultColor;
		}
	}

	void OnCollisionStay2D(Collision2D collision) {
		Transform other = collision.collider.transform;
		bool leftOf = LeftOf(other);
		bool rightOf = RightOf(other);
		bool onTopOf = OnTopOf(other);
		bool below = Below(other);
		if (!leftOf && !rightOf) {
			grounded = onTopOf;
			canDash = onTopOf;
		} else if (leftOf) {
			wallToRight = !onTopOf && !below;
			if (grounded) {
				wallSliding = false;
			}
		} else if (rightOf) {
			wallToLeft = !onTopOf && !below;
			if (grounded) {
				wallSliding = false;
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		Transform other = collision.collider.transform;
		bool leftOf = LeftOf(other);
		bool rightOf = RightOf(other);
		bool onTopOf = OnTopOf(other);
		bool below = Below(other);
		if (onTopOf) {
			grounded = false;
		}
		if (leftOf) {
			wallToRight = false;
			wallSliding = false;
		} else if (rightOf) {
			wallToLeft = false;
			wallSliding = false;
		} else {
			grounded = false;
		}
	}
}
