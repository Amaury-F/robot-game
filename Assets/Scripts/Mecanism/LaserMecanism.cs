using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMecanism : Mecanism
{
    private GameObject laser;

    void Start()
    {
        laser = this.transform.GetChild(0).gameObject;
    }

    public override void ActivateMecanism()
    {
        laser.SetActive(false);
    }

    public override void DeactivateMecanism()
    {
        laser.SetActive(true);
    }
}
