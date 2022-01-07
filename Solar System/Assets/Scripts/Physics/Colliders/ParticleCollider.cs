using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//collsiion enum
public enum CollisionType
{
    SPHERE,
    AABB,
    PLANE
}

[RequireComponent(typeof(Particle2D))]
public abstract class ParticleCollider : MonoBehaviour
{
    public float restitution = 1f;          //elasticity

    [HideInInspector]
    public CollisionType collisionType;     //colider type

    [HideInInspector]
    public Particle2D physics;              //refecen to physic

    [HideInInspector]
    protected CollisionManager CM;

    //get physics component
    void Start() 
    {
        physics = GetComponent<Particle2D>();
        CM = FindObjectOfType<CollisionManager>();
        CM.particleList.Add(this);
    }
}
