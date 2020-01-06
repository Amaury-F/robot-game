using UnityEngine;
using System.Collections;

public class EnemyBMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float attackDistance = .5f;
    public Transform target;
    private Vector2 moveAmount;
    private float moveDirection = 1.0f;
    private int unit = 200;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        float dist = Vector3.Distance(target.position, transform.position);
        if (dist < attackDistance)
            Chase();
        else
        {
            Return(initialPosition);
            Wander();
        }

        unit--;

        if (unit < 1)
        {
            Flip();
            unit = 200;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Objects"))
            Flip();

        if (other.gameObject.layer == LayerMask.NameToLayer("Level"))
            Flip();

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            Destroy(gameObject, 2);
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
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    private void Wander()
    {
        moveAmount.x = moveDirection * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);
    }

    void Return(Vector3 initPos)
    {
            transform.position = Vector2.MoveTowards(transform.position, new Vector3 (transform.position.x, initPos.y, transform.position.z), moveSpeed * Time.deltaTime);
    }
}