using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderDetection : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            player.GetComponent<PlayerControl>().isOnLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            player.GetComponent<PlayerControl>().isOnLadder = false;
        }
    }
}
