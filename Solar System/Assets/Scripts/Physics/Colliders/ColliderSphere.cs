using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSphere : ParticleCollider
{
    public float radius         = 1f;   //radius of sphere

    private void Awake()
    {
        //set up
        collisionType = CollisionType.SPHERE;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
