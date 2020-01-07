using UnityEngine;
using System.Collections;

public class EnemyAMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float attackDistance = .2f;
    public Transform target;

    public LayerMask avoidance;

    private bool facingTarget = false;
    private Vector2 moveAmount;
    private float moveDirection = 1.0f;
    private float attackSpeed = 2f;
    private int unit = 600;
    private Controller2D controller;

    public GameObject fieldOfView;

    void Start()
    {
        controller = GetComponent<Controller2D>();
    }

    void FixedUpdate() {
        float dist = Vector3.Distance(target.position, transform.position);

        if (dist < attackDistance && IsFacingObject())
            Chase();
        else
            Wander();

        unit--;

        if (unit < 1)
        {
            Flip();
            unit = 600;
        }
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
        Vector3 vector = target.position - transform.position;
        vector = vector.normalized;
        controller.Move(vector * attackSpeed * Time.deltaTime);
        //transform.position = Vector2.MoveTowards(transform.position, target.position, attackSpeed * Time.deltaTime);
    }

    void Wander()
    {
        moveAmount.x = moveDirection * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);
    }

    bool IsFacingObject()
    {
        BoxCollider2D collider = fieldOfView.GetComponent<BoxCollider2D>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            //if(go.gameObject.layer == LayerMask.NameToLayer("Player"))
        }

            return collider.bounds.Contains(target.position);
    }
}