using UnityEngine;
using System.Collections;

public class EnemyAMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float attackDistance = .2f;
    public Transform target;
    private bool facingTarget = false;
    private Vector2 moveAmount;
    private float moveDirection = 1.0f;
    private float attackSpeed = 2f;

    void Start()
    {

    }

    void FixedUpdate()
    {
        float dist = Vector3.Distance(target.position, transform.position);
        Debug.Log(IsFacingObject());

        if (dist < attackDistance && IsFacingObject())
            Chase();
        else
            Wander();
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Objects"))
            Flip();

        if (other.gameObject.layer == LayerMask.NameToLayer("Level"))
        {
            if (other.gameObject.name != "Ground")
                Flip();
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            Destroy(gameObject, 2);
    }

    void Flip()
    {
        moveDirection *= -1;
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }

    void Chase()
    {
        Debug.Log("Hey");
        transform.position = Vector2.MoveTowards(transform.position, target.position, attackSpeed * Time.deltaTime);
    }

    void Wander()
    {
        moveAmount.x = moveDirection * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);
    }

    bool IsFacingObject()
    {
        // Check if the gaze is looking at the front side of the object
        Vector3 forward = transform.right;
        Vector3 toOther = (target.position - transform.position).normalized;

        RaycastHit _hit = new RaycastHit();

        if (Physics.Raycast(transform.position, transform.right, out _hit, 1))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * _hit.distance, Color.yellow);
            Debug.Log("COUCOU !!!!!!");
        }

        if (Vector3.Dot(forward, toOther) < 0.7f)
        {
            Debug.Log("Not facing the object");
            return false;
        }

        Debug.Log("Facing the object");
        return true;
    }
}