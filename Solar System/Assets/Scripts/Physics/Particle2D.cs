using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour{
    public float mass                  = 1;                 //mass of the object (cannot be les than 0)
    public Vector3 velocity            = Vector3.zero;      //the velocity of our object (will be use to chang position)
    public Vector3 gravity             = Vector3.zero;      //gravity of an object
    public Vector3 acceleration        = Vector3.zero;      //rate of speed chage
    public Vector3 accumulatedAccerlation = Vector3.zero;   //our accululated forces 
    public float dampingConstant       = 1;                 //damping for slow down
    public bool realative = false;

    protected ParticleCollider hitBox;
    protected Particle2D physics;                                 //reference to ourself
    protected ForceGeneratorManager forceGeneratorManager;        //manages all the force generattors
    protected CollisionManager CM;
    protected Game GM;
    protected GridPartition GP;
    public int currentLoc;

    [HideInInspector]
    public float invMass = 1;

    //validate certain inputs
    void OnValidate() {
        mass = Mathf.Max(mass, 0.00000000001f);
        invMass = 1 / mass;
        currentLoc = -1;
    }

    protected virtual void Awake() 
    {
        invMass = 1 / mass;
    }


    //grab the physics component
    protected virtual void Start() {
        //set up
        physics = gameObject.GetComponent<Particle2D>();
        forceGeneratorManager = gameObject.GetComponent<ForceGeneratorManager>();
        GP = FindObjectOfType<GridPartition>();

        hitBox = gameObject.GetComponent<ParticleCollider>();
        GM = GameObject.Find("GameManager").GetComponent<Game>();

        if (hitBox != null)
            GP.Add(hitBox);

        CM = FindObjectOfType<CollisionManager>();
        //GP = FindObjectOfType<GridPartition>();
    }

    //update physics
    protected virtual void FixedUpdate() {
        //get acculmative force
        if (forceGeneratorManager != null)
        {
            forceGeneratorManager.UpdateGenerators();
            accumulatedAccerlation += forceGeneratorManager.GetAcceleration(invMass);
        }

        //integrate
        accumulatedAccerlation += gravity;

        //collsion
        if (hitBox != null && GM.partitionsActive == false)
        {
            bool collided = CM.ResolveAllColliders(hitBox);

            //add to the score
            //if (collided && tag == "Target")
            //{
            //    GM.increaseScore();
            //}
        }

        accumulatedAccerlation.z = 0;

        //integrate
        if (GM.pause == false)
            Integrator.Integrate(physics, realative);

        accumulatedAccerlation = Vector3.zero;

        //GP.InitStartUp();
    }
}
