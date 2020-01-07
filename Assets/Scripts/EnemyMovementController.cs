using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    private bool isFacingRight = false;
    public float speed = 2f;
    public GameObject target;
    private Rigidbody2D enemy;
    bool moveRight;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.position.x <= target.transform.position.x)
            moveRight = false;
        else
            moveRight = true;

        if (moveRight)
        {
            enemy.velocity = new Vector2(-transform.localScale.x, 0) * speed;
            if (!isFacingRight)
                Flip();
        }

        if (!moveRight)
        {
            enemy.velocity = new Vector2(transform.localScale.x, 0) * speed;
            if (isFacingRight)
                Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
