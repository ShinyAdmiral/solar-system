using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedSpring : ForceGenerator {
    [SerializeField] Transform fixedPoint = null; //point to move towrds
    public float springConstant = 0.5f;           //spring constant
    public float springRestLength = 1f;           //rest length


    public override void UpdateGenerator() {

        //calc hooks law
        force = transform.position - fixedPoint.position;

        float magnitude = force.magnitude;
        magnitude = (magnitude - springRestLength) * springConstant;

        force.Normalize();
        force *= -magnitude;
    }
}
