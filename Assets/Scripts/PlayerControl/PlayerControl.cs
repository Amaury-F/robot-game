using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private bool isMovingCrate;
    private GameObject crateNear;
    private readonly List<BoxCollider2D> crates = new List<BoxCollider2D>();
    private Transform crateDetector;

    private Controller2D controller;
    private static readonly int WalkDir = Animator.StringToHash("walkDir");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");

    void Start() {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();

        foreach (var crate in GameObject.FindGameObjectsWithTag("Moveable")) {
            BoxCollider2D box = crate.GetComponent<BoxCollider2D>();
            if (box) {
                crates.Add(box);
            }
            
        }

        crateDetector = transform.GetChild(0);

        gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update() {

        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below) {
            velocity.y = jumpVelocity;
            animator.SetBool(IsJumping, true);
        } else if (controller.collisions.below) {
            animator.SetBool(IsJumping, false);
        }

        float targetVelocityX = input.x * moveSpeed;
        animator.SetInteger(WalkDir, Math.Sign(targetVelocityX));
        if (!isMovingCrate) {
            if (Math.Sign(targetVelocityX) < 0) {
                transform.localScale = new Vector3(-1, 1, 1);
            } else if (Math.Sign(targetVelocityX) > 0) {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;

        if (Input.GetKey(KeyCode.R)) {
            velocity.y -= (gravity - jumpVelocity/2) * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
        
        if (Input.GetKeyDown(KeyCode.E)) {
            if (isMovingCrate) {
                isMovingCrate = false;
                moveSpeed *= 3;
            } else if (CheckBlockNear()) {
                isMovingCrate = true;
                moveSpeed /= 3;
            }
        }

        if (isMovingCrate) {
            Controller2D crateController = crateNear.GetComponent<Controller2D>();

            Vector3 crateVelocity = velocity;
            crateVelocity.y = 0;
            crateVelocity.z = 0;
            crateController.Move(crateVelocity * Time.deltaTime);
        }
        
    }

    bool CheckBlockNear() {
        foreach (var crate in crates) {
            if (crate.bounds.Contains(crateDetector.position)) {
                crateNear = crate.gameObject;
                return true;
            }
        }

        return false;
    }
    
    
}
