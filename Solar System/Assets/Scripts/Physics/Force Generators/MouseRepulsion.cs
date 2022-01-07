using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRepulsion : ForceGenerator
{
    [SerializeField] float repulsionForce = 25f;    //repulsion force
    public override void UpdateGenerator()
    {

        //apply force in direction
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (transform.position - mousePos).normalized;
            force = direction * repulsionForce;
        }

        else
            force = Vector2.zero;
    }
}
