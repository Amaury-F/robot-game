using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField]
    Mecanism mecanism;

    private bool isActivated = false;

    public void Interact()
    {
        if (isActivated)
        {
            isActivated = false;
            mecanism.DeactivateMecanism();
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (!isActivated)
        {
            isActivated = true;
            mecanism.ActivateMecanism();
            GetComponent<SpriteRenderer>().flipX = true;

        }
    }

}
