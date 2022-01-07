using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringForce : ForceGenerator {
    [SerializeField] Transform otherPoint = null;   //point to move towrds
    public float  springConstant = 0.5f;            //spring constant
    public float springRestLength = 1f;             //rest length

    private void Start() 
    {
        //sync variables
        SpringForce otherEnd        = otherPoint.GetComponent<SpringForce>();
        otherEnd.springConstant     = springConstant;
        otherEnd.springRestLength   = springRestLength;
    }

    public override void UpdateGenerator() {
        //destroy if we don't exist
        if (otherPoint == null) {
            Destroy(gameObject);
            return;
        }

        //calc hooks law
        force               = transform.position - otherPoint.position;

        float magnitude     = force.magnitude;
        magnitude           = Mathf.Abs(magnitude - springRestLength);
        magnitude           *= springConstant;

        force.Normalize();
        force               *= -magnitude;
    }
}
