using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPlane : ParticleCollider
{
    [HideInInspector]
    public Vector3 normal; //normal

    private void Awake()
    {
        //set up
        normal = transform.up;
        collisionType = CollisionType.PLANE;

       // Debug.Log(normal);
    }
}
