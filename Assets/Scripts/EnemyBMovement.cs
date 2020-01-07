using System;
using UnityEngine;
using System.Collections;

// Drone
public class EnemyBMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float forgetDistance = 8f;
    public float chaseSpeedBoost = 1f;
    public float walkDist = 10;
    
    private Transform target;
    
    private bool isChasing;
    private Vector2 moveAmount;
    private float moveDirection = 1.0f;
    private float walked = 0;
    private Vector3 initialPosition;
    private Controller2D controller;
    private DetectionZone detection;

    void Start()
    {
        initialPosition = transform.position;
        controller = GetComponent<Controller2D>();
        detection = transform.GetChild(0).gameObject.GetComponent<DetectionZone>();
        
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate() {

        isChasing = CheckPlayerInRange();


        if (isChasing) {
            Chase();
        } else {
            Return(initialPosition);
            Wander();
        }
    }

    bool CheckPlayerInRange() {
        float dist = Vector3.Distance(target.position, transform.position);
        if (isChasing) {
            return dist < forgetDistance;
        }

        return detection.TryDetectPlayer();
    }

    private void Flip()
    {
        moveDirection *= -1;
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }

    private void Chase()
    {
        Vector3 chaseDir = (target.position - transform.position).normalized;
        controller.Move(Time.deltaTime * moveSpeed * chaseSpeedBoost * chaseDir);
    }

    private void Wander()
    {
        moveAmount.x = moveDirection * moveSpeed * Time.deltaTime;
        walked += Math.Abs(moveAmount.x);
        if (walked > walkDist) {
            walked = 0;
            Flip();
        }
        if (controller.Move(moveAmount)) {
            Flip();
        }
    }

    void Return(Vector3 initPos)
    {
            transform.position = Vector2.MoveTowards(transform.position, new Vector3 (transform.position.x, initPos.y, transform.position.z), moveSpeed * Time.deltaTime);
    }
    
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, forgetDistance);

        Gizmos.color = Color.blue;
        var transform1 = transform;
        Vector3 a = transform1.position;
        a.x += walkDist;
        Gizmos.DrawLine(transform1.position, a);
    }
}