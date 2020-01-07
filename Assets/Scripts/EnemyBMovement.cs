using System;
using UnityEngine;
using System.Collections;

// Drone
public class EnemyBMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float attackDistance = 3f;
    public float forgetDistance = 8f;
    public float chaseSpeedBoost = 1f;
    public Transform target;
    
    private bool isChasing;
    private Vector2 moveAmount;
    private float moveDirection = 1.0f;
    private int unit = 500;
    private Vector3 initialPosition;
    private Controller2D controller;

    void Start()
    {
        initialPosition = transform.position;
        controller = GetComponent<Controller2D>();
    }

    void FixedUpdate() {

        isChasing = CheckPlayerInRange();


        if (isChasing) {
            Chase();
        } else {
            Return(initialPosition);
            Wander();
        }

        unit--;

        if (unit < 1)
        {
            Flip();
            unit = 500;
        }
    }

    bool CheckPlayerInRange() {
        float dist = Vector3.Distance(target.position, transform.position);
        if (isChasing && dist > forgetDistance) {
            return false;
        }

        if (dist < attackDistance) {
            var targetPos = target.position;
            var originPos = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(originPos, targetPos - originPos,
                (targetPos - originPos).magnitude, controller.collisionMask);
            
            return !hit;
        }

        return isChasing;

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
        //transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    private void Wander()
    {
        moveAmount.x = moveDirection * moveSpeed * Time.deltaTime;
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        
    }
}