using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class VisibleWhenOccupied : MonoBehaviour
{
    private Occupiable check;

    private bool lastOn = true;

    // Start is called before the first frame update
    void Start()
    {
        check = GetComponent<Occupiable>();
        if (check == null)
        {
            throw new Exception("No occupiable component found for component");
        }
    }

    // Update is called once per frame
    void Update()
    {
        var on = check.occupier != null;

        if (on == lastOn)
        {
            return;
        }

        lastOn = on;

        foreach (var rend in GetComponentsInChildren<Renderer>())
        {
            rend.enabled = on;
        }

        foreach (var click in GetComponentsInChildren<Collider2D>())
        {
            click.enabled = on;
        }
    }
}
