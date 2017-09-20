using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour {
	public float moveForce = 30f;
	public float maxSpeed = 5f;
	public float jumpForce = 200f;

	public Transform groundCheck;
	public LayerMask groundLayerMask;

	public bool isGrounded = false;
	private bool facingRight = true;
	private bool jump = false;

	private Animator animator;
	private Rigidbody2D rigidbody2D;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		rigidbody2D = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump") && isGrounded) {
			jump = true;
		}
	}

	// Perform Physics simulation within FixedUpdate. Runs 50 times per second.
	void FixedUpdate() {
		isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, groundLayerMask);
		HandleHorizontalMovement (Input.GetAxis ("Horizontal"));

		if (jump) {
			Jump ();
		}
	}

	private void HandleHorizontalMovement(float horizontal) {
		animator.SetFloat("Speed", Mathf.Abs(horizontal));

		if (horizontal * rigidbody2D.velocity.x < maxSpeed) {
			rigidbody2D.AddForce(Vector2.right * horizontal * moveForce);
		}

		ConstrainSpeed ();
		DetermineFacing (horizontal);
	}

	private void ConstrainSpeed() {
		if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed) {
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		}
	}

	private void DetermineFacing(float horizontal) {
		// Sprite faces left by default
		if (horizontal < 0 && !facingRight) {
			Flip ();
		} else if (horizontal > 0 && facingRight) {
			Flip ();
		}
	}

	private void Flip() {
		facingRight = !facingRight;
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
	}

	private void Jump() {
		Debug.Log ("Jumping");
		animator.SetTrigger("Jump");
		rigidbody2D.AddForce(new Vector2(0f, jumpForce));
		jump = false;
	}
}
