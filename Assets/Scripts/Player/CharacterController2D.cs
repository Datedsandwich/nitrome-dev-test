﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class CharacterController2D : MonoBehaviour {
	// Threshold before input will be handled. Arbitrary.
	public float inputThreshold = 0.2f;

	public float moveSpeed = 2f;
	public float maxSpeed = 5f;
	public float jumpForce = 5f;

	public Transform groundCheck;
	public LayerMask groundLayerMask;

	public bool isGrounded = false;
	private bool facingRight = true;
	private bool jump = false;

	private Animator animator;
	private Rigidbody2D rigidbody2D;

	// Use this for initialization
	void Start() {
		animator = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update() {
		if(Input.GetButtonDown("Jump") && isGrounded) {
			jump = true;
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

		if(Mathf.Abs(horizontal) >= inputThreshold) {
			float currentSpeed = horizontal * rigidbody2D.velocity.x;

			if (currentSpeed < moveSpeed) {
				rigidbody2D.AddForce(Vector2.right * horizontal * (moveSpeed - currentSpeed), ForceMode2D.Impulse);
			}

			ConstrainSpeed();
			DetermineFacing(horizontal);
		}
	}

	private void HandleVerticalMovement() {
		if (jump) {
			Jump();
		}

		animator.SetFloat("VerticalSpeed", Mathf.Sign(rigidbody2D.velocity.y));
	}

	private void ConstrainSpeed() {
		if (Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed) {
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		}
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
		rigidbody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		jump = false;
	}
}
