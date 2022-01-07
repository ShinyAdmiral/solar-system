using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeForceGravity : ForceGenerator
{
    [SerializeField] Particle3D realtiveTarget = null;
    [SerializeField] float massScale = 1.0f;

    Game GM = null;
    Particle3D us;
    float distScale = 0.0f;

    private void Start() 
    {
        if (realtiveTarget != null)
            transform.parent = realtiveTarget.transform;
        GM = GameObject.FindObjectOfType<Game>();
        us = GetComponent<Particle3D>();
        distScale = 1.0f / transform.localScale.x;
        force = Vector3.zero;
    }

    // Update is called once per frame
    public override void UpdateGenerator()
    {
        if (realtiveTarget != null) {
            Vector3 dist = -transform.localPosition * distScale;
            force = GM.gravityConstant * (us.mass * (realtiveTarget.mass * massScale) / dist.sqrMagnitude) * dist.normalized;

            Debug.Log(dist);
        }
    }
}

