using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAttraction : ForceGenerator
{
    [SerializeField] float attractionForce = 25f;   //attraction force
    public override void UpdateGenerator() 
    {
        //move towards mouse
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePos - transform.position).normalized;
            force = direction * attractionForce;
        }

        else
            force = Vector2.zero;
    }
}
