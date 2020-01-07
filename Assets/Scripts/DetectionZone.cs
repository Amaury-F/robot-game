using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public LayerMask collisionMask;

    private float rayTargetOffset = 0.6f;

    private bool playerInTriggerZone;
    private GameObject rayPointOrigin;
    private GameObject player;

    private void Start()
    {
        rayPointOrigin = transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }


    public bool TryDetectPlayer()
    {
        if (playerInTriggerZone) {
            Vector2 rayOrigin = rayPointOrigin.transform.position;
            Vector2 rayTarget = new Vector2(player.transform.position.x, player.transform.position.y + rayTargetOffset) - rayOrigin;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayTarget, 15f, collisionMask);

            if (hit)
            {
                if (hit.transform.gameObject == player)
                {
                    Debug.DrawRay(rayOrigin, (hit.point - rayOrigin), Color.red, 0.01f);
                    return true;
                }
                else
                {
                    Debug.DrawRay(rayOrigin, (hit.point - rayOrigin), Color.green, 0.01f);
                }
            }
        }

        return false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTriggerZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInTriggerZone = false;
        }
    }
}
