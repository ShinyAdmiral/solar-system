using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAABB : ParticleCollider
{
    public Vector3 boundingSize = Vector3.one;

    [HideInInspector]
    public Vector3 boundingSizeHalf;

    [HideInInspector]
    public Vector3 bottomLeftBack;
    
    [HideInInspector]
    public Vector3 topRightFront;

    private void Awake()
    {
        collisionType = CollisionType.AABB;
        boundingSizeHalf = boundingSize * 0.5f;
        bottomLeftBack = transform.position - boundingSizeHalf;
        topRightFront = transform.position + boundingSizeHalf;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boundingSize);
    }

}
