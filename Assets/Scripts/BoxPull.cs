using UnityEngine;
using System.Collections;

public class BoxPull : MonoBehaviour {

    public bool beingPushed;
    float xPos;

    private Vector3 lastPos;
    // Use this for initialization
    void Start() {
        xPos = transform.position.x;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (beingPushed == false)
            transform.position = new Vector3(xPos, transform.position.y);
        else
            xPos = transform.position.x;
    }


}