using UnityEngine;
using System.Collections;

public class PushPull : MonoBehaviour
{

    public bool pushing;
    public LayerMask boxLayer;
    public float distance = 2;
    
    private Vector2 offset;
    private GameObject box;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance, boxLayer);

        if (hit.collider != null && Input.GetKeyDown(KeyCode.E))
        {
            pushing = true;

            box = hit.collider.gameObject;
            offset = box.transform.position - transform.position;
            box.GetComponent<FixedJoint2D>().enabled = true;
            box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();

            box.GetComponent<BoxPull>().beingPushed = true;
        }
        else if (Input.GetKey(KeyCode.E))
        {


            //box.transform.position = (Vector2)transform.position + offset;
        }
        else if (box != null)
        {
            box.GetComponent<BoxPull>().beingPushed = false;
            box.GetComponent<FixedJoint2D>().enabled = false;
        }

    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    //}
}