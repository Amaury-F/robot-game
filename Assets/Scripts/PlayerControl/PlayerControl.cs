using System;
using UnityEngine;
using System.Collections;
// ReSharper disable CheckNamespace

[RequireComponent (typeof (Controller2D))]
public class PlayerControl : MonoBehaviour {

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    private float gravity;
    private float jumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;

    private Animator animator;
    private SpriteRenderer renderer;

    private Controller2D controller;
    private static readonly int WalkDir = Animator.StringToHash("walkDir");

    void Start() {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();

        gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    void Update() {

        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown (KeyCode.Space) && controller.collisions.below) {
            velocity.y = jumpVelocity;
            animator.SetBool("isJumping", true);
        } else if (controller.collisions.below) {
            animator.SetBool("isJumping", false);
        }

        float targetVelocityX = input.x * moveSpeed;
        animator.SetInteger(WalkDir, Math.Sign(targetVelocityX));
        if (Math.Sign(targetVelocityX) < 0) {
            renderer.flipX = true;
        } else if (Math.Sign(targetVelocityX) > 0) {
            renderer.flipX = false;
        }
        
        
        velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move (velocity * Time.deltaTime);
    }
}
