using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class FallingObject : MonoBehaviour {

    private float gravity;
    private Controller2D controller;
    private Vector3 velocity;

    void Start() {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * 2.2f) / Mathf.Pow (0.4f, 2);
    }
    
    void Update() {
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
