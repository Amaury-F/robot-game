using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public float enemySpeed;
    public GameObject enemy;

    private bool canFlip = true;
    private bool facingRight = false;
    private float flipTime = 5f;
    private float nextFlipChance = 0f;

    public float chargeTime;
    float startChargeTime;
    bool charging;
    Rigidbody2D enemyRB;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextFlipChance)
        {
            if (Random.Range(0, 10) >= 5)
                flipFacing();
            nextFlipChance = Time.time + flipTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (facingRight && collision.transform.position.x < transform.position.x)
                flipFacing();
            else if (!facingRight && collision.transform.position.x > transform.position.x)
                flipFacing();
        }
        canFlip = false;
        charging = true;
        startChargeTime = Time.time + chargeTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (startChargeTime >= Time.time)
            {
                if (!facingRight)
                    enemyRB.AddForce(new Vector2(-1, 0) * enemySpeed);
                else
                    enemyRB.AddForce(new Vector2(1, 0) * enemySpeed);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canFlip = true;
            charging = false;
            enemyRB.velocity = new Vector2(0f, 0f);
        }
    }

    void flipFacing()
    {
        if (!canFlip)
            return;

        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = enemy.transform.localScale;
        theScale.x *= -1;
        enemy.transform.localScale = theScale;
    }
}
