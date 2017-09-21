using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class CharacterController2D : MonoBehaviour {
	public float moveSpeed = 2f;
	public float maxSpeed = 5f;

	//Jump related variables
	public float initialJumpForce;
	public float extraJumpForce;
	public float maxExtraJumpTime;
	public float delayToExtraJumpForce;
	private float jumpTimer;
	private bool hasJumped;
	private bool isJumping;

	public LayerMask groundLayerMask;

	public bool isGrounded = false;
	private bool facingRight = true;

	private Animator animator;
	private new Rigidbody2D rigidbody2D;
	private Transform groundCheck;

	// Use this for initialization
	void Start() {
		animator = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();

		groundCheck = transform.Find("Ground Check");
	}
	
	// Update is called once per frame
	void Update() {
		if(Input.GetButtonDown("Jump") && isGrounded) {
			hasJumped = true;
			isJumping = true;
			jumpTimer = Time.time;
		}

		if(Input.GetButtonUp("Jump") || Time.time - jumpTimer > maxExtraJumpTime) {
			isJumping = false;
		}
	}

	// Perform Physics simulation within FixedUpdate. Runs 50 times per second.
	void FixedUpdate() {
		isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, groundLayerMask);
		animator.SetBool("Grounded", isGrounded);

		HandleHorizontalMovement(Input.GetAxis ("Horizontal"));
		HandleVerticalMovement();
	}

	private void HandleHorizontalMovement(float horizontal) {
		
		animator.SetFloat("WalkSpeed", Mathf.Abs(horizontal));

		rigidbody2D.velocity = new Vector2(horizontal * moveSpeed, rigidbody2D.velocity.y);

		ConstrainSpeed ();
		DetermineFacing(horizontal);
	}

	private void HandleVerticalMovement() {
		if (hasJumped) {
			Jump();
		}

		if(isJumping) {
			ExtendJump();
		}

		animator.SetFloat("VerticalSpeed", Mathf.Sign(rigidbody2D.velocity.y));
	}

	private void ConstrainSpeed() {
		rigidbody2D.velocity = Vector3.ClampMagnitude(rigidbody2D.velocity, maxSpeed);
	}

	private void DetermineFacing(float horizontal) {
		// Sprite faces left by default
		if (horizontal < 0 && !facingRight) {
			Flip();
		} else if (horizontal > 0 && facingRight) {
			Flip();
		}
	}

	private void Flip() {
		facingRight = !facingRight;
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
	}

	private void Jump() {
		animator.SetTrigger("Jump");

		rigidbody2D.AddForce(transform.up * initialJumpForce, ForceMode2D.Impulse);
		hasJumped = false;
	}

	private void ExtendJump() {
		if (Time.time - jumpTimer > delayToExtraJumpForce) {
			rigidbody2D.AddForce(transform.up * extraJumpForce);
		}
	}
}
